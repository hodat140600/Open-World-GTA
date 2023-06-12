using _SDK.Entities;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Assets._SDK.Entities
{
    public abstract class AbstractEntity : IEntity
    {
        [ShowInInspector, LabelWidth(50), GUIColor(1, .5f, .5f), PropertyOrder(-2)]
        public abstract int Id { get; }

        [field: SerializeField, LabelWidth(50), PropertyOrder(-1)]
        public string Name { get; set; }
    }
}