using System.Collections;
using System.Linq;
using _GAME._Scripts;
using _GAME._Scripts._CharacterController._AI;
using _GAME._Scripts.Inventory;
using _GAME._Scripts.Npc;
using _GAME._Scripts.Npc.Enemy;
using _GAME._Scripts.Npc.Pedestrian;
using _GAME._Scripts.Npc.SkillSystem;
using _NEWGAME._Scripts.Game;
using Assets._SDK.Skills;
using DG.Tweening;
using Extensions;
using MyBox;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

[ClassHeader("AI Melee Skill")]
public class AIMeleeSkillBehaviour : SimpleMeleeAI_Controller
{
    private const string MATERIAL_VARIABLE_DEATH = "_Destroyer_Value_1";
    private Material _materialNPC;
    private Animator _netBoxAnimator;
    private bool _hasBeenHeadShot = false;
    private ObjectPoolController _objectPoolController;
    private WeaponType _killedWeapon;
    private bool notifiedKilledEvent = false;
    private RouteNavigator _routeNavigator;
    private NpcSkillSystem _skillSystem;
    private AudioSource _audioSource;
    float _tempSpeed;

    public void LevelUp(AIMeleeSkillLevel aIMeleeSkillLevel)
    {
        //StartCoroutine(UpdateAIData(aIMeleeSkillLevel));
        UpdateAIData(aIMeleeSkillLevel);
    }

    public override void ChangeMaxHealth(int value)
    {
        maxHealth = value;
        if (maxHealth < 0)
            maxHealth = 0;
    }

    private void UpdateAIData(AIMeleeSkillLevel aIMeleeSkillLevel)
    {
        Init();
        base.smoothSpeed = 2f;
        ChangeMaxHealth(aIMeleeSkillLevel.maxHealth);
        base.stateRoutineIteration = aIMeleeSkillLevel.stateRoutine;
        base.destinationRoutineIteration = aIMeleeSkillLevel.destinationRoutine;
        // base.findTargetIteration = aIMeleeSkillLevel.findTarget;
        var typeSwitch = new TypeSwitch()
            .Case((Pedestrian pedestrian) =>
            {
                base.pathArea = Gameplay.Instance.waypointArea;
                base.patrolWithoutAreaStyle = AIPatrolWithOutAreaStyle.GoToStartPoint;
                base.currentState = AIStates.PatrolWaypoints;
            })
            .Case((Enemy enemy) =>
            {
                //base.patrolWithoutAreaStyle = AIPatrolWithOutAreaStyle.Idle;
                base.oldState = AIStates.Idle;
                base.currentState = AIStates.Idle;
                _netBoxAnimator?.SetBool(idDeadAnimator, isDead);
            });
        typeSwitch.Switch(_skillSystem.npc);
        base.chaseSpeed = aIMeleeSkillLevel.chaseSpeed;
        base.layersToDetect = aIMeleeSkillLevel.detectLayer;
        base.fieldOfView = aIMeleeSkillLevel.fieldOfViewToDetect;
        base.minDetectDistance = aIMeleeSkillLevel.detectDistance / 2;
        base.maxDetectDistance = aIMeleeSkillLevel.detectDistance;
        base.distanceToLostTarget = aIMeleeSkillLevel.distanceToLostTarget;
        base.minTimeToAttack = aIMeleeSkillLevel.timeBetweenAttack;
        base.maxTimeToAttack = aIMeleeSkillLevel.timeBetweenAttack;
        base.randomAttackCount = false;
        base.passiveToDamage = aIMeleeSkillLevel.passiveToDamage;
        base.agressiveAtFirstSight = aIMeleeSkillLevel.agressiveAtFirestSight;
        base.chanceToRoll = 0;
        base.chanceToBlockAttack = 0;
        base.chanceToBlockInStrafe = 0;
        base.raiseShield = 0;
        base.lowerShield = 0;
        meleeManager.defaultDamage.damageValue = aIMeleeSkillLevel.damage;
        ChangeHealth(MaxHealth);
        _tempSpeed = patrolSpeed;
    }

    protected override void Start()
    {
        base.Start();
        //onDead.AddListener(ActionOnDied);
    }

    private void OnEnable()
    {
        Start();
    }

    int idChasingAnimator = Animator.StringToHash("isChasing");
    int idDeadAnimator = Animator.StringToHash("isDead");

    public override void Init()
    {
        base.Init();
        TryGetComponent(out _skillSystem);
        onStartReceiveDamage.AddListener(CheckDeath);
        onDead.AddListener(ActionOnDied);
        _netBoxAnimator = transform.GetComponentInFirstLayerChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();

        _materialNPC = transform.GetComponentInFirstLayerChildren<SpriteRenderer>()?.material;
        _materialNPC?.SetFloat(MATERIAL_VARIABLE_DEATH, 0f);
        onChase = new UnityEvent();
        onIdle = new UnityEvent();
        onPatrol = new UnityEvent();
        if (_netBoxAnimator != null)
        {
            onChase.AddListener(OnChasingTarget);
            onIdle.AddListener(OnIdle);
        }

        isInit = true;
    }

    private void OnChasingTarget()
    {
        _netBoxAnimator?.SetBool(idChasingAnimator, true);
    }

    private void OnIdle()
    {
        _netBoxAnimator?.SetBool(idChasingAnimator, false);
    }

    public void CheckDeath(Damage damage)
    {
        if (damage.receiver.CompareTag("Head"))
        {
            _hasBeenHeadShot = true;
            damage.receiver.GetComponent<Animator>().SetTrigger(EyeshotHash);
        }

        _killedWeapon = damage.damageType switch
        {
            "Handgun" => WeaponType.Handgun,
            "AssaultRifle" => WeaponType.AssaultRifle,
            "SMG" => WeaponType.SMG,
            "Shotgun" => WeaponType.Shotgun,
            "RPG" => WeaponType.RPG,
            _ => throw new System.NotImplementedException("??")
        };
    }

    public void ActionOnNextbotSpawn()
    {
        RunningAway();
    }

    void RunningAway()
    {
        patrolSpeed = 10f;
    }

    public void ActionOnStartDay()
    {
        ResetSpeed();
    }

    void ResetSpeed()
    {
        Debug.Log("Het chay dau");
        patrolSpeed = _tempSpeed;
    }

    void ActionOnDied(GameObject target)
    {
        //currentTarget.character = null;
        //currentTarget.colliderTarget = null;

        if (_audioSource != null)
        {
            _audioSource.clip = SfxManager.Instance.GetNextBotDeadAudioClip();
            _audioSource.Play();
        }

        DOVirtual.Float(0f, 1f, waitTime, v => _materialNPC?.SetFloat(MATERIAL_VARIABLE_DEATH, v)).SetUpdate(true)
            .OnComplete(() => _materialNPC?.SetFloat(MATERIAL_VARIABLE_DEATH, 1f));
        currentTarget.transform = null;
        if (_skillSystem)
        {
            StartCoroutine(Disable(target, _skillSystem.npc.Id.ToString()));
            if (!notifiedKilledEvent)
            {
                notifiedKilledEvent = true;
                MessageBroker.Default.Publish(new NpcKilledEvent(_skillSystem.npc.ModelType, _hasBeenHeadShot,
                    _killedWeapon));
            }
        }
    }

    public void InitPooling(ObjectPoolController objectPoolController)
    {
        _objectPoolController = objectPoolController;
    }


    public void InitNavigator(RouteNavigator routeNavigator)
    {
        _routeNavigator = routeNavigator;
    }

    protected override int GetCurrentWaypoint()
    {
        _routeNavigator.ToNextWaypoint();
        var index = pathArea.GetWaypointIndex(_routeNavigator.CurrentWaypoint.Id);
        return index;
    }

    private const float waitTime = 3f;
    private static WaitForSeconds WaitForHalfSeconds = new(waitTime);
    private static readonly int EyeshotHash = Animator.StringToHash("eyeshot");

    IEnumerator Disable(GameObject thisObject, string id)
    {
        yield return WaitForHalfSeconds;
        var typeSwitch = new TypeSwitch()
            .Case((Pedestrian pedestrian) => _objectPoolController.ReturnNPCToPool(thisObject, id))
            .Case((Enemy enemy) => _objectPoolController.ReturnEnemyToPool(thisObject, id));
        typeSwitch.Switch(_skillSystem.npc);
        notifiedKilledEvent = false;
    }
}