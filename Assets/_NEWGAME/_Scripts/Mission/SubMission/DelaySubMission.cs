using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME._Scripts
{
    [Serializable, Title("DELAY")]
    public class DelaySubMission : SubMission
    {
        [field: SerializeField]
        public float DelaySeconds { get; private set; }

        public override void Accept(MissionMonitor monitor)
        {
            monitor.Delay(this);
        }
    }
}