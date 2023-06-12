using System;
using _GAME._Scripts.Npc;
using MyBox;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME._Scripts
{
    [Serializable, Title("SPAWN NPC (POOLING)")]
    public class SpawnNpc : SubMission
    {
        //[SerializeField, Searchable]
        //public SpawnEnemySetting SpawnEnemySetting;

        [SerializeField, HideLabel]
        public EnemySpawnerModel SpawnEnemySetting;

        public int KilledCount { get; set; }
        public int HeadshotCount { get; set; }
        public int Score => KilledCount + HeadshotCount;

        public override void Accept(MissionMonitor monitor)
        {
            monitor.SpawnNpc(this);
        }

        public override void ResetProgress()
        {
            base.ResetProgress();
            KilledCount   = 0;
            HeadshotCount = 0;
        }
    }
}