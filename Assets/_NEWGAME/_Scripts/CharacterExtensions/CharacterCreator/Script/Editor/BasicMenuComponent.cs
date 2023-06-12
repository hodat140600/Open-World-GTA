using UnityEngine;
using UnityEditor;

namespace _GAME._Scripts._CharacterController._Actions
{
    // BASIC FEATURES
    public partial class MenuComponent
    {
        [MenuItem("GameObject/DatHQ/Utils/Create SimpleTrigger", false)]
        static void AddSimpleTrigger()
        {
            var obj = new GameObject("SimpleTrigger", typeof(SimpleTrigger));


            SceneView view = SceneView.lastActiveSceneView;
            if (SceneView.lastActiveSceneView == null)
                throw new UnityException("The Scene View can't be access");

            Vector3 spawnPos = view.camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 5f));
            if (Selection.activeGameObject)
            {
                obj.transform.parent = Selection.activeGameObject.transform;
                spawnPos = Selection.activeGameObject.transform.position;
            }
            obj.transform.position = spawnPos;
            obj.layer = LayerMask.NameToLayer("Triggers");

            Selection.activeGameObject = obj.gameObject;
        }

        [MenuItem("GameObject/DatHQ/Utils/Create SimpleTrigger With Input", false)]
        static void AddSimpleTriggerWithInput()
        {
            var obj = new GameObject("SimpleTrigger WithInput", typeof(SimpleTriggerWithInput));


            SceneView view = SceneView.lastActiveSceneView;
            if (SceneView.lastActiveSceneView == null)
                throw new UnityException("The Scene View can't be access");

            Vector3 spawnPos = view.camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 5f));
            if (Selection.activeGameObject)
            {
                obj.transform.parent = Selection.activeGameObject.transform;
                spawnPos = Selection.activeGameObject.transform.position;
            }
            obj.transform.position = spawnPos;
            obj.layer = LayerMask.NameToLayer("Triggers");

            Selection.activeGameObject = obj.gameObject;
        }

        [MenuItem("DatHQ/Basic Locomotion/Actions/Generic Action")]
        static void GenericActionMenu()
        {
            if (Selection.activeGameObject)
                Selection.activeGameObject.AddComponent<GenericAction>();
            else
                Debug.Log("Please select the Player to add this component.");
        }

        [MenuItem("DatHQ/Basic Locomotion/Components/Generic Animation")]
        static void GenericAnimationMenu()
        {
            if (Selection.activeGameObject)
                Selection.activeGameObject.AddComponent<GenericAnimation>();
            else
                Debug.Log("Please select the Player to add this component.");
        }

        [MenuItem("DatHQ/Basic Locomotion/Actions/Ladder Action")]
        static void LadderActionMenu()
        {
            if (Selection.activeGameObject)
                Selection.activeGameObject.AddComponent<LadderAction>();
            else
                Debug.Log("Please select the Player to add this component.");
        }

        [MenuItem("DatHQ/Basic Locomotion/Components/HitDamageParticle")]
        static void HitDamageMenu()
        {
            if (Selection.activeGameObject)
                Selection.activeGameObject.AddComponent<HitDamageParticle>();
            else
                Debug.Log("Please select a vCharacter to add the component.");
        }

        [MenuItem("DatHQ/Basic Locomotion/Components/HeadTrack")]
        static void HeadTrackMenu()
        {
            if (Selection.activeGameObject)
                Selection.activeGameObject.AddComponent<HeadTrack>();
            else
                Debug.Log("Please select a vCharacter to add the component.");
        }

        [MenuItem("DatHQ/Basic Locomotion/Components/FootStep")]
        static void FootStepMenu()
        {
            if (Selection.activeGameObject)
                Selection.activeGameObject.AddComponent<FootStep>();
            else
                Debug.Log("Please select a GameObject to add the component.");
        }

        [MenuItem("DatHQ/Basic Locomotion/Resources/New AudioSurface")]
        static void NewAudioSurface()
        {
            ScriptableObjectUtility.CreateAsset<AudioSurface>();
        }

        [MenuItem("DatHQ/Basic Locomotion/Resources/New Ragdoll Generic Template")]
        static void RagdollGenericTemplate()
        {
            ScriptableObjectUtility.CreateAsset<RagdollGenericTemplate>();
        }
    }
}