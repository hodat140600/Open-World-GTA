using UnityEngine;
using System.Collections;

namespace _GAME._Scripts
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class LeoReadOnlyAttribute : PropertyAttribute
    {
        public readonly bool justInPlayMode;

        public LeoReadOnlyAttribute(bool justInPlayMode = true)
        {
            this.justInPlayMode = justInPlayMode;
        }
    }
}
