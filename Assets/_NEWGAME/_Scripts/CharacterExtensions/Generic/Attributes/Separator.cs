using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _GAME._Scripts
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class Separator : PropertyAttribute
    {
        public string label;
        public string tooltip;
        public string style;
        public int fontSize = 10;

        public Separator()
        {
            fontSize = 15;
        }

        public Separator(string label, string tooltip = "")
        {
            this.label = label;
            this.tooltip = tooltip;
            fontSize = 15;
        }

        public Separator(string label, int fontSize, string tooltip = "")
        {
            this.label = label;
            this.tooltip = tooltip;
            this.fontSize = fontSize;
        }
    }
}