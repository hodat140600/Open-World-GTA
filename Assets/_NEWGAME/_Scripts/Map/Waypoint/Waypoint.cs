using System;
using System.Collections.Generic;
using _SDK.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME._Scripts
{
    [Serializable]
    public class Waypoint : IEntity
    {
        [ShowInInspector, PropertyOrder(-1), HideLabel, GUIColor(1, .2f, .2f)]
        public int Id => (nameof(Waypoint) + Name).GetHashCode();

        [field: SerializeField, HideLabel, HorizontalGroup("Inline", .25f)]
        public string Name { get; set; }

        [SerializeField, HideLabel, HorizontalGroup("Inline/Pos")]
        public Vector3 Position;

        [SerializeField]
        public List<int> neighbourIds;
        
        public Waypoint(string name, Vector3 position, List<int> neighbourIds)
        {
            Name              = name;
            Position          = position;
            this.neighbourIds = neighbourIds;
        }
    }
}