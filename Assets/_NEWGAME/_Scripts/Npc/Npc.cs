using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _GAME._Scripts.Npc
{
    [Serializable]
    public class Npc : INpc
    {
        [ShowInInspector, ReadOnly, LabelWidth(50), GUIColor(1, .5f, .5f), PropertyOrder(-2)]
        public int Id => _id;
        // (nameof(Npc) + Name).GetHashCode();

        [SerializeField]
        private int _id;

        [field: SerializeField, LabelWidth(50), GUIColor(1, 0.8f, 0.4f), PropertyOrder(-1),
         InfoBox("Name field is affected by filename.\nIf you wish to rename the character, rename the filename instead."), OnValueChanged(nameof(FixIdOnBuild))]
        public string Name { get; set; }
        
        [field: SerializeField, BoxGroup("Model"), HideLabel, EnumToggleButtons]
        public ModelType ModelType { get; private set; }

        [field: SerializeField, HorizontalGroup("Model/Variant"), Range(1, 6),
                Tooltip("Variant of the model type")]
        public int Variant { get; private set; }

#if UNITY_EDITOR
        [HorizontalGroup("Model/Variant", .2f), Button("Random", ButtonSizes.Medium), GUIColor(0, 1, 0)]
        private void RandomVariant()
        {
            Variant = Random.Range(1, 7);
            AssetDatabase.SaveAssets();
        }

#endif
        void FixIdOnBuild()
        {
            _id = (nameof(Npc) + Name).GetHashCode();
        }
    }
}