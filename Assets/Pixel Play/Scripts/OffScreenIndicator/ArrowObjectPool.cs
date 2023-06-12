using System.Collections.Generic;
using UnityEngine;

class ArrowObjectPool : MonoBehaviour
{
    public static ArrowObjectPool current;

    [Tooltip("Assign the arrow prefab.")]
    public Indicator pooledObject;
    [Tooltip("Initial pooled amount.")]
    public int pooledAmount = 1;
    [Tooltip("Should the pooled amount increase.")]
    public bool willGrow = true;

    List<Indicator> _pooledObjects;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        _pooledObjects = new List<Indicator>();

        for (int i = 0; i < pooledAmount; i++)
        {
            Indicator arrow = Instantiate(pooledObject, transform, false);
            arrow.Activate(false);
            _pooledObjects.Add(arrow);
        }
    }

    /// <summary>
    /// Gets pooled objects from the pool.
    /// </summary>
    /// <returns></returns>
    public Indicator GetPooledObject()
    {
        foreach (Indicator t in _pooledObjects)
        {
            if (!t.Active)
            {
                return t;
            }
        }

        if (!willGrow) return null;
        
        Indicator arrow = Instantiate(pooledObject, transform, false);
        arrow.Activate(false);
        _pooledObjects.Add(arrow);
        return arrow;
    }

    /// <summary>
    /// Deactive all the objects in the pool.
    /// </summary>
    public void DeactivateAllPooledObjects()
    {
        foreach (Indicator arrow in _pooledObjects)
        {
            arrow.Activate(false);
        }
    }
}
