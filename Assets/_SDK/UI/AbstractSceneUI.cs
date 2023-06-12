using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _SDK.UI
{
    public abstract class AbstractSceneUI : MonoBehaviour
    {
        private   GameObject                        _gameObject;
        protected Dictionary<string, AbstractPanel> _panels;

        protected void Init()
        {
            _panels = new Dictionary<string, AbstractPanel>();
        }

        protected void CallAnimationOnPanel(string panelName, string animationName, bool value)
        {
            GetOrAddPanel(panelName).animator.SetBool(animationName, value);
        }

        protected AbstractPanel GetOrAddPanel(string panelName)
        {
            if (!_panels.ContainsKey(panelName))
            {
                var panel = transform.Find(panelName).GetComponent<AbstractPanel>();

                _panels.Add(panelName, panel);
            }

            return _panels[panelName];
        }
    }
}