using System;
using _GAME._Scripts.Npc;
using MyBox;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME._Scripts
{
    [Serializable, Title("KILL")]
    public class KillNpcQuest : SubMission
    {
        [field: SerializeField, HorizontalGroup("Design"), HideLabel]
        public int Count { get; private set; }

        [field: SerializeField, HorizontalGroup("Design", .8f), HideLabel, SearchableEnum]
        public ModelType Type { get; private set; }

        public int CurrentCount { get; set; }

        public override string Progress => $"{CurrentCount}/{Count}";
        protected override string DefaultQuestDescription => $"Kill {Type}";

        public override void Accept(MissionMonitor monitor)
        {
            monitor.ConfigureKillQuest(this);
        }

        public override void ResetProgress()
        {
            base.ResetProgress();
            CurrentCount = 0;
        }
    }
}