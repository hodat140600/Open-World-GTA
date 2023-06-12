using _GAME._Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _GAME._Scripts._CharacterController._Actions
{
    /// <summary>
    /// Generic Action receiver for <seealso cref="vGenericAction"/> events. 
    /// Use this component inside character with <see cref="vGenericAction"/> component.
    /// This is usefull for trigger events based in the <seealso cref="TriggerGenericAction.actionName"/>.
    /// </summary>
    [ClassHeader("Action Receiver")]
    public class GenericActionReceiver : ExtendMonoBehaviour
    {
        public List<string> supportedActionNames = new List<string>() { "Action" };
        public UnityEngine.Events.UnityEvent onEnterTriggerAction;
        public UnityEngine.Events.UnityEvent onExitTriggerAction;
        public UnityEngine.Events.UnityEvent onStartAction;
        public UnityEngine.Events.UnityEvent onCancelAction;
        public UnityEngine.Events.UnityEvent onEndAction;

        private void Start()
        {
            GenericAction genericAction = gameObject.GetComponentInParent<GenericAction>();
            if (genericAction)
            {
                genericAction.OnEnterTriggerAction.AddListener(OnEnterTriggerAction);
                genericAction.OnExitTriggerAction.AddListener(OnExitTriggerAction);
                genericAction.OnStartAction.AddListener(OnStartAction);
                genericAction.OnCancelAction.AddListener(OnCancelAction);
                genericAction.OnEndAction.AddListener(OnEndAction);
            }
        }
        private void OnDestroy()
        {
            GenericAction genericAction = GetComponentInParent<GenericAction>();
            if (genericAction)
            {
                genericAction.OnEnterTriggerAction.RemoveListener(OnEnterTriggerAction);
                genericAction.OnExitTriggerAction.RemoveListener(OnExitTriggerAction);
                genericAction.OnStartAction.RemoveListener(OnStartAction);
                genericAction.OnCancelAction.RemoveListener(OnCancelAction);
                genericAction.OnEndAction.RemoveListener(OnEndAction);
            }
        }

        protected virtual bool IsValidAction(TriggerGenericAction actionInfo)
        {
            bool isValid = enabled && gameObject.activeInHierarchy && actionInfo != null && supportedActionNames.Contains(actionInfo.actionName);
            return isValid;
        }

        /// <summary>
        /// Event called when Enter in trigger
        /// </summary>
        /// <param name="actionInfo"></param>
        public virtual void OnEnterTriggerAction(TriggerGenericAction actionInfo)
        {
            if (IsValidAction(actionInfo))
            {
                onEnterTriggerAction.Invoke();
            }
        }
        /// <summary>
        /// Event Called when exit Trigger
        /// </summary>
        /// <param name="actionInfo"></param>
        public virtual void OnExitTriggerAction(TriggerGenericAction actionInfo)
        {
            if (IsValidAction(actionInfo))
            {
                onExitTriggerAction.Invoke();
            }
        }
        /// <summary>
        /// Event called when action is started
        /// </summary>
        /// <param name="actionInfo"></param>
        public virtual void OnStartAction(TriggerGenericAction actionInfo)
        {
            if (IsValidAction(actionInfo))
            {
                onStartAction.Invoke();
            }
        }
        /// <summary>
        /// Event called when action is canceled
        /// </summary>
        /// <param name="actionInfo"></param>
        public virtual void OnCancelAction(TriggerGenericAction actionInfo)
        {
            if (IsValidAction(actionInfo))
            {
                onCancelAction.Invoke();
            }
        }
        /// <summary>
        /// Event called when action is finished or canceled
        /// </summary>
        /// <param name="actionInfo"></param>
        public virtual void OnEndAction(TriggerGenericAction actionInfo)
        {
            if (IsValidAction(actionInfo))
            {
                onEndAction.Invoke();
            }
        }
    }
}