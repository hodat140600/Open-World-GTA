using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME._Scripts
{
    [Serializable, Title("SHOW DESCRIPTION")]
    public class ShowDescription : SubMission
    {
        [field: SerializeField, HideLabel]
        public string description { get; private set; }

        public override void Accept(MissionMonitor monitor)
        {
            // monitor.ShowDescription(this);
        }
    }
}