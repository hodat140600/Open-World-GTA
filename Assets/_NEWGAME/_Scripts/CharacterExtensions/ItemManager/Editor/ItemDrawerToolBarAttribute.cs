using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace _GAME._Scripts._ItemManager
{
    /// <summary>
    /// This attribute is used to draw aditional custom editor like a ToolBar for <see cref="vItem"/> in  partial classes of the <see cref="ItemDrawer"/> using <see cref="ItemDrawer.OnDrawItem"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ItemDrawerToolBarAttribute : Attribute
    {
        public string title;
        public ItemDrawerToolBarAttribute(string title)
        {
            this.title = title;
        }
    }
}