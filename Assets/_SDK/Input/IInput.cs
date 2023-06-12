using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInput
{
    IObservable<Vector3> MoveStream { get; }
    IObservable<bool> FireStream { get; }
    IObservable<int> PunchStream { get; }
}
