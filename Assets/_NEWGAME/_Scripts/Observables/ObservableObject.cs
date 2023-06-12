using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservableObject : MonoBehaviour
{
    private List<IObserverObject> _observerObjects = new List<IObserverObject>();

    public void AddObserver(IObserverObject observerObject)
    {
        _observerObjects.Add(observerObject);
    }

    public void RemoveObserver(IObserverObject observerObject)
    {
        _observerObjects.Remove(observerObject);
    }

    protected void NotifyObservers()
    {
        _observerObjects.ForEach((_observerObject) =>
        {
            _observerObject.OnNotify();
        });
    }
    protected void NotifyObservers(IDataObserver data)
    {
        _observerObjects.ForEach((_observerObject) =>
        {
            _observerObject.OnNotify(data);
        });
    }
}
