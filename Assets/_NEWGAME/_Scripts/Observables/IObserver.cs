using System;
using System.Collections;
using UnityEngine;

public interface IObserver
{
    IDisposable Observe(IObservable observable);
}
