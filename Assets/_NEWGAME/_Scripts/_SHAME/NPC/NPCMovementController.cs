using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCMovementController : MonoBehaviour
{
    [Header("Destination var")]
    protected Vector3 destination;
    public bool reachDestination=false;


    protected virtual void Update()
    {
        destination.y = transform.position.y;
    }
    public void SetDestination(Vector3 xdestination)
    {
        this.destination = xdestination;
        reachDestination= false;
    }
}
