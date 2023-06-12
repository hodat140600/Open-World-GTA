using UnityEngine;
namespace _GAME._Scripts
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class EnumFlagAttribute : PropertyAttribute
    {
        public string enumName;

        public EnumFlagAttribute() { }

        public EnumFlagAttribute(string name)
        {
            enumName = name;

        }
    }
}