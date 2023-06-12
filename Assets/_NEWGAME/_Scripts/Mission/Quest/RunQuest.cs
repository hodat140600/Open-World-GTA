using System;
using _GAME._Scripts.Npc;
using MyBox;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME._Scripts
{
    [Serializable, Title("RUN")]
    public class RunQuest : SubMission
    {
        [field: SerializeField, HideLabel, SuffixLabel("m", true)]
        public float Distance { get; private set; }

        public float CurrentDistance { get; set; }

        public override string Progress => $"{CurrentDistance:F0}/{Distance:F0}";
        protected override string DefaultQuestDescription => $"Run {Distance}m";

        public override void Accept(MissionMonitor monitor)
        {
            monitor.ConfigureRunQuest(this);
        }

        public override void ResetProgress()
        {
            base.ResetProgress();
            CurrentDistance = 0;
        }
    }
}