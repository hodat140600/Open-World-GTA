using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _GAME._Scripts
{
    public class MinMaxAttribute : PropertyAttribute
    {
        public float minLimit = 0;
        public float maxLimit = 1f;
        public MinMaxAttribute()
        {

        }
        public MinMaxAttribute(float min, float max)
        {
            minLimit = min;
            maxLimit = max;
        }
    }
}