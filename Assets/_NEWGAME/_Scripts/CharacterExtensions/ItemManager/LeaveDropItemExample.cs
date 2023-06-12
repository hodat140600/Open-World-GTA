using UnityEngine;
using System.Collections;

namespace _GAME._Scripts._ItemManager
{
    public class LeaveDropItemExample : MonoBehaviour
    {
        ItemManager itemManager;
        private Rect windowRect;
        private Vector2 scroll;

        void OnGUI()
        {
            windowRect = GUILayout.Window(0, windowRect, vLeaveDropItensWindow, "Leave and Drop Items test by DatHQ:");
        }

        private void vLeaveDropItensWindow(int windowID)
        {
            GUILayout.BeginVertical();
            itemManager = FindObjectOfType<ItemManager>();

            if (itemManager)
            {
                scroll = GUILayout.BeginScrollView(scroll, GUILayout.Width(300), GUILayout.MinHeight(300));
                for (int i = 0; i < itemManager.items.Count; i++)
                {
                    GUILayout.BeginHorizontal("box");

                    GUILayout.Label(new GUIContent("Name:" + itemManager.items[i].name + "\nAmount :" + itemManager.items[i].amount.ToString()), GUILayout.Width(200), GUILayout.Height(40));
                    GUILayout.BeginVertical("box");
                    if (GUILayout.Button("Leave"))
                    {
                        //itemManager.temporarilyIgnoreItemAnimation = true;
                        //itemManager.inventory.OnDestroyItem(itemManager.items[i], 1);
                        //itemManager.temporarilyIgnoreItemAnimation = false;
                        break;
                    }
                    if (GUILayout.Button("Drop"))
                    {
                        //itemManager.temporarilyIgnoreItemAnimation = true;
                        //itemManager.inventory.OnDropItem(itemManager.items[i], 1);
                        //itemManager.temporarilyIgnoreItemAnimation = false;
                        break;
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
            GUI.DragWindow();
        }
    }
}