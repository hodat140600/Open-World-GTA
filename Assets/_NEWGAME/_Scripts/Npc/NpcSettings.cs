using Sirenix.OdinInspector;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace _GAME._Scripts.Npc
{
    public abstract class NpcSettings : ScriptableObject
    {
        public abstract INpc Npc { get; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Npc.Name = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(this));
            if (!EditorApplication.isUpdating)
            {
                AssetDatabase.SaveAssetIfDirty(this);
            }
        }
        [Button("SAVE SETTINGS", ButtonSizes.Large)]
        public void SaveSettings()
        {
            AssetDatabase.SaveAssets();
        }
#endif
    }
}