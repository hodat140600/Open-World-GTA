using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME._Scripts
{
    [CreateAssetMenu(fileName = "Map", menuName = "World/Map")]
    public class MapSettings : SerializedScriptableObject
    {
        [SerializeField]
        public Map Entity;
    }
}