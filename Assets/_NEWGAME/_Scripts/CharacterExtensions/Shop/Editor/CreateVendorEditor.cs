using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
namespace _GAME._Scripts
{
    public class CreateVendorEditor : EditorWindow
    {
        GUISkin skin;
        GameObject vendorObj;
        GameObject vendorCanvas;
        ItemListDataWrapper itemListDataWrapper;
        Editor humanoidpreview;
        RuntimeAnimatorController controller;
        int layer;
        LayerMask layerMask;
        [MenuItem("Shop/Vendor/Create Vendor", false, 1)]
        public static void CreateNewCharacter()
        {
            GetWindow<CreateVendorEditor>();
        }

        void OnEnable()
        {
            //m_Logo = Resources.Load("icon_v2") as Texture2D;
            vendorObj = Selection.activeGameObject;

            if (vendorObj)
            {
                //charAnimator = charObj.GetComponent<Animator>();
                humanoidpreview = Editor.CreateEditor(vendorObj);
            }

        }

        void OnGUI()
        {
            if (!skin) skin = Resources.Load("dSkin") as GUISkin;
            GUI.skin = skin;
            GUILayout.BeginVertical("Vendor Creator Window", "window");
            GUILayout.Space(30);
            GUILayout.BeginVertical("box");

            vendorObj = EditorGUILayout.ObjectField("Vendor Model", vendorObj, typeof(GameObject), true, GUILayout.ExpandWidth(true)) as GameObject;
            controller = EditorGUILayout.ObjectField("Animator Controller: ", controller, typeof(RuntimeAnimatorController), false) as RuntimeAnimatorController;
            vendorCanvas = EditorGUILayout.ObjectField("Vendor Canvas", vendorCanvas, typeof(GameObject), true, GUILayout.ExpandWidth(true)) as GameObject;
            itemListDataWrapper = EditorGUILayout.ObjectField("ItemListDataWrapper", itemListDataWrapper, typeof(ItemListDataWrapper), true, GUILayout.ExpandWidth(true)) as ItemListDataWrapper;

            layer = EditorGUILayout.Popup("Vendor Layer", layer, GetLayerMaskNames());
            Debug.Log(layer);

            if (GUI.changed && vendorObj != null)
                humanoidpreview = Editor.CreateEditor(vendorObj);

            if (CanCreate())
            {
                DrawHumanoidPreview();
                if (GUILayout.Button("Create"))
                    Create();
            }
            GUILayout.EndVertical();
            GUILayout.EndVertical();
        }

        string[] GetLayerMaskNames()
        {
            List<string> toReturn = new List<string>();
            for (int i = 0; i < 32; i++)
            {
                string name = LayerMask.LayerToName(i);
                if (name == "")
                {
                    name = "EmptyLayer";
                }
                toReturn.Add(name);
            }
            return toReturn.ToArray();
        }

        void DrawHumanoidPreview()
        {
            GUILayout.FlexibleSpace();

            if (humanoidpreview != null)
            {
                humanoidpreview.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(100, 400), "window");
            }
        }

        bool CanCreate()
        {
            if (vendorObj && vendorCanvas && itemListDataWrapper)
                return true;

            return false;
        }

        void Create()
        {
            GameObject newVendor = InstantiateNewCharacter(vendorObj);
            newVendor.name = "Vendor_" + vendorObj.gameObject.name;
            newVendor.transform.position = Vector3.zero;

            var rigidbody = newVendor.GetComponent<Rigidbody>();
            if (rigidbody == null)
                rigidbody = newVendor.AddComponent<Rigidbody>();


            var collider = newVendor.GetComponent<CapsuleCollider>();
            if (collider == null)
                collider = newVendor.AddComponent<CapsuleCollider>();

            var vendor = newVendor.AddComponent<Vendor>();
            var camera = Instantiate(Resources.Load("VendorCamera")) as GameObject;
            var canvas = Instantiate(vendorCanvas) as GameObject;
            var trigger = Instantiate(Resources.Load("VendorTrigger")) as GameObject;

            var animator = newVendor.GetComponent<Animator>();
            if (animator == null)
                animator = newVendor.AddComponent<Animator>();

            camera.transform.SetParent(newVendor.transform);
            canvas.transform.SetParent(newVendor.transform);
            trigger.transform.SetParent(newVendor.transform);
            trigger.transform.position = Vector3.zero;

            vendor.itemListDataWrapper = itemListDataWrapper;

            animator.runtimeAnimatorController = controller;


            camera.GetComponent<Camera>().cullingMask = 1 << layer;


            Transform[] tArray = newVendor.GetComponentsInChildren<Transform>();
            for (int i = 0; i < tArray.Length; i++)
            {
                if (tArray[i].GetComponent<SkinnedMeshRenderer>())
                    tArray[i].gameObject.layer = layer;

            }
            // rigidbody
            rigidbody.useGravity = true;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbody.mass = 50;

            // capsule collider 
            collider.height = ColliderHeight(vendorObj.GetComponent<Animator>());
            collider.center = new Vector3(0, (float)System.Math.Round(collider.height * 0.5f, 2), 0);
            collider.radius = (float)System.Math.Round(collider.height * 0.15f, 2);
            Selection.activeGameObject = newVendor;
            SceneView.lastActiveSceneView.FrameSelected();
            Close();

        }
        private GameObject InstantiateNewCharacter(GameObject selected)
        {
            if (selected == null) return selected;
            if (selected.scene.IsValid()) return selected;

            return PrefabUtility.InstantiatePrefab(selected) as GameObject;

        }
        float ColliderHeight(Animator animator)
        {
            var foot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            var hips = animator.GetBoneTransform(HumanBodyBones.Hips);
            return (float)System.Math.Round(Vector3.Distance(foot.position, hips.position) * 2f, 2);
        }
    }
}