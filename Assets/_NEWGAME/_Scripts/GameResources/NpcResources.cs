using System;
using System.Linq;
using System.Collections.Generic;
using _GAME._Scripts.Npc;
using _GAME._Scripts.Npc.Enemy;
using _GAME._Scripts.Npc.Pedestrian;
using Assets._SDK.Game;
using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _GAME._Scripts.GameResources
{
    public class NpcResources : AbstractGameResources
    {
        private const string NPC_SKILL_SETTINGS_DIRECTORY   = "Assets/_NEWGAME/_Settings/Npc/Skill";
        private const string NPC_SETTINGS_DIRECTORY         = "Assets/_NEWGAME/_Settings/Npc";
        private const string SPAWN_ENEMY_SETTINGS_DIRECTORY = "Assets/_NEWGAME/_Settings/EnemySpawn";
        private const string NPC_MODEL_PEOPLE_DIR           = "Assets/_NEWGAME/Prefabs/Characters/NPC/People/";
        private const string NPC_MODEL_NEXTBOTS_DIR         = "Assets/_NEWGAME/Prefabs/Characters/NPC/Nextbots/";
        
        public List<AbstractSkillSettings> npcSkillSettings;

        public List<PedestrianSettings> pedestrianSettings;

        public List<EnemySettings> enemySettings;

        public List<SpawnEnemySetting> spawnEnemySettings;

        [SerializeField]
        private Dictionary<int, NpcSettings> _npcById;

        [field: SerializeField]
        public Dictionary<ModelType, Sprite> AvatarByModelType { get; private set; }

        public Dictionary<int, NpcSettings> NpcById => _npcById;

        [SerializeField]
        public Dictionary<ModelType, List<GameObject>> ModelTypeToModel;

        public INpc GetNpcById(int npcId)
        {
            return NpcById[npcId].Npc;
        }

        public Dictionary<int, NpcSettings> GetNpcsByIds(List<int> npcIds)
        {
            Dictionary<int, NpcSettings> npcs = new Dictionary<int, NpcSettings>();
            foreach (var id in npcIds)
            {
                var temp = NpcById.FirstOrDefault(_ => _.Key == id);
                npcs.Add(temp.Key, temp.Value);
            }

            return npcs;
        }

#if UNITY_EDITOR
        private void LoadNpc()
        {
            npcSkillSettings   = LoadSkillSettings<AbstractSkillSettings>(NPC_SKILL_SETTINGS_DIRECTORY);
            pedestrianSettings = LoadScriptableObject<PedestrianSettings>(NPC_SETTINGS_DIRECTORY);
            enemySettings      = LoadScriptableObject<EnemySettings>(NPC_SETTINGS_DIRECTORY);

            MapToDictionary();
        }

        private void MapToDictionary()
        {
            _npcById = new Dictionary<int, NpcSettings>();

            foreach (PedestrianSettings p in pedestrianSettings)
                NpcById.Add(p.pedestrian.Id, p);

            foreach (EnemySettings e in enemySettings)
                NpcById.Add(e.Enemy.Id, e);
        }

        private void LoadModels()
        {
            ModelTypeToModel = new Dictionary<ModelType, List<GameObject>>();

            List<ModelType> NextbotsTypes = new()
            {
                ModelType.AngryMunci,
                ModelType.Armstrong,
                ModelType.BingChilling,
                ModelType.Grudges,
                ModelType.Juggler,
                ModelType.Obunga,
                ModelType.Pinheadditto,
                ModelType.Quandaledingle,
                ModelType.SeleneDelgado,
                ModelType.SuperIdol,
                ModelType.Yoshie,
                ModelType.ZhongXina,
                ModelType.Nerd
            };

            foreach (ModelType type in Enum.GetValues(typeof(ModelType)))
            {
                ModelTypeToModel.Add(type, new List<GameObject>());
                string path = (NextbotsTypes.Contains(type) ? NPC_MODEL_NEXTBOTS_DIR : NPC_MODEL_PEOPLE_DIR) + type;
                ModelTypeToModel[type] = LoadGameObjects(path);
            }

            AssetDatabase.SaveAssets();
        }

        private List<GameObject> LoadGameObjects(string path)
        {
            string[]         assetPaths = AssetDatabase.FindAssets("", new[] { path });
            List<GameObject> list       = new();

            foreach (string assetPath in assetPaths)
            {
                GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(assetPath));
                list.Add(gameObject);
            }

            return list;
        }

        private void LoadEnemySpawnSettings()
        {
            spawnEnemySettings = LoadScriptableObject<SpawnEnemySetting>(SPAWN_ENEMY_SETTINGS_DIRECTORY);
        }

        [Button("Load Resources", ButtonSizes.Medium)]
        public void LoadNPCResources()
        {
            LoadModels();
            LoadNpc();
            LoadEnemySpawnSettings();
            Debug.Log($"Finished. Retrieved:" +
                      $"\n    {npcSkillSettings.Count} skills" +
                      $"\n    {pedestrianSettings.Count} pedestrians" +
                      $"\n    {enemySettings.Count} enemies" +
                      $"\n    {ModelTypeToModel.Count} model types");
        }
#endif

        public GameObject GetModelByTypeAndVariant(Npc.Npc npc)
        {
            var Models        = ModelTypeToModel[npc.ModelType];
            var ActualVariant = Mathf.Clamp(npc.Variant - 1, 0, Models.Count - 1);
            return ModelTypeToModel[npc.ModelType][ActualVariant];
        }

        public Dictionary<int, GameObject> GetAllModelNPC()
        {
            Dictionary<int, GameObject> models = new Dictionary<int, GameObject>();
            foreach (var npc in NpcById)
            {
                models.Add(npc.Key, GetModelByTypeAndVariant((Npc.Npc)npc.Value.Npc));
            }

            return models;
        }
    }
}