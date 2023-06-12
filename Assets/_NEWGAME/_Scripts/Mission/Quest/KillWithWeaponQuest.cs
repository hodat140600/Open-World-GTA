using System;
using _GAME._Scripts.Inventory;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME._Scripts
{
    [Serializable, Title("KILL WITH WEAPON")]
    public class KillWithWeaponQuest : SubMission
    {
        [field: SerializeField, HorizontalGroup("Design"), HideLabel]
        public int Count { get; private set; }

        [field: SerializeField, HorizontalGroup("Design", .8f), HideLabel, EnumToggleButtons, EnumPaging, GUIColor(0f, 1f, 0.84f, 1f)]
        public WeaponType WeaponType { get; private set; }

        public int CurrentCount { get; set; }

        public override string Progress => $"{CurrentCount}/{Count}";
        protected override string DefaultQuestDescription => $"Kill using {WeaponType}";

        public override void Accept(MissionMonitor monitor)
        {
            monitor.ConfigureKillWithWeaponQuest(this);
        }

        public override void ResetProgress()
        {
            base.ResetProgress();
            CurrentCount = 0;
        }
    }
}