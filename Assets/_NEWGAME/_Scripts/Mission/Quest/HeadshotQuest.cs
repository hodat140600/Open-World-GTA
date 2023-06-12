using System;
using _GAME._Scripts.Npc;
using MyBox;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME._Scripts
{
    [Serializable, Title("EYESHOT")]
    public class HeadshotQuest : SubMission
    {
        [field: SerializeField, HorizontalGroup("Design"), HideLabel, SuffixLabel("enemies", overlay: true)]
        public int Count { get; private set; }

        // [field: SerializeField, HorizontalGroup("Design", .8f), HideLabel, SearchableEnum]
        // public ModelType Type { get; private set; }

        public int CurrentCount { get; set; }

        public override string Progress => $"{CurrentCount}/{Count}";
        protected override string DefaultQuestDescription => $"Eyeshot {Count} enemies";

        public override void Accept(MissionMonitor monitor)
        {
            monitor.ConfigureHeadshotQuest(this);
        }

        public override void ResetProgress()
        {
            base.ResetProgress();
            CurrentCount = 0;
        }
    }
}