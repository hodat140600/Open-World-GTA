using _GAME._Scripts.Npc;
using _GAME._Scripts.Npc.Enemy;
using _SDK.Entities;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEditor;
using UnityEngine;

[Serializable]
public class EnemySpawnerModel : IEntity, IDataObserver
{
    // [HideInInspector]
    public int Id => (nameof(EnemySpawnerModel) + Name).GetHashCode();

    // [HideInInspector]
    public string Name { get; set; }

    public List<EnemySpawn> enemySpawns;

    public ModelType GetModelType(int index)
    {
        return enemySpawns[index].enemySetting.Npc.ModelType;
    }

    public int GetNumberInOneSpawn(int index)
    {
        return enemySpawns[index].LimitInSpawn;
    }

    [Serializable]
    public class EnemySpawn
    {
        [SerializeField, ValueDropdown("GetAllScriptableObjects", IsUniqueList = true, DropdownTitle = "Select EnemySettings", DrawDropdownForListElements = true, ExcludeExistingValuesInList = true)]
        public EnemySettings enemySetting;

        [field: SerializeField, Range(1, 20), Tooltip("Number spawn in a wave")]
        public int LimitInSpawn = 1;

        [PropertySpace(3, 3)]
        [Button("EndLess Spawn", ButtonSizes.Large)]
        [ShowIf("@this.isEndLessSpawn == false", false)]
        [HorizontalGroup("Button", .4f)]
        private void EndlessOn()
        {
            isEndLessSpawn = true;
        }

        [PropertySpace(3, 3)]
        [Button("EndLess Spawn", ButtonSizes.Large)]
        [ShowIf("@this.isEndLessSpawn == true", false)]
        [HorizontalGroup("Button", .4f)]
        [GUIColor(0.27f, 1f, 0f, 1f)]
        private void EndlessOff()
        {
            isEndLessSpawn = false;
        }

        [HideInInspector]
        public bool isEndLessSpawn = true;

        //[OnValueChanged(nameof(LinkToNPCSetting))]
        //[field: SerializeField, BoxGroup("Model"), HideLabel, EnumToggleButtons]
        //public ModelType modelType;

#if UNITY_EDITOR
        private static IEnumerable GetAllScriptableObjects()
        {
            return AssetDatabase.FindAssets("t:EnemySettings")
                .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x))
                .Select(x => new ValueDropdownItem(x, AssetDatabase.LoadAssetAtPath<EnemySettings>(x)));
        }
        //private void LinkToNPCSetting()
        //{

        //    AssetDatabase.SaveAssets();
        //}
#endif
    }
}
