using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeSwitch
{
    Dictionary<Type, Action<object>> matches = new Dictionary<Type, Action<object>>();
    public TypeSwitch Case<T>(Action<T> action) { matches.Add(typeof(T), (x) => action((T)x)); return this; }
    public void Switch(object x) { matches[x.GetType()](x); }
}
