using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _GAME._Scripts._Melee
{
    using _GAME._Scripts;
    using _GAME._Scripts._Camera;
    using _GAME._Scripts._CharacterController;
    using _GAME._Scripts._ItemManager;

    public class CreateMeleeCharacterEditor : EditorWindow
    {
        GUISkin skin;
        public GameObject template;

        public bool useGameController = true;
        public bool useInventory = true;

        public GameObject inventory;
        public ItemListData itemListData;

        public GameObject charObj;
        Animator charAnimator;

        Vector2 rect = new Vector2(500, 660);
        Editor humanoidpreview;
        Texture2D m_Logo;

        /// <summary>
        /// 3rdPersonController Menu 
        /// </summary>    
        [MenuItem("DatHQ/Melee Combat/Create Melee Controller", false, 1)]
        public static void CreateNewCharacter()
        {
            GetWindow<CreateMeleeCharacterEditor>();
        }

        bool isHuman, isValidAvatar, charExist;
        public virtual void OnEnable()
        {
            m_Logo = Resources.Load("ThirdPersonExtensions/icon_d2") as Texture2D;
            if (Selection.activeObject)
            {
                charObj = Selection.activeGameObject;
            }
            if (charObj)
            {
                charAnimator = charObj.GetComponent<Animator>();
                humanoidpreview = Editor.CreateEditor(charObj);
            }

            charExist = charAnimator != null;
            isHuman = charExist ? charAnimator.isHuman : false;
            isValidAvatar = charExist ? charAnimator.avatar.isValid : false;
        }

        public virtual void OnGUI()
        {
            if (!skin)
            {
                skin = Resources.Load("dSkin") as GUISkin;
            }

            GUI.skin = skin;

            minSize = rect;
            titleContent = new GUIContent("Character", null, "Third Person Character Creator");
            GUILayout.BeginVertical("Character Creator Window", "window");
            GUILayout.Label(m_Logo, GUILayout.MaxHeight(25));
            GUILayout.Space(5);

            GUILayout.BeginVertical("box");

            if (!charObj)
            {
                EditorGUILayout.HelpBox("Make sure your FBX model is set as Humanoid!", MessageType.Info);
            }
            else if (!charExist)
            {
                EditorGUILayout.HelpBox("Missing a Animator Component", MessageType.Error);
            }
            else if (!isHuman)
            {
                EditorGUILayout.HelpBox("This is not a Humanoid", MessageType.Error);
            }
            else if (!isValidAvatar)
            {
                EditorGUILayout.HelpBox(charObj.name + " is a invalid Humanoid", MessageType.Info);
            }

            template = EditorGUILayout.ObjectField("Template", template, typeof(GameObject), true, GUILayout.ExpandWidth(true)) as GameObject;
            charObj = EditorGUILayout.ObjectField("FBX Model", charObj, typeof(GameObject), true, GUILayout.ExpandWidth(true)) as GameObject;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("--- Optional---");

            useGameController = EditorGUILayout.Toggle("Add GameController", useGameController);
            useInventory = EditorGUILayout.Toggle("Add Inventory", useInventory);

            if (useInventory)
            {
                inventory = EditorGUILayout.ObjectField("Inventory Prefab", inventory, typeof(GameObject), true, GUILayout.ExpandWidth(true)) as GameObject;
                itemListData = EditorGUILayout.ObjectField("ItemListData", itemListData, typeof(ItemListData), true, GUILayout.ExpandWidth(true)) as ItemListData;
            }
            if (GUI.changed && charObj != null && charObj.GetComponent<ThirdPersonController>() == null)
            {
                humanoidpreview = Editor.CreateEditor(charObj);
            }

            if (charObj != null && charObj.GetComponent<ThirdPersonController>() != null)
            {
                EditorGUILayout.HelpBox("This gameObject already contains the component vThirdPersonController", MessageType.Warning);
            }

            GUILayout.EndVertical();

            //GUILayout.BeginHorizontal("box");
            //EditorGUILayout.LabelField("Need to know how it works?");
            //if (GUILayout.Button("Video Tutorial"))
            //{
            //    Application.OpenURL("");
            //}
            //GUILayout.EndHorizontal();

            if (charObj)
            {
                charAnimator = charObj.GetComponent<Animator>();
            }

            charExist = charAnimator != null;
            isHuman = charExist ? charAnimator.isHuman : false;
            isValidAvatar = charExist ? charAnimator.avatar.isValid : false;

            if (CanCreate())
            {
                DrawHumanoidPreview();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Create"))
                {
                    Create();
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }

        public virtual bool CanCreate()
        {
            return isValidAvatar && isHuman && charObj != null && charObj.GetComponent<ThirdPersonController>() == null;
        }

        /// <summary>
        /// Draw the Preview window
        /// </summary>
        public virtual void DrawHumanoidPreview()
        {
            GUILayout.FlexibleSpace();

            if (humanoidpreview != null)
            {
                humanoidpreview.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(100, 400), "window");
            }
        }

        private GameObject InstantiateNewCharacter(GameObject selected)
        {
            if (selected == null)
            {
                return selected;
            }

            if (selected.scene.IsValid())
            {
                return selected;
            }

            return PrefabUtility.InstantiatePrefab(selected) as GameObject;
        }

        /// <summary>
        /// Created the Third Person Controller
        /// </summary>
        public virtual void Create()
        {
            // base for the character
            GameObject newCharacter = InstantiateNewCharacter(charObj);

            if (!newCharacter)
            {
                return;
            }

            GameObject _template = Instantiate(template, newCharacter.transform.position, newCharacter.transform.rotation);

            // finds the '3D Model' gameobject or crate one if it doesn't exist
            Transform modelParent = _template.transform.Find("3D Model");

            if (modelParent == null)
            {
                modelParent = new GameObject("3D Model").transform;
                modelParent.parent = _template.transform;
            }

            // finds the 'DatHQ Components' gameobject or crate one if it doesn't exist
            Transform componentsParent = _template.transform.Find("DatHQ Components");

            if (componentsParent == null)
            {
                componentsParent = new GameObject("DatHQ Components").transform;
                componentsParent.parent = _template.transform;
            }

            newCharacter.transform.parent = modelParent;
            newCharacter.transform.localPosition = Vector3.zero;
            newCharacter.transform.localEulerAngles = Vector3.zero;
            _template.name = "dMeleeController_" + charObj.gameObject.name;

            Animator animatorController = newCharacter.GetComponent<Animator>();
            Animator animatorTemplate = _template.GetComponent<Animator>();

            animatorTemplate.avatar = animatorController.avatar;
            animatorTemplate.Rebind();
            DestroyImmediate(animatorController);

            newCharacter.tag = "Player";

            var p_layer = LayerMask.NameToLayer("Player");
            newCharacter.layer = p_layer;

            foreach (Transform t in newCharacter.transform.GetComponentsInChildren<Transform>())
            {
                t.gameObject.layer = p_layer;
            }

            Selection.activeGameObject = _template;

            // search for a MainCamera and attach to the tpCamera
            var mainCamera = Camera.main;
            var tpCamera = _template.GetComponentInChildren<ThirdPersonCamera>();

            if (mainCamera == null)
            {
                mainCamera = new GameObject("MainCamera", typeof(Camera), typeof(AudioListener)).GetComponent<Camera>();
                mainCamera.tag = "MainCamera";
            }

            if (mainCamera.transform.parent != tpCamera.transform)
            {
                mainCamera.transform.parent = tpCamera.transform;
                mainCamera.transform.localPosition = Vector3.zero;
                mainCamera.transform.localEulerAngles = Vector3.zero;
            }

            // add the gameController example
            if (useGameController)
            {
                GameObject gC = null;
                var gameController = FindObjectOfType<GameController>();
                if (gameController == null)
                {
                    gC = new GameObject("vGameController_Example");
                    gC.AddComponent<GameController>();
                }
            }

            if (useInventory)
            {
                // add prefab inventory to the 'DatHQ Components' gameObject inside the Controller
                inventory = Instantiate(inventory, componentsParent.transform.position, componentsParent.transform.rotation);
                inventory.gameObject.transform.parent = componentsParent.transform;
                inventory.transform.localPosition = Vector3.zero;
                inventory.transform.localEulerAngles = Vector3.zero;

                // add shooter melee item list data
                var _itemManager = _template.GetComponent<ItemManager>();
                _itemManager.itemListData = itemListData;
            }
            else
            {
                // remove ItemManager from the character
                var _inventory = _template.GetComponent<ItemManager>();
                DestroyImmediate(_inventory as ItemManager, true);
            }

            // load bones for the BodySnapControl
            var _bodySnap = _template.GetComponentInChildren<BodySnappingControl>();
            _bodySnap.LoadBones();

            SceneView.lastActiveSceneView.FrameSelected();
            Close();
        }
    }
}