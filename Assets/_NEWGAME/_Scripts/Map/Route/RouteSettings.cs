using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME._Scripts
{
    [CreateAssetMenu(fileName = "Route", menuName = "World/Route")]
    public class RouteSettings : SerializedScriptableObject
    {
        [HideLabel]
        public Route Route;
    }
}