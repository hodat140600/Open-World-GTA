using System.Collections.Generic;
using UnityEngine;

namespace _GAME._Scripts
{
    [ClassHeader("MESSAGE RECEIVER", "Use this component with the MessageSender to call Events.")]
    public class MessageReceiver : ExtendMonoBehaviour
    {
        public static event OnReceiveMessage onReceiveGlobalMessage;
        public List<MessageListener> messagesListeners;
        [System.Serializable]
        public delegate void OnReceiveMessage(string name, string message = null);
        [System.Serializable]
        public class OnReceiveMessageEvent : UnityEngine.Events.UnityEvent<string> { }

        public event OnReceiveMessage onReceiveMessage;
        private void Start()
        {
            for (int i = 0; i < messagesListeners.Count; i++)
            {
                MessageListener messageListener = messagesListeners[i];
                if (messageListener.receiveFromGlobal)
                {
                    onReceiveGlobalMessage -= messageListener.OnReceiveMessage;
                    onReceiveGlobalMessage += messageListener.OnReceiveMessage;
                }
                else
                {
                    onReceiveMessage -= messageListener.OnReceiveMessage;
                    onReceiveMessage += messageListener.OnReceiveMessage;
                }
            }
        }
        [System.Serializable]
        public class MessageListener
        {
            public string Name;
            public bool receiveFromGlobal;
            public OnReceiveMessageEvent onReceiveMessage;

            public void OnReceiveMessage(string name, string message = null)
            {
                if (Name.Equals(name)) onReceiveMessage.Invoke(string.IsNullOrEmpty(message) ? string.Empty : message);

            }
            public MessageListener(string name)
            {
                Name = name;
            }
            public MessageListener(string name, UnityEngine.Events.UnityAction<string> listener)
            {
                Name = name;
                onReceiveMessage.AddListener(listener);
            }
        }

        /// <summary>
        /// Add Action Listener
        /// </summary>
        /// <param name="name">Message Name</param>
        /// <param name="listener">Action Listener</param>
        public void AddListener(string name, UnityEngine.Events.UnityAction<string> listener)
        {

            if (messagesListeners.Exists(l => l.Name.Equals(name)))
            {
                var messageListener = messagesListeners.Find(l => l.Name.Equals(name));
                messageListener.onReceiveMessage.AddListener(listener);
            }
            else
            {
                messagesListeners.Add(new MessageListener(name, listener));
            }
        }

        /// <summary>
        /// Remove Action Listener
        /// </summary>
        /// <param name="name">Message Name</param>
        /// <param name="listener">Action Listener</param>
        public void RemoveListener(string name, UnityEngine.Events.UnityAction<string> listener)
        {
            if (messagesListeners.Exists(l => l.Name.Equals(name)))
            {
                var messageListener = messagesListeners.Find(l => l.Name.Equals(name));
                messageListener.onReceiveMessage.RemoveListener(listener);
            }
        }

        /// <summary>
        /// Call Action with message
        /// </summary>
        /// <param name="name">message name</param>
        /// <param name="message">message value</param>
        public void Send(string name, string message)
        {
            if (enabled == false) return;
            onReceiveMessage?.Invoke(name, message);
        }

        /// <summary>
        /// Call Action without message
        /// </summary>
        /// <param name="name">message name</param>
        public void Send(string name)
        {
            if (enabled == false) return;
            onReceiveMessage?.Invoke(name, string.Empty);
        }

        public static void SendGlobal(string name, string message = null)
        {
            onReceiveGlobalMessage?.Invoke(name, message);
        }
    }
}