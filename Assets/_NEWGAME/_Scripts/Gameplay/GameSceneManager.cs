using _GAME._Scripts.Game;
using Assets._SDK.StateMachine;
using Stateless;
using UniRx;
using UnityEngine;

public enum GameSceneState
{
    Init,
    Playing,
    Chasing,
    Death,
}

public enum GameSceneTrigger
{
    InitToPlaying,
    Die,
    Revive,
}


public class GameSceneManager : SceneSingleton<GameSceneManager>
    , IHasStateMachine<GameSceneState, GameSceneTrigger>
{
    [SerializeField] GameSceneState                                 startState;
    public           StateMachine<GameSceneState, GameSceneTrigger> GameStateMachine;

    public ReactiveProperty<GameSceneState> PreviousState { get; private set; }

    public ReactiveProperty<GameSceneState> CurrentState { get; private set; }

    // public CharacterClass    playerScript;
    // public SpawnerController spawnController;


    public void Fire(GameSceneTrigger trigger) => GameStateMachine.Fire(trigger);
    public bool CanFire(GameSceneTrigger trigger) => GameStateMachine.CanFire(trigger);
    public bool IsInState(GameSceneState state) => GameStateMachine.IsInState(state);

    protected override void OnAwake()
    {
        base.OnAwake();

        CurrentState  = new ReactiveProperty<GameSceneState>(GameSceneState.Init);
        PreviousState = new ReactiveProperty<GameSceneState>(GameSceneState.Init);

        GameStateMachine = new StateMachine<GameSceneState, GameSceneTrigger>(CurrentState.Value);

        GameStateMachine.OnTransitioned((transition) =>
        {
            CurrentState.Value  = transition.Destination;
            PreviousState.Value = transition.Source;
        });

        // set frame rate
        Application.targetFrameRate = 60;
        // change money
        GameManager.Instance.Wallet?.DefaultAccount.Deposit(100000);

        //config state
        GameStateMachine.Configure(GameSceneState.Init)
            .OnActivate(() => InitGameState())
            .Permit(GameSceneTrigger.InitToPlaying, GameSceneState.Playing);

        GameStateMachine.Configure(GameSceneState.Playing)
            .Permit(GameSceneTrigger.Die, GameSceneState.Death);

        GameStateMachine.Configure(GameSceneState.Death)
            .Permit(GameSceneTrigger.Revive, GameSceneState.Init);
    }

    private void Start()
    {
        GameStateMachine.Activate();
    }

    private void InitGameState()
    {
        
    }
}