using _GAME._Scripts;
using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class AIMeleeSkillLevel : AbstractSkillLevel
{
    [TabGroup("TabGroup", "Health")]
    [SerializeField, HorizontalGroup("TabGroup/Health/Point"), SuffixLabel("int",true)]
    public int maxHealth;

    [TabGroup("TabGroup", "Locomtion")]
    [SerializeField, Range(0, 2f), HorizontalGroup("TabGroup/Locomtion/ChaseSpeed"), SuffixLabel("float", true)]
    public float chaseSpeed;

    [TabGroup("TabGroup", "Iteration")]
    [SerializeField, Range(0, 1.5f), HorizontalGroup("TabGroup/Iteration/StateRoutine"), SuffixLabel("float", true)]
    public float stateRoutine;

    [TabGroup("TabGroup", "Iteration")]
    [SerializeField, Range(0, 10f), HorizontalGroup("TabGroup/Iteration/DestinationRoutine"), SuffixLabel("float", true)]
    public float destinationRoutine;

    [TabGroup("TabGroup", "Iteration")]
    [SerializeField, Range(0, 1.5f), HorizontalGroup("TabGroup/Iteration/FindTarget"), SuffixLabel("float", true)]
    public float findTarget;

    [TabGroup("TabGroup", "Detection")]
    [SerializeField, HorizontalGroup("TabGroup/Detection/LayerMask")]
    public LayerMask detectLayer = 1 << 8;

    [SerializeField, HorizontalGroup("TabGroup/Detection/AIState")]
    public _GAME._Scripts._CharacterController._AI.SimpleMeleeAI_Motor.AIStates AIState;

    [SerializeField, Range(0,360), HorizontalGroup("TabGroup/Detection/ViewDectect"), SuffixLabel("int", true)]
    public int fieldOfViewToDetect;

    [Tooltip("Distance to noticed the Target without FOV")]
    [SerializeField, HorizontalGroup("TabGroup/Detection/DistanceDetect"), SuffixLabel("int", true)]
    public int detectDistance;

    [Tooltip("Distance to lost the Target")]
    [SerializeField, HorizontalGroup("TabGroup/Detection/DistanceLost"), SuffixLabel("int", true)]
    public int distanceToLostTarget;

    [TabGroup("TabGroup", "Combat")]

    [Tooltip("Check if you want the Enemy to chase the Target at first sight")]
    [Button("Agressive At First Sight", ButtonSizes.Medium)]
    [ShowIf("@this.agressiveAtFirestSight == false", false)]
    [HorizontalGroup("TabGroup/Combat/Behaviour", .5f, Order = -1)]
    private void AgressiveAtFirestSightOn()
    {
        agressiveAtFirestSight = true;
    }

    [Tooltip("Check if you want the Enemy to chase the Target at first sight")]
    [Button("Agressive At First Sight", ButtonSizes.Medium)]
    [ShowIf("@this.agressiveAtFirestSight == true", false)]
    [HorizontalGroup("TabGroup/Combat/Behaviour", .5f, Order = -1)]
    [GUIColor(0.27f, 1f, 0f, 1f)]
    private void AgressiveAtFirestSightOff()
    {
        agressiveAtFirestSight = false;
    }
    [HideInInspector]
    public bool agressiveAtFirestSight;

    [Tooltip("Check if you want the Enemy to be passive even if you attack him")]
    [Button("Passive When Take Damage", ButtonSizes.Medium)]
    [ShowIf("@this.passiveToDamage == false", false)]
    [HorizontalGroup("TabGroup/Combat/Behaviour", .5f, Order = -1)]
    private void PassiveToDamageOn()
    {
        passiveToDamage = true;
    }

    [Tooltip("Check if you want the Enemy to be passive even if you attack him")]
    [Button("Passive When Take Damage", ButtonSizes.Medium)]
    [ShowIf("@this.passiveToDamage == true", false)]
    [HorizontalGroup("TabGroup/Combat/Behaviour", .5f, Order = -1)]
    [GUIColor(0.27f, 1f, 0f, 1f)]
    private void PassiveToDamageOff()
    {
        passiveToDamage = false;
    }

    [HideInInspector]
    public bool passiveToDamage;

    [SerializeField, HorizontalGroup("TabGroup/Combat/TimeAttack"), SuffixLabel("float", true)]
    public float timeBetweenAttack;


    [SerializeField, HorizontalGroup("TabGroup/Combat/Damage"), SuffixLabel("int", true)]
    public int damage;

    [SerializeField/*, HorizontalGroup("TabGroup/Combat/Level")*/]
    private LevelType levelType;

    public override int Index => (int)levelType;
}
