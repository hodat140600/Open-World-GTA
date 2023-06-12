using System.Collections.Generic;
namespace _GAME._Scripts._CharacterController._Actions
{
    [ClassHeader("Trigger Action Event", helpBoxText = "Use this to filter a specific TriggerAction so you can use Events with the Controller or components attached to the Controller", useHelpBox = true)]
    public class TriggerActionEvent : ExtendMonoBehaviour
    {
        public List<ActionEvent> actionFinders;

        public void TriggerEvent(TriggerGenericAction action)
        {
            var _action = actionFinders.Find(a => a.actionName.Equals(action.gameObject.name));

            if (_action != null)
            {
                _action.onTriggerEvent.Invoke();
            }
        }

        [System.Serializable]
        public class ActionEvent
        {
            public string actionName;
            public UnityEngine.Events.UnityEvent onTriggerEvent;
        }
    }
}