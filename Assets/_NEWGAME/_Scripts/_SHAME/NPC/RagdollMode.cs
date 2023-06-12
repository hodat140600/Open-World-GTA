using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollMode : MonoBehaviour
{
    [SerializeField]
    private Collider[] colliders;
    [SerializeField]
    private Rigidbody[] rigidBodies;

    [SerializeField]
    private Collider collider;
    [SerializeField]
    private Rigidbody rigidBody;

    private void Awake()
    {
        LoadALlInfo();
        TurnOffRagDoll();
    }
    private void Update()
    {
        
    }
    private void LoadALlInfo()
    {
        colliders = gameObject.GetComponentsInChildren<Collider>();
        rigidBodies = gameObject.GetComponentsInChildren<Rigidbody>();
        collider = gameObject.GetComponent<Collider>();
        rigidBody= gameObject.GetComponent<Rigidbody>();
    }

    public void TurnOnRagDoll()
    {
        foreach(Collider col in colliders)
        {
            if (col == collider) continue;
            col.enabled = true;
        }
        foreach(Rigidbody rid in rigidBodies)
        {
            if (rid == rigidBody) continue;
            rid.isKinematic = false;
        }
        collider.enabled = false;
        rigidBody.isKinematic = false;
    }
    public void TurnOffRagDoll()
    {
        foreach (Collider col in colliders)
        {
            if (col == collider) continue;
            col.enabled = false;
        }
        foreach (Rigidbody rid in rigidBodies)
        {
            if (rid == rigidBody) continue;
            rid.isKinematic = true;
        }
        collider.enabled = true;
        rigidBody.isKinematic = true;
    }
}
