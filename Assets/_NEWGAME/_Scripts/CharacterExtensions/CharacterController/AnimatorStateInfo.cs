using System.Collections.Generic;
using UnityEngine;

namespace _GAME._Scripts._EventSystems
{
    public interface IAnimatorStateInfoController
    {
        AnimatorStateInfos animatorStateInfos { get; }
    }
    public static class IAnimatorStateInfoHelper
    {
        /// <summary>
        /// Register all listener to <see cref="AnimatorTagBase"/> listener
        /// </summary>
        /// <param name="animatorStateInfos"></param>
        public static void Register(this IAnimatorStateInfoController animatorStateInfos)
        {
            if (animatorStateInfos.isValid())
            {
                animatorStateInfos.animatorStateInfos.RegisterListener();
            }
        }
        /// <summary>
        /// Remove all listener from <see cref="AnimatorTagBase"/> 
        /// </summary>
        /// <param name="animatorStateInfos"></param>
        public static void UnRegister(this IAnimatorStateInfoController animatorStateInfos)
        {
            if (animatorStateInfos.isValid())
            {
                animatorStateInfos.animatorStateInfos.RemoveListener();
            }
        }
        /// <summary>
        /// Check if is valid 
        /// </summary>
        /// <param name="animatorStateInfos"></param>
        /// <returns></returns>
        public static bool isValid(this IAnimatorStateInfoController animatorStateInfos)
        {
            return animatorStateInfos != null && animatorStateInfos.animatorStateInfos != null && animatorStateInfos.animatorStateInfos.animator != null;
        }

    }
    [System.Serializable]
    public class AnimatorStateInfos
    {
        public bool debug;
        public Animator animator;
        public AnimatorStateInfos(Animator animator)
        {
            this.animator = animator;

            Init();
        }

        public void Init()
        {
            if (animator)
            {
                stateInfos = new StateInfo[animator.layerCount];
                for (int i = 0; i < stateInfos.Length; i++)
                {
                    stateInfos[i] = new StateInfo(i);
                }
            }
        }

        public void RegisterListener()
        {
            var bhv = animator.GetBehaviours<AnimatorTagBase>();
            for (int i = 0; i < bhv.Length; i++)
            {
                bhv[i].RemoveStateInfoListener(this);
                bhv[i].AddStateInfoListener(this);

            }
            if (debug)
            {
                Debug.Log($"Listeners Registered", animator);
            }
        }

        public void RemoveListener()
        {
            if (animator)
            {
                var bhv = animator.GetBehaviours<AnimatorTagBase>();
                for (int i = 0; i < bhv.Length; i++)
                {
                    bhv[i].RemoveStateInfoListener(this);
                }
                if (debug)
                {
                    Debug.Log($"Listeners Removed", animator);
                }
            }
        }

        public StateInfo[] stateInfos = new StateInfo[0];
        [System.Serializable]
        public class StateInfo
        {
            public int layer;
            public int shortPathHash;
            public float normalizedTime;
            public List<string> tags = new List<string>();
            public StateInfo(int layer)
            {
                this.layer = layer;
            }
        }
        /// <summary>
        /// Add tag to the layer
        /// </summary>
        /// <param name="tag">Tag</param>
        /// <param name="layer">Animator layer</param>
        public void AddStateInfo(string tag, int layer)
        {
            if (stateInfos.Length > 0 && layer < stateInfos.Length)
            {
                StateInfo info = stateInfos[layer];
                info.tags.Add(tag);
                info.shortPathHash = 0;
                info.normalizedTime = 0;
            }
            if (debug)
            {
                Debug.Log($"<color=green>Add tag : <b><i>{tag}</i></b></color>,in the animator layer :{layer}", animator);
            }
        }
        /// <summary>
        /// Uptade State info
        /// </summary>       
        /// <param name="layer">state layer</param>
        /// <param name="normalizedTime">state normalizedTime</param>
        /// <param name="fullPathHash">state fullPathHash</param>
        public void UpdateStateInfo(int layer, float normalizedTime, int fullPathHash)
        {
            if (stateInfos.Length > 0 && layer < stateInfos.Length)
            {
                StateInfo info = stateInfos[layer];
                info.normalizedTime = normalizedTime;
                info.shortPathHash = fullPathHash;
            }
        }
        /// <summary>
        /// Remove Tag of the layer
        /// </summary>
        /// <param name="tag">Tag</param>
        /// <param name="layer">Animator layer</param>
        public void RemoveStateInfo(string tag, int layer)
        {
            if (stateInfos.Length > 0 && layer < stateInfos.Length)
            {
                StateInfo info = stateInfos[layer];
                if (info.tags.Contains(tag))
                {
                    info.tags.Remove(tag);
                    if (info.tags.Count == 0)
                    {
                        info.shortPathHash = 0;
                        info.normalizedTime = 0;
                    }
                    if (debug)
                    {
                        Debug.Log($"<color=red>Remove tag : <b><i>{tag}</i></b></color>, in the animator layer :{layer}", animator);
                    }
                }
            }
        }
        /// <summary>
        /// Check If StateInfo list contains tag
        /// </summary>
        /// <param name="tag">tag to check</param>
        /// <returns></returns>
        public bool HasTag(string tag)
        {
            return System.Array.Exists(stateInfos, info => info.tags.Contains(tag));
        }
        /// <summary>
        /// Check if All tags in in StateInfo List
        /// </summary>
        /// <param name="tags">tags to check</param>
        /// <returns></returns>
        public bool HasAllTags(params string[] tags)
        {
            var has = tags.Length > 0 ? true : false;
            for (int i = 0; i < tags.Length; i++)
            {
                if (!HasTag(tags[i]))
                {
                    has = false;
                    break;
                }
            }
            return has;
        }
        /// <summary>
        /// Check if StateInfo List Contains any tag
        /// </summary>
        /// <param name="tags">tags to check</param>
        /// <returns></returns>
        public bool HasAnyTag(params string[] tags)
        {
            var has = false;
            for (int i = 0; i < tags.Length; i++)
            {
                if (HasTag(tags[i]))
                {
                    has = true;
                    break;
                }
            }
            return has;
        }
        /// <summary>
        /// Get current animator state info using tag
        /// </summary>
        /// <param name="tag">tag</param>
        /// <returns>if tag exit return AnimatorStateInfo? else return null</returns>
        public StateInfo GetStateInfoUsingTag(string tag)
        {
            return System.Array.Find(stateInfos, info => info.tags.Contains(tag));
        }

        public float GetCurrentNormalizedTime(int layer)
        {
            if (stateInfos.Length > 0 && layer < stateInfos.Length)
            {
                StateInfo info = stateInfos[layer];
                return info.normalizedTime;
            }
            return 0;
        }

    }
}
