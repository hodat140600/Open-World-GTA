using _GAME._Scripts;
using _GAME._Scripts.Game;
using _GAME._Scripts.Npc;
using _GAME._Scripts.Npc.Enemy;
using _GAME._Scripts.Npc.Pedestrian;
using _GAME._Scripts.Npc.SkillSystem;
using MyBox;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ClassHeader("ObjectPoolController", iconName = "ThirdPersonExtensions/inputIcon")]
public class ObjectPoolController : ExtendMonoBehaviour
{
    public GameResources Resources => GameManager.Instance.Resources;
    public Transform tommy;

#if UNITY_EDITOR
    public IntGameObjectArrayDictionary npcs;
    public IDictionary<int, List<GameObject>> IntGameObjectDictionary
    {
        get { return npcs; }
        set { npcs.CopyFrom(value); }
    }
    public Dictionary<int, List<GameObject>> EnemiesEditor { get => npcs; private set => IntGameObjectDictionary = value; }
#endif
    [SerializeField] private Dictionary<int, List<GameObject>> enemies = new Dictionary<int, List<GameObject>>();
    [SerializeField] private Dictionary<int, List<GameObject>> pedestrians = new Dictionary<int, List<GameObject>>();
    [SerializeField] private Dictionary<int, NpcSettings> npcSettings;
    [SerializeField] private Dictionary<int, GameObject> models;
    [EditorToolbar("NPC Pooler", order = 0)]
    [SerializeField] private MyPooler.ObjectPooler npcPooler;
    [SerializeField] private ObservableObject _observableSubject;

    [EditorToolbar("ProjectilePooler", order = 2)]
    [SerializeField] private MyPooler.ObjectPooler projectilePooler;
    public Dictionary<int, List<GameObject>> Enemies { get => enemies; private set => enemies = value; }
    public Dictionary<int, List<GameObject>> Pedestrians { get => pedestrians; private set => pedestrians = value; }


    private void OnEnable()
    {
#if UNITY_EDITOR
        IntGameObjectDictionary = new Dictionary<int, List<GameObject>>();
#endif
        npcPooler ??= CreateInstancePooler(nameof(npcPooler));
        //projectilePooler ??= CreateInstancePooler(nameof(projectilePooler));

        InitNPCPooling();
        InitNPC();
    }

    void InitNPC()
    {
        foreach (var item in models)
        {
            Enemies.Add(item.Key, new List<GameObject>());
            Pedestrians.Add(item.Key, new List<GameObject>());
        }
    }


    public void SpawnNPC(int id, Vector3 position)
    {
        Pedestrians[id].Add(GetNPCFromPool<PedestrianSkillSystem>(id.ToString(), position));
    }

    public void SpawnEnemy(int id, Vector3 position)
    {
        Enemies[id].Add(GetNPCFromPool<EnemySkillSystem>(id.ToString(), position));
#if UNITY_EDITOR
        EnemiesEditor = Enemies;
#endif
    }

    public void ReturnNPCToPool(GameObject npc, string id)
    {
        ReturnToPool(npc, id, Pedestrians);
    }

    public void ReturnEnemyToPool(GameObject npc, string id)
    {
        ReturnToPool(npc, id, Enemies);
    }

    #region Pooling Modules

    /// <summary>
    /// Get Npc From Pool with IdNpc
    /// </summary>
    /// <param name="id"></param>
    /// <param name="position"></param>
    public GameObject GetNPCFromPool<T>(string id, Vector3 position) where T : NpcSkillSystem
    {
        var npc = npcPooler.GetFromPool(id, position, Quaternion.identity);

        if (npc.TryGetComponent(out T npcSkillSystem))
        {
            npcSkillSystem.UpdateSkillSlot<AIMeleeSkillSlot>();
            return npc;
        }
        CreateNPCSkillSystem<T>(npc, int.Parse(id));
        return npc;
    }
    /// <summary>
    /// Return Npc To Pool with IdNpc
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="id"></param>
    public void ReturnToPool(GameObject npc, string id, Dictionary<int, List<GameObject>> dictionary)
    {
        npcPooler.ReturnToPool(id, npc);
        dictionary[int.Parse(id)].Remove(npc);
    }

