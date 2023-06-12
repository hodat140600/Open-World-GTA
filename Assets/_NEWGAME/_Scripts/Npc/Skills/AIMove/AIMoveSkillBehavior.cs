using System.Collections;
using _GAME._Scripts;
using _GAME._Scripts.Npc;
using _GAME._Scripts.Npc.SkillSystem;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

// todo remove, rename to AINgu
public class AIMoveSkillBehavior : MonoBehaviour, IObserver
{
    private NavMeshAgent navMeshAgent;
    private Animator     animator;

    private AIMoveSkillLevel aiMoveSkillLevel;
    public float moveSpeed { get; private set; }

    private bool _isChasingTarget = false;
    private CompositeDisposable _disposables;

    // private float radius      = 100;  // radius arround player to find next destination
    // private float distanceRun = 100f; // run away player
    public  bool  isLeader    = false;

    private NpcSkillSystem      npcSkillSystem;
    private FormationController formationController;

    // TODO REMOVE
    private ShootSkillBehavior shootSkillBehavior;
    // private PunchSkillBehavior punchSkillBehavior;

    private RouteNavigator routeNavigator;

    private static readonly int WalkHash           = Animator.StringToHash("walk");
    private static readonly int DieHash            = Animator.StringToHash("die");
    private static readonly int HitHash            = Animator.StringToHash("hit");
    private static readonly int WalkMultiplierHash = Animator.StringToHash("WalkMultiplier");
    private static readonly int IdleHash           = Animator.StringToHash("idle");

    private void Awake()
    {
        _disposables = new CompositeDisposable();
    }

    private void OnEnable()
    {
        formationController = Gameplay.Instance.FormationController;

        npcSkillSystem = GetComponent<NpcSkillSystem>();
        INpc npc = npcSkillSystem.npc;
        // routeNavigator = new RouteNavigator(npc.Route, npc.StartingRouteIndex, Gameplay.Instance.Map);

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator     = GetComponent<Animator>();

        InitAnimator();
        UpdateWalkAnimationSpeed();
    }

    private void Start()
    {
        npcSkillSystem     = GetComponent<NpcSkillSystem>();
        shootSkillBehavior = GetComponent<ShootSkillBehavior>();
        // punchSkillBehavior = GetComponent<PunchSkillBehavior>();
    }

    private void InitAnimator()
    {
        animator.SetBool(WalkHash, true);
        animator.SetBool(DieHash, false);
        animator.SetBool(HitHash, false);
        animator.Play(moveSpeed == 0 ? "Idle" : "Walk_Fwd", 0, Random.Range(0f, 1f));
    }

    private void UpdateWalkAnimationSpeed()
    {
        animator.SetFloat(WalkMultiplierHash, Random.Range(.9f, 1.1f));
    }

    public void SetFlagChasing(bool isChasingTarget)
    {
        _isChasingTarget = isChasingTarget;
    }
 

    private void Update()
    {
        //if (!isLeader)
        //    return;

        //bool reachedCurrentCheckpoint = navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;

        //if (reachedCurrentCheckpoint && !_isChasingTarget)
        //    MoveToNextCheckpoint();
    }

    public void LevelUp(AIMoveSkillLevel skillLevel)
    {
        aiMoveSkillLevel              = skillLevel;
        moveSpeed                     = skillLevel.Speed;
        navMeshAgent.speed            = moveSpeed;
        navMeshAgent.stoppingDistance = isLeader ? 10f : 5f;
    }

    private void MoveToNextCheckpoint()
    {
        routeNavigator.ToNextWaypoint();
        Vector3 nextDestination = routeNavigator.CurrentWaypoint.Position;
        //navMeshAgent.SetDestination(nextDestination);
        formationController.ApplyFormation(npcSkillSystem.npc.Id, nextDestination, nextDestination + new Vector3(100, 0, 100), moveSpeed);
    }

    public void SetDestinationAndSpeed(Vector3 pos, float speed)
    {
        if(navMeshAgent.isActiveAndEnabled)navMeshAgent?.SetDestination(pos);
        moveSpeed          = speed * Random.Range(.9f, 1.1f);
        navMeshAgent.speed = moveSpeed;
    }


    public System.IDisposable Observe(IObservable observable)
    {
        if (observable == null) return null;

        return ((NPCObservable)observable).AIStateStream.Subscribe((_) =>
        {
            SetFlagChasing(_);
        }).AddTo(_disposables);
    }
}