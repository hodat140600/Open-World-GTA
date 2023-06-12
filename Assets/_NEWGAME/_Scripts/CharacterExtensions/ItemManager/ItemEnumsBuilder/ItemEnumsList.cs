using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace _GAME._Scripts._ItemManager
{
    public class ItemEnumsList : ScriptableObject
    {
        [SerializeField, @HideInInspector]
        public List<string> itemTypeEnumValues = new List<string>();
        [SerializeField, @HideInInspector]
        public List<string> itemAttributesEnumValues = new List<string>();

        [SerializeField, @HideInInspector]
        public List<string> itemTypeEnumFormats = new List<string>();
        [SerializeField, @HideInInspector]
        public List<string> itemAttributesEnumFormats = new List<string>();


    }
}
