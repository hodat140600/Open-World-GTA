using System;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    None,
    Idle,
    Walk,
    Chase,
    TargetSpotted,
    Scared,
    Attack
}
public enum NPCType
{
    Police,
    Gangstar,
    NormalPedestrian,
    AgressivePedestrian, 
    Army,
}

[Serializable]
public struct AIData
{
    public NavMeshAgent agent;
    public Transform target;
    public NPCWeaponManager npcWeaponManager;
    public Rigidbody rigid;

    public Animator anim;
    public AIState state;
    public AIState prevState;

    [Header("value")]
    [Range(0, 100)] public float walkSpeed;
    [Range(0, 100)] public float chaseSpeed;
    [Range(0, 100)] public float runSpeed;
    [Range(0, 100)] public float attackSpeed;

    [Range(0, 100)] public float radius;

    [Header("Distance value")]
    [Range(0, 100)] public float minChaseDistance;
    [Range(0, 100)] public float maxChaseDistance;
    [Range(0, 100)] public float minAttackDistance;
    [Range(0, 100)] public float scaredDistance;
    // stop distance
    [Range(0, 20)] public float attackStopDistance;
    [Range(0, 20)] public float walkStopDistance;
    [Range(0, 20)] public float shootingRange;

    [Space(5)]
    

    public bool isScared;
}
