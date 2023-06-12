using System;
using System.Collections.Generic;
using System.Linq;
using _GAME._Scripts;
using _GAME._Scripts.Game;
using _GAME._Scripts.GameResources;
using _GAME._Scripts.Npc;
using _GAME._Scripts.Npc.Enemy;
using _GAME._Scripts.Npc.Group;
using _GAME._Scripts.Npc.Pedestrian;
using _GAME._Scripts.Npc.SkillSystem;
using Sirenix.OdinInspector;
using TRavljen.UnitFormation;
using TRavljen.UnitFormation.Formations;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SpawnerController : SerializedMonoBehaviour
{
    private List<NpcGroupSettings> NpcGroupSettingsList => GameManager.Instance.Resources.NpcGroupResources.npcGroupSettings; // NPC o day chua co skill
    private NpcResources NpcResources => GameManager.Instance.Resources.NpcResources;
    private FormationController FormationController => Gameplay.Instance.FormationController;
    private Map Map => Gameplay.Instance.Map;
    private NpcResources Resources;

    public List<NpcGroup> groups;

    public void Init(NpcResources resources)
    {
        Resources = resources;
        //LoadNpcs();
    }

    public void LoadNpcs()
    {
        groups = new List<NpcGroup>();

        foreach (NpcGroup group in NpcGroupSettingsList.Select(groupSettings => groupSettings.npcGroup))
        {
            InitGroupGameObjects(group);
        }
    }

    public void InitGroupGameObjects(NpcGroup group)
    {
        InitGroup(group);
        Transform groupTransform = CreateGroupTransform(group.Name);

        foreach (int id in group.npcIds)
        {
            INpc npc = NpcResources.GetNpcById(id);

            GameObject npcGameObject = npc switch
            {
                Police     => CreatePoliceGameObject(npc),
                Pedestrian => CreatePedestrianGameObject(npc),
                Gangster   => CreateGangsterGameObject(npc),
                Enemy      => CreateEnemyGameObject(npc),
                _          => null
            };

            SetNpcToGroup(npcGameObject, group, groupTransform);
        }

        groups.Add(group);
    }

    private void InitGroup(NpcGroup group)
    {
        group.NpcList          = new List<GameObject>();
        group.currentFormation = FormationController.GetFormation(group.formationType, group.unitSpacing);
    }

    private Transform CreateGroupTransform(string groupName)
    {
        Transform groupTransform = new GameObject().transform;
        groupTransform.name = groupName;
        groupTransform.SetParent(transform);
        return groupTransform;
    }

    private static void SetNpcToGroup(GameObject policeGameObject, NpcGroup group, Transform groupTransform)
    {
        group.NpcList.Add(policeGameObject);
        policeGameObject.transform.SetParent(groupTransform);
    }

    private GameObject GetModelByTypeAndVariant(Npc npc)
    {
        return Resources.GetModelByTypeAndVariant(npc);
    }

    private GameObject CreatePoliceGameObject(INpc npc)
    {
        Police            police      = (Police)npc;
        GameObject        go          = Instantiate(GetModelByTypeAndVariant(police));
        PoliceSkillSystem skillSystem = go.AddComponent<PoliceSkillSystem>();
        skillSystem.police = police;
        skillSystem.InitPolice(skillSystem.police);
        go.SetActive(false);
        return go;
    }

    private GameObject CreatePedestrianGameObject(INpc npc)
    {
        Pedestrian            pedestrian  = (Pedestrian)npc;
        GameObject            go          = Instantiate(GetModelByTypeAndVariant(pedestrian));
        PedestrianSkillSystem skillSystem = go.AddComponent<PedestrianSkillSystem>();
        skillSystem.pedestrian = pedestrian;
        skillSystem.InitPedestrian(skillSystem.pedestrian);
        go.SetActive(false);
        return go;
    }

    private GameObject CreateGangsterGameObject(INpc npc)
    {
        Gangster            gangster    = (Gangster)npc;
        GameObject          go          = Instantiate(GetModelByTypeAndVariant(gangster));
        GangsterSkillSystem skillSystem = go.AddComponent<GangsterSkillSystem>();
        skillSystem.gangster = gangster;
        skillSystem.InitGangster(skillSystem.gangster);
        go.SetActive(false);
        return go;
    }

    private GameObject CreateEnemyGameObject(INpc npc)
    {
        Enemy            Enemy       = (Enemy)npc;
        GameObject       go          = Instantiate(GetModelByTypeAndVariant(Enemy));
        EnemySkillSystem skillSystem = go.AddComponent<EnemySkillSystem>();
        skillSystem.Enemy = Enemy;
        skillSystem.InitEnemy(skillSystem.Enemy);
        go.SetActive(false);
        return go;
    }

    private Vector3 GetSpawnPosition(GameObject objectSpawn)
    {
        return Vector3.zero;
        // Map.GetWaypointById(npc.Route.GetWaypointIdByIndex(npc.StartingRouteIndex)).Position;
    }

    public void SpawnGroup(NpcGroup group)
    {
        GameObject          leader          = group.NpcList[0];
        AIMoveSkillBehavior leaderMoveSkill = leader.GetComponent<AIMoveSkillBehavior>();
        float               leaderSpeed     = leaderMoveSkill.moveSpeed;

        int npcCount = group.NpcList.Count;

        UnitsFormationPositions formationPos = GetFormationPos(group, group.startingWaypointPosition, npcCount);

        for (int i = 0; i < npcCount; i++)
        {
            GameObject npcGameObject = group.NpcList[i];
            Vector3    npcPosition   = formationPos.UnitPositions[i];

            npcGameObject.transform.position    = npcPosition;
            npcGameObject.transform.eulerAngles = Random.Range(0, 360) * Vector3.up;
            npcGameObject.SetActive(true);
        }

        leaderMoveSkill.isLeader = true;
        UpdateGroupSpeed(group, leaderSpeed, formationPos);
    }

    public void UpdateGroupSpeed(NpcGroup group, float moveSpeed, UnitsFormationPositions formationPos)
    {
        for (int i = 0; i < group.NpcList.Count; i++)
            if (group.NpcList[i].TryGetComponent(out AIMoveSkillBehavior aiMoveSkillBehavior))
                aiMoveSkillBehavior.SetDestinationAndSpeed(formationPos.UnitPositions[i], moveSpeed);
    }

    private UnitsFormationPositions GetFormationPos(NpcGroup group, Vector3 position, int npcCount)
    {
        UnitsFormationPositions formationPos = CalculatePositionForEachUnit(position,
            position + new Vector3(100, 0, 100), npcCount, group.currentFormation);
        return formationPos;
    }

    public UnitsFormationPositions CalculatePositionForEachUnit(Vector3 destination, Vector3 facingPosition, int numOfUnit, IFormation formation)
    {
        Vector3       direction    = facingPosition - destination;
        float         angle        = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        List<Vector3> newPositions = FormationPositioner.GetAlignedPositions(numOfUnit, formation, destination, angle);
        return new UnitsFormationPositions(newPositions, angle);
    }

    public void SpawnAllGroups()
    {
        foreach (NpcGroup group in groups)
            SpawnGroup(group);
    }
}