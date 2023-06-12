using _GAME._Scripts;
using _GAME._Scripts.Game;
using _GAME._Scripts.Npc.SkillSystem;
using Assets._SDK.Skills;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class NPCSpawnerController : MonoBehaviour, IObserverObject
{
    [SerializeField]
    private float _minDistance = 25.0f;
    [SerializeField]
    private float _maxDistance = 60.0f;
    [SerializeField]
    const float _timeBetweenSpawn = 0.2f;

    private ObservableObject _observableSubject;
    private ObjectPoolController _objectPoolController;

    public ObjectPoolController ObjectPoolController { get => _objectPoolController; private set => _objectPoolController = value; }

    public void Initialized(ObjectPoolController objectPoolController, ObservableObject observableSubject)
    {
        ObjectPoolController = objectPoolController;
        _observableSubject = observableSubject;
        _observableSubject.AddObserver(this);
    }

    void Start()
    {
        SpawnNPC();
    }

    public void OnNotify()
    {
        ResetSpawn();
        NotifyOnStartDay();
    }

    public void OnNotify(IDataObserver enemySpawnModel)
    {
        var ts = new TypeSwitch()
        .Case((EnemySpawnerModel x) =>
        {
            NotifyNextbotSpawnToNPC();
            SpawnEnemies((EnemySpawnerModel)enemySpawnModel);
        })
        .Case((ClearEnemyCurrent x) =>
        {
            ClearSpawn();
        });
        ts.Switch(enemySpawnModel);
    }
    void NotifyNextbotSpawnToNPC()
    {
        foreach (var npcType in ObjectPoolController.Pedestrians)
        {
            foreach (var npc in npcType.Value)
            {
                npc.GetComponent<AIMeleeSkillBehaviour>().ActionOnNextbotSpawn();
            }
        }

    }
    void NotifyOnStartDay()
    {
        foreach (var npcType in ObjectPoolController.Pedestrians)
        {
            foreach (var npc in npcType.Value)
            {
                npc.GetComponent<AIMeleeSkillBehaviour>().ActionOnStartDay();
            }
        }
    }

    #region Spawn Enemy Netbox

    void ResetSpawn()
    {
        StopAllCoroutines();
        ClearSpawn();
    }

    void ClearSpawn()
    {
        Debug.Log("Khong co chay duoc");
        foreach (var item in ObjectPoolController.Enemies)
        {
            var temp = item.Value.Count;
            for (int i = 0; i < temp; i++)
            {
                ObjectPoolController.ReturnEnemyToPool(item.Value[0], item.Key.ToString());
                // Debug.Log("Key :" +item.Key +
                //     "\n Total : " + temp +
                //     "\n Total current: " + item.Value.Count +
                //     "\n Count :" + i);
            }
        }
    }

    private void SpawnEnemies(EnemySpawnerModel enemySpawnModel)
    {
        enemySpawnModel.enemySpawns.ForEach(enemySpawn =>
        {
            StartCoroutine(SpawnLimit(enemySpawn.enemySetting.Enemy.Id, enemySpawn.LimitInSpawn, enemySpawn.isEndLessSpawn));
        });
    }

    WaitForSeconds WaitForSeconds = new WaitForSeconds(_timeBetweenSpawn);
    IEnumerator SpawnLimit(int id, int maxNumber, bool isEndlessSpawn)
    {
        // TODO: Condition to stop spawn
        while (isEndlessSpawn)
        {
            var pos = GetPositionOnRing(ObjectPoolController.tommy.position, _minDistance, _maxDistance)/* + (Vector3.up * 3f)*/;
            ObjectPoolController.SpawnEnemy(id, pos);
            Debug.Log("Sinh ra ne");
            yield return WaitForSeconds;
            yield return new WaitUntil(() => ObjectPoolController.Enemies[id].Count < maxNumber);
        }
    }
    private Vector3 ToXZ(Vector2 aVec)
    {
        return new Vector3(aVec.x, 0, aVec.y);
    }

    private Vector3 GetPositionOnRing(Vector3 origin, float minRadius, float maxRadius)
    {
        var v = Random.insideUnitCircle;
        return ToXZ(v.normalized * minRadius + v * (maxRadius - minRadius)) + origin;
    }
    private Vector3 GetRandomPositionOnNavmesh(Vector3 startPos, float distance)
    {
        return NavMeshUtil.GetRandomPoint(startPos, distance);
    }

    #endregion

    #region Spawn NPC Pedestrians

    void SpawnNPC()
    {
        GameManager.Instance.Resources.NpcResources.pedestrianSettings.ForEach((npc) =>
        {
            Vector3 positionWaypoint = Gameplay.Instance.Map.GetWaypointById(npc.pedestrian.routeSettings.Route.waypointsIds[npc.pedestrian.startWaypoint]).Position + (Vector3.up * 3);
            ObjectPoolController.SpawnNPC(npc.Npc.Id, positionWaypoint);
        });
    }
    //_GAME._Scripts._CharacterController._AI.Waypoint RandomWaypoint()
    //{
    //    var _nodes = Gameplay.Instance.waypointArea.GetValidPoints();
    //    var index = Random.Range(0, _nodes.Count - 1);
    //    if (_nodes != null && _nodes.Count > 0 && index < _nodes.Count)
    //        return _nodes[index];
    //    return null;
    //}
    #endregion
}
