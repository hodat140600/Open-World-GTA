﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

namespace _GAME._Scripts
{
    [System.Serializable]
    public class Point : MonoBehaviour
    {
        public int Id; 
        public float timeToStay = 0f;
        public int maxVisitors = 1;
        public bool isValid = true;
        public List<Transform> visitors;
        public float areaRadius = 1f;

        public void SetIdMappingData(int id)
        {
            Id = id; 
        }

        void Start()
        {
            visitors = new List<Transform>();
        }

        public Vector3 position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public virtual void Enter(Transform visitor)
        {
            if (!visitors.Contains(visitor))
                visitors.Add(visitor);
        }

        public virtual void Exit(Transform visitor)
        {
            if (visitors.Contains(visitor))
                visitors.Remove(visitor);
        }

        public virtual bool IsOnWay(Transform visitor)
        {
            return visitors.Contains(visitor);
        }

        public virtual bool CanEnter(Transform visitor)
        {
            if (visitors.Contains(visitor)) return true;
            if (!isValid) return false;
            if (visitors.Count == maxVisitors) return false;
            return true;
        }
    }
}