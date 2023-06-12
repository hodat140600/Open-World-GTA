using System.Collections.Generic;
using _SDK.Entities;
using Assets._SDK.Entities;
using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Assets._SDK.Game
{
    public abstract class AbstractGameResources : SerializedMonoBehaviour
    {
#if UNITY_EDITOR
        protected Dictionary<int, TEntitySettings> LoadEntitySettings<TEntitySettings, TEntity>(string path, string filter = "")
            where TEntity : IEntity
            where TEntitySettings : AbstractEntitySettings<TEntity>
        {
            var assetPaths = AssetDatabase.FindAssets(filter, new[] { path });

            Dictionary<int, TEntitySettings> dictionary = new();

            foreach (string assetPath in assetPaths)
            {
                TEntitySettings itemSetting =
                    AssetDatabase.LoadAssetAtPath<TEntitySettings>(AssetDatabase.GUIDToAssetPath(assetPath));
                dictionary.TryAdd(itemSetting.Entity.Id, itemSetting);
            }

            return dictionary;
        }
        
        protected List<T> LoadSkillSettings<T>(string path, string filter = "") where T : AbstractSkillSettings
        {
            var assets = AssetDatabase.FindAssets(filter, new string[] { path });
            List<T> list = new List<T>();

            for (int i = 0; i < assets.Length; i++)
            {
                T itemSetting = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(assets[i]));
                if (itemSetting != null)
                    list.Add(itemSetting);
            }

            return list;
        }

        protected List<T> LoadScriptableObject<T>(string path, string filter = "") where T : ScriptableObject
        {
            var assets = AssetDatabase.FindAssets(filter, new string[] { path });
            List<T> list = new List<T>();

            for (int i = 0; i < assets.Length; i++)
            {
                T itemSetting = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(assets[i]));
                if (itemSetting != null)
                    list.Add(itemSetting);
            }

            return list;
        }
        
        protected List<T> LoadPrefab<T>(string path, string filter = "") where T : MonoBehaviour
        {
            var     assets = AssetDatabase.FindAssets(filter, new string[] { path });
            List<T> list   = new List<T>();

            for (int i = 0; i < assets.Length; i++)
            {
                T itemSetting = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(assets[i]));
                if (itemSetting != null)
                    list.Add(itemSetting);
            }

            return list;
        }
#endif
    }
}