using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu (fileName = "EnemySpawnSetting", menuName = "EnemySpawnSetting")]
public class SpawnEnemySetting : ScriptableObject
{
    public EnemySpawnerModel EnemySpawnModel;

#if UNITY_EDITOR
    private void OnValidate()
    {
        EnemySpawnModel.Name = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(this));
        if (!EditorApplication.isUpdating)
        {
            AssetDatabase.SaveAssetIfDirty(this);
        }
    }
#endif
}