    private void CreateNPCSkillSystem<T>(GameObject npc, int id) where T : NpcSkillSystem
    {
        npcSettings.TryGetValue(id, out NpcSettings Settings);
        NpcSkillSystem skillSystem = npc.AddComponent<T>();
        skillSystem = Settings.Npc switch
        {
            Pedestrian => CreatePedestrianGameObject((PedestrianSkillSystem)skillSystem, Settings),
            Enemy => CreateEnemyGameObject((EnemySkillSystem)skillSystem, Settings),
            _ => null
        };
    }

    private PedestrianSkillSystem CreatePedestrianGameObject(PedestrianSkillSystem skillSystem, NpcSettings npcSettings)
    {
        skillSystem.pedestrian = (Pedestrian)npcSettings.Npc;
        //var idWaypoint = skillSystem.pedestrian.routeSettings.Route.waypointsIds[skillSystem.pedestrian.startWaypoint];
        //var positionWaypoint = Gameplay.Instance.Map.GetWaypointById(idWaypoint).Position;
        //skillSystem.gameObject.transform.position = positionWaypoint;
        skillSystem.InitPedestrian(skillSystem.pedestrian);
        skillSystem.GetSkillBehaviour<AIMeleeSkillSlot, AIMeleeSkillBehaviour>(out AIMeleeSkillBehaviour aIMeleeSkillBehaviour);
        aIMeleeSkillBehaviour.InitPooling(this);
        aIMeleeSkillBehaviour.InitNavigator(new RouteNavigator(skillSystem.pedestrian.routeSettings.Route, skillSystem.pedestrian.startWaypoint, Gameplay.Instance.Map));
        return skillSystem;
    }
    private EnemySkillSystem CreateEnemyGameObject(EnemySkillSystem skillSystem, NpcSettings npcSettings)
    {
        skillSystem.Enemy = (Enemy)npcSettings.Npc;
        skillSystem.InitEnemy(skillSystem.Enemy);
        skillSystem.GetSkillBehaviour<AIMeleeSkillSlot, AIMeleeSkillBehaviour>(out AIMeleeSkillBehaviour aIMeleeSkillBehaviour);
        aIMeleeSkillBehaviour.InitPooling(this);
        return skillSystem;
    }


    private void InitNPCPooling()
    {
        npcSettings = Resources.NpcResources.NpcById;
        models = Resources.NpcResources.GetAllModelNPC();
        npcPooler.pools = new List<MyPooler.ObjectPooler.Pool>();
        int index = 0;
        foreach (var model in models)
        {
            npcPooler.pools.Add(new MyPooler.ObjectPooler.Pool());
            npcPooler.CreatePoolDictionary(model.Value, index, 10, 5, model.Key.ToString());
            index++;
        }
        npcPooler.CreatePools();
    }

    private MyPooler.ObjectPooler CreateInstancePooler(string nameObject)
    {
        this.gameObject.AddComponent<NPCSpawnerController>().Initialized(this, _observableSubject);
        var gameObject = new GameObject(nameObject, typeof(MyPooler.ObjectPooler));
        gameObject.transform.SetParent(this.transform);
        return gameObject.GetComponent<MyPooler.ObjectPooler>();
    }

    #endregion
}
public static class ObjectPoolingUtils
{
    public static void CreatePoolDictionary(this MyPooler.ObjectPooler objectPooler, GameObject prefab, int typeObject, int amount, int extensionLimit, string tag = "")
    {
        if (tag.IsNullOrEmpty()) tag = prefab.name;
        objectPooler.pools[typeObject].prefab = prefab;
        objectPooler.pools[typeObject].tag = tag;
        objectPooler.pools[typeObject].shouldExpandPool = true;
        objectPooler.pools[typeObject].amount = amount;
        objectPooler.pools[typeObject].extensionLimit = extensionLimit;
    }
}

[Serializable]
public class GameObjectArrayStorage : SerializableDictionary.Storage<List<GameObject>> { }

[Serializable]
public class IntGameObjectArrayDictionary : SerializableDictionary<int, List<GameObject>, GameObjectArrayStorage> { }