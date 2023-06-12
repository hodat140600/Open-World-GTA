using System.Collections;
using _GAME._Scripts._ItemManager;
using _NEWGAME._Scripts.Game;
using _SDK.SplashScreen;
using Assets._SDK.Game;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _GAME._Scripts.Game
{
    public enum GameState
    {
        Init,
        Lobby,

        // LobbyHome,
        LobbySettings,
        LobbyHome,
        Playing,
    }

    public enum GameTrigger
    {
        InitToLobby,
        BackToLobby,
        ShowShop,
        ShowSettings,

        // LoadMap,
        Play,
    }

    public class GameManager : AbstractGameManager
    {
        public new static GameManager Instance => (GameManager)AbstractGameManager.Instance;
        public GameResources Resources;
        
        public ReactiveProperty<GameState> CurrentState { get; private set; }
        public ReactiveProperty<GameState> PreviousState { get; private set; }

        public void Fire(GameTrigger trigger) => StateMachine.Fire(trigger);

        public bool CanFire(GameTrigger trigger) => StateMachine.CanFire(trigger);

        public bool TryFire(GameTrigger trigger)
        {
            if (StateMachine.CanFire(trigger))
            {
                StateMachine.Fire(trigger);
                return true;
            }

            return false;
        }


        private ItemManager _itemManager;
        public ItemManager ItemManager
        {
            get
            {
                if (!_itemManager)
                {
                    var gos = GameObject.FindGameObjectsWithTag("Player");
                    foreach (var go in gos)
                    {
                        if (go.TryGetComponent(out ItemManager manager))
                            _itemManager = manager;
                    }
                }

                return _itemManager;
            }
            set { _itemManager = value; }
        }

        private SkinManager _skinManager;
        public SkinManager SkinManager
        {
            get
            {
                if (!_skinManager)
                {
                    var gos = GameObject.FindGameObjectsWithTag("Player");
                    foreach (var go in gos)
                    {
                        if (go.TryGetComponent(out SkinManager manager))
                            _skinManager = manager;
                    }
                }
                return _skinManager;
            }
            set
            {
                _skinManager = value;
            }
        }

        protected override void OnAwake()
        {
            CurrentState  = new ReactiveProperty<GameState>(Instance.StateMachine.State);
            PreviousState = new ReactiveProperty<GameState>();

            Instance.StateMachine.OnTransitioned(t =>
            {
                PreviousState.Value = t.Source;
                CurrentState.Value  = t.Destination;
            });

            /*-------------------------*/

            StateMachine.Configure(GameState.Init)
                .OnActivate(InitGame)
                .Permit(GameTrigger.InitToLobby, GameState.Lobby);

            /*-------------------------*/

            StateMachine.Configure(GameState.Lobby)
                .InitialTransition(GameState.LobbyHome);

            StateMachine.Configure(GameState.LobbyHome)
                .SubstateOf(GameState.Lobby);

            StateMachine.Configure(GameState.LobbyHome)
                .Permit(GameTrigger.Play, GameState.Playing);

            StateMachine.Configure(GameState.LobbySettings)
                .SubstateOf(GameState.Lobby);

            StateMachine.Configure(GameState.LobbyHome)
                .Permit(GameTrigger.ShowSettings, GameState.LobbySettings);

            StateMachine.Configure(GameState.LobbySettings)
                .Permit(GameTrigger.BackToLobby, GameState.LobbyHome);

            /*-------------------------*/

            StateMachine.Configure(GameState.Playing)
                .OnEntry(() =>
                {
                    SaveDataTommy();
                    SceneLoader.Instance.LoadGameScene(null);
                })
                .OnExit(()=>SceneLoader.Instance.LoadLobbyScene(LoadDataTommy));

            StateMachine.Configure(GameState.Playing)
                .Permit(GameTrigger.BackToLobby, GameState.LobbyHome);
        }

        protected override void OnStart()
        {
            Wallet ??= new _SDK.Money.Wallet();

            LoadDataTommy();
            // ensure resources not null
            Resources ??= FindObjectOfType<GameResources>();

            StateMachine.Activate();
        }
        private void LoadDataTommy()
        {
            ItemManager.LoadItemsExample();
        }

        private void SaveDataTommy()
        {
            ItemManager.SaveItemsExample();
        }

        private void InitGame()
        {
            Fire(GameTrigger.InitToLobby);
        }

        protected override void OnPlayingEntry()
        {
        }
    }
}