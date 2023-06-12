using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserverObject 
{
    public void OnNotify();
    public void OnNotify(IDataObserver data);
}
