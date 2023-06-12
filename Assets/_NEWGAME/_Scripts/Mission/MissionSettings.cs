using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _GAME._Scripts
{
    [CreateAssetMenu(fileName = "Mission", menuName = "Mission"), HideMonoScript]
    public class MissionSettings : ScriptableObject
    {
        [field: SerializeField, HideLabel]
        public Mission Mission { get; private set; }

#if UNITY_EDITOR
        [Button("ADD", ButtonSizes.Large), GUIColor(0.27f, 1f, 0f, 1f)]
        private void AddSubMission()
        {
            Mission.subMissions.Add(null);
        }
        
        [Button("SAVE", ButtonSizes.Large), GUIColor(0, .6f, 1)]
        private void UpdateGroup()
        {
            AssetDatabase.SaveAssets();
        }
#endif
    }
}