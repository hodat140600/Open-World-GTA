using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        public static void DestroyAllChildren(this Transform t)
        {
            foreach (Transform child in t)
                Object.Destroy(child.gameObject);
        }

        public static void DestroyImmediateAllChildren(this Transform t)
        {
            var tempList = t.Cast<Transform>().ToList();
            foreach (var child in tempList)
                Object.DestroyImmediate(child.gameObject);
        }

        public static T GetComponentInFirstLayerChildren<T>(this Transform transform) where T : Component
        {
            foreach (Transform children in transform)
            {
                if (children.TryGetComponent(out T component))
                    return component;
            }

            return null;
        }
    }
}