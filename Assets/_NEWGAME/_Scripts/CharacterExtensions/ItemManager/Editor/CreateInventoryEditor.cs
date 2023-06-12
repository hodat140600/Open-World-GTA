using _GAME._Scripts._CharacterController;
using UnityEditor;
using UnityEngine;

namespace _GAME._Scripts._ItemManager
{
    public class CreateInventoryEditor : EditorWindow
    {
        GUISkin skin;
        GameObject inventoryPrefab;
        ItemListData itemListData;
        Vector2 rect = new Vector2(500, 240);
        Texture2D m_Logo;

        [MenuItem("DatHQ/Inventory/ItemManager (Player Only)", false, 3)]
        public static void CreateNewInventory()
        {
            GetWindow(typeof(CreateInventoryEditor), true, "Item Manager Creator", true);
        }

        void OnGUI()
        {
            if (!skin) skin = Resources.Load("dSkin") as GUISkin;
            GUI.skin = skin;

            minSize = rect;
            m_Logo = Resources.Load("icon_d2") as Texture2D;
            GUILayout.BeginVertical("ITEM MANAGER CREATOR", "window", GUILayout.MaxHeight(100), GUILayout.MaxWidth(490));
            GUILayout.Label(m_Logo, GUILayout.MaxHeight(25));

            GUILayout.BeginVertical("box");
            EditorGUILayout.HelpBox("Go to the folder DatHQ/ItemManager/Prefabs to select a Inventory prefab", MessageType.Info);
            inventoryPrefab = EditorGUILayout.ObjectField("Inventory Prefab: ", inventoryPrefab, typeof(GameObject), false) as GameObject;
            EditorGUILayout.HelpBox("Go to the folder DatHQ/ItemManager/ItemListData to select a ItemListData or create a new one in the Inventory menu", MessageType.Info);
            itemListData = EditorGUILayout.ObjectField("Item List Data: ", itemListData, typeof(ItemListData), false) as ItemListData;

            if (inventoryPrefab != null)
            {
                EditorGUILayout.HelpBox("Please select a Inventory Prefab, you can find one at the Inventory/Prefabs Folder", MessageType.Warning);
            }

            GUILayout.EndVertical();

            GUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField("Need to know how it works?");
            if (GUILayout.Button("Video Tutorial"))
            {
                Application.OpenURL("");
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (inventoryPrefab != null && itemListData != null)
            {
                if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<ThirdPersonController>() != null)
                {
                    if (GUILayout.Button("Create"))
                        Create();
                }
                else
                    EditorGUILayout.HelpBox("Please select the Player to add this component", MessageType.Warning);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Created the ItemManager
        /// </summary>
        void Create()
        {
            if (Selection.activeGameObject != null)
            {
                var itemManager = Selection.activeGameObject.AddComponent<ItemManager>();
                if (inventoryPrefab != null)
                {
                    var _inventory = PrefabUtility.InstantiatePrefab(inventoryPrefab.gameObject) as GameObject;
                    _inventory.transform.SetParent(Selection.activeTransform);
                }

                itemManager.itemListData = itemListData;
                ItemManagerUtilities.CreateDefaultEquipPoints(itemManager);
            }
            else
                Debug.Log("Please select the Player to add this component.");

            Close();
        }
    }
}