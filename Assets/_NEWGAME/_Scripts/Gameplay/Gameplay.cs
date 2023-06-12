using System;
using System.Collections;
using _GAME._Scripts._CharacterController._AI;
using _GAME._Scripts._ItemManager;
using _GAME._Scripts.Game;
using _GAME._Scripts.Inventory;
using DG.Tweening;
using Stateless;
using UniRx;
using UnityEngine;

namespace _GAME._Scripts
{
    public enum GameplayState
    {
        Init,
        Running,
        Pausing,
        ShowingSettings,
        ShowingMap,
    }

    public enum GameplayTrigger
    {
        Start,
        BackToPlaying,
        Pause,
        Unpause,
        ShowMap,
        ShowSettings,
    }

    public class Gameplay : SceneSingleton<Gameplay>
    {
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
                            return manager;
                    }
                }

                return _itemManager;
            }
            private set { _itemManager = value; }
        }

        protected override void Awake()
        {
            base.Awake();
            InitMachine();
        }

        public StateMachine<GameplayState, GameplayTrigger> StateMachine { get; private set; }
        public ReactiveProperty<GameplayState> PreviousState { get; private set; }
        public ReactiveProperty<GameplayState> CurrentState { get; private set; }

        public void Fire(GameplayTrigger trigger) => StateMachine.Fire(trigger);

        public bool CanFire(GameplayTrigger trigger) => StateMachine.CanFire(trigger);

        public bool IsInState(GameplayState playState) => StateMachine.IsInState(playState);

        public bool TryFire(GameplayTrigger Trigger)
        {
            bool canFire = CanFire(Trigger);
            if (canFire)
                Fire(Trigger);

            return canFire;
        }

        public MissionManager  MissionManager;
        public WeaponInventory WeaponInventory;

        [SerializeField] private MapSettings MapSettings;

        [SerializeField] public SpawnerController   spawnerController;
        [SerializeField] public WaypointArea        waypointArea;
        [SerializeField] public DaytimeBehaviour    daytimeBehaviour;
        public                  FormationController FormationController;

        [HideInInspector]
        public Map Map;

        private void InitMachine()
        {
            StateMachine = new StateMachine<GameplayState, GameplayTrigger>(GameplayState.Init);

            CurrentState  = new ReactiveProperty<GameplayState>(Instance.StateMachine.State);
            PreviousState = new ReactiveProperty<GameplayState>();

            Instance.StateMachine.OnTransitioned(t =>
            {
                CurrentState.Value  = t.Destination;
                PreviousState.Value = t.Source;
            });

            StateMachine.Configure(GameplayState.Init)
                .OnActivate(InitGameplay)
                .Permit(GameplayTrigger.Start, GameplayState.Running);

            StateMachine.Configure(GameplayState.Running)
                .OnEntryFrom(GameplayTrigger.Start, () => MissionManager.Fire(MissionTrigger.Start));

            StateMachine.Configure(GameplayState.Running)
                .Permit(GameplayTrigger.ShowMap, GameplayState.ShowingMap);

            StateMachine.Configure(GameplayState.Running)
                .Permit(GameplayTrigger.ShowSettings, GameplayState.ShowingSettings);

            /*-------------------------*/

            StateMachine.Configure(GameplayState.ShowingMap)
                .SubstateOf(GameplayState.Pausing)
                .Ignore(GameplayTrigger.ShowMap)
                .Permit(GameplayTrigger.BackToPlaying, GameplayState.Running);

            StateMachine.Configure(GameplayState.ShowingSettings)
                .SubstateOf(GameplayState.Pausing)
                .Ignore(GameplayTrigger.ShowSettings)
                .Permit(GameplayTrigger.BackToPlaying, GameplayState.Running);

            /*-------------------------*/

            StateMachine.Configure(GameplayState.Running)
                .Permit(GameplayTrigger.Pause, GameplayState.Pausing);

            /*-------------------------*/

            StateMachine.Configure(GameplayState.Pausing)
                .Ignore(GameplayTrigger.Pause)
                .Permit(GameplayTrigger.Unpause, GameplayState.Running)
                .OnEntry(() => SmoothTimeScale(1, 0, 0f))
                .OnExit(() => SmoothTimeScale(0, 1, 0f));

            /*-------------------------*/

            StateMachine.Activate();
        }

        private void SmoothTimeScale(float from, float to, float duration)
        {
            DOVirtual.Float(from, to, duration, v => Time.timeScale = v).SetUpdate(true)
                .OnComplete(() => Time.timeScale                    = to);
        }

        private void InitGameplay()
        {
            StartCoroutine(InitTommyWeapon());
            InitInventory();
            InitMap();
            InitNpc();
            InitMissionManager();
            InitDaytimeBehaviour();

            Fire(GameplayTrigger.Start);
        }


        private void InitNpc()
        {
            FormationController = new FormationController();
            spawnerController.Init(GameManager.Instance.Resources.NpcResources);
        }

        private void InitMap()
        {
            Map = MapSettings.Entity;
            Map.UpdateWaypointDictionary();
        }

        private void InitInventory()
        {
            WeaponInventory ??= new WeaponInventory();
        }

        private void InitMissionManager()
        {
            MissionManager ??= new MissionManager();
        }

        private void InitDaytimeBehaviour()
        {
            daytimeBehaviour ??= FindObjectOfType<DaytimeBehaviour>();
            daytimeBehaviour.Init();
        }

        private IEnumerator InitTommyWeapon()
        {
            yield return new WaitUntil(() => GameManager.Instance.Resources != null);
            yield return new WaitUntil(() => GameManager.Instance.Resources.WeaponResources != null);
            yield return new WaitUntil(() => GameManager.Instance.ItemManager != null);
            LoadTommy();
        }

        public void LoadTommy()
        {
            ItemManager.LoadItemsExample();
            foreach (WeaponType type in Enum.GetValues(typeof(WeaponType)))
            {
                if (type == WeaponType.Skin) continue;
                CollectAmmo((int)type, 0);
            }
        }

        public void CollectAmmo(int id, int amount)
        {
            ItemReference itemReference = new ItemReference(id);
            ItemManager.AddItem(itemReference);
            var ammo = ItemManager.items.Find(_ => _.id == id);
            ammo.amount = amount;
            ItemManager.onChangeItemAmount?.Invoke(ammo);
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (waypointArea == null)
            {
                waypointArea = FindObjectOfType<WaypointArea>();
            }
        }
#endif
    }
}