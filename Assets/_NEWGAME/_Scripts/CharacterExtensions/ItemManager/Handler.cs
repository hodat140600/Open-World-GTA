using UnityEngine;
using System.Collections.Generic;

namespace _GAME._Scripts
{
    [System.Serializable]
    public class Handler
    {
        public Transform defaultHandler;
        public List<Transform> customHandlers;
        public Handler()
        {
            customHandlers = new List<Transform>();
        }
    }
}