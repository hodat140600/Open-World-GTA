using _SDK.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets._SDK.Entities
{
    public class AbstractEntitySettings<T> : SerializedScriptableObject where T: IEntity
    {
        public T Entity;
    }
}