using System;
using System.Collections.Generic;
using Assets._SDK.Entities;
using Sirenix.OdinInspector;
using TRavljen.UnitFormation.Formations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _GAME._Scripts.Npc.Group
{
    [Serializable]
    public class NpcGroup : AbstractEntity
    {
        public List<int>  npcIds;
        public IFormation currentFormation;

        [HideInInspector]
        public float unitSpacing = 2f;

        public int leaderId;

        public FormationType formationType;

        public List<GameObject> NpcList { get; set; }

        public override int Id => (nameof(NpcGroup) + Name).GetHashCode();
        
        [ReadOnly]
        public Vector3 startingWaypointPosition;
    }
}