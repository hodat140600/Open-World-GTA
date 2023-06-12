using System;
using System.Collections;
using _GAME._Scripts.Game;
using Assets._SDK.Ads;
using Assets._SDK.Analytics;
using Assets._SDK.Logger;
using GleyTrafficSystem;
using MyBox;
using Stateless;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _GAME._Scripts
{
    public enum MissionTrigger
    {
        Start,
        Fail,
        Retry,
        EndDay,
        NextDay
    }

    public enum MissionState
    {
        Start,
        Running,
        DayEnded,
        Failed,
        Pausing
    }

    public class MissionManager
    {
        private       int    _highestMissionScore;
        private const string HighestMissionScoreKey = nameof(MissionManager) + nameof(HighestMissionScoreKey);

        private       int    _currentMissionOrder;
        private const string CurrentMissionOrderKey = nameof(MissionManager) + nameof(_currentMissionOrder);

        private MissionResources Resources => GameManager.Instance.Resources.MissionResources;
        public Mission CurrentMission => Resources.GetMissionByOrder(_currentMissionOrder);
        public MissionMonitor MissionMonitor { get; private set; }

        public ReactiveProperty<MissionState> CurrentMissionState;
        public ReactiveProperty<MissionState> PreviousMissionState;

        private Coroutine _currentMissionCoroutine;

        public static Action<ScoreInfo> OnScoreUpdated;
        private       bool              _revivedOnce = false;

        public static Action<bool> PreventInputIf;

        private int CurrentScore => MissionMonitor.Score;
        private bool HasGotNewHighestScore => MissionMonitor.HasGotNewHighestScore;

        public MissionManager()
        {
            InitMachine();
            InitMissionMonitor();
        }

        private void InitMissionMonitor()
        {
            MissionMonitor = Object.FindObjectOfType<MissionMonitor>();
        }

        private StateMachine<MissionState, MissionTrigger> StateMachine;

        public void Fire(MissionTrigger trigger)
        {
            StateMachine.Fire(trigger);
        }

        public void InitMachine()
        {
            StateMachine         = new StateMachine<MissionState, MissionTrigger>(MissionState.Start);
            CurrentMissionState  = new ReactiveProperty<MissionState>();
            PreviousMissionState = new ReactiveProperty<MissionState>();

            StateMachine.OnTransitioned(v =>
            {
                CurrentMissionState.Value  = v.Destination;
                PreviousMissionState.Value = v.Source;
                ($"MISSION MANAGER" +
                 $"\n {v.Source}" +
                 $"\n {v.Trigger}" +
                 $"\n {v.Destination}").Log();
            });

            StateMachine.Configure(MissionState.Start)
                .OnActivate(LoadData)
                .OnEntry(Init)
                .Permit(MissionTrigger.Start, MissionState.Running);

            StateMachine.Configure(MissionState.Running)
                .OnEntryFrom(MissionTrigger.Start, _ => StartMission())
                .OnEntryFrom(MissionTrigger.Retry, _ => MissionMonitor.ClearNpcAtTommyArea())
                .Permit(MissionTrigger.EndDay, MissionState.DayEnded)
                .PermitDynamic(MissionTrigger.Fail, AllowReviveOnce);

            StateMachine.Configure(MissionState.Failed)
                .SubstateOf(MissionState.Pausing)
                .Ignore(MissionTrigger.Fail)
                .Permit(MissionTrigger.Retry, MissionState.Running)
                .Permit(MissionTrigger.EndDay, MissionState.DayEnded);

            StateMachine.Configure(MissionState.DayEnded)
                .OnEntry(() => AdsManager.Instance.ShowInterstitial(Gameplay.Instance.MissionManager.CurrentMission.order, AnalyticParamKey.END_DAY))
                .OnExit(() => MissionMonitor.EnableNpcAndTraffic())
                .SubstateOf(MissionState.Pausing)
                .Permit(MissionTrigger.Retry, MissionState.Start)
                .Permit(MissionTrigger.NextDay, MissionState.Start);

            StateMachine.Configure(MissionState.Pausing)
                .OnEntry(() => Gameplay.Instance.Fire(GameplayTrigger.Pause))
                .OnExit(() => Gameplay.Instance.Fire(GameplayTrigger.Unpause));
            
            StateMachine.Activate();
            

        }

        private void SendScoreInfo()
        {
            _highestMissionScore = Mathf.Max(CurrentScore, _highestMissionScore);

            ScoreInfo thisScoreInfo = new ScoreInfo(
                thisScore: CurrentScore,
                highscore: _highestMissionScore,
                isNewHighscore: HasGotNewHighestScore,
                killed: MissionMonitor.KilledCount,
                headshot: MissionMonitor.HeadshotCount
            );

            OnScoreUpdated?.Invoke(thisScoreInfo);
            UpdateNextMission();
        }

        private MissionState AllowReviveOnce()
        {
            if (_revivedOnce)
                return MissionState.DayEnded;

            _revivedOnce = true;
            return MissionState.Failed;
        }

        private void StartMission()
        {
            if (_currentMissionCoroutine != null)
                MyCoroutines.StopCoroutine(_currentMissionCoroutine);

            _currentMissionCoroutine = MissionMonitor.MissionRoutine(CurrentMission, _highestMissionScore).StartCoroutine();
        }

        private void Init()
        {
            MissionMonitor.CleanUpPanel();
            _revivedOnce = false;
            Gameplay.Instance.TryFire(GameplayTrigger.Unpause);
            StateMachine.Fire(MissionTrigger.Start);
        }

        private void UpdateNextMission()
        {
            GetNextMissionOrder();


            SaveData();
        }

        private void GetNextMissionOrder()
        {
            _currentMissionOrder++;
            _currentMissionOrder %= Resources.MissionCount;
            if (_currentMissionOrder == 0)
                _currentMissionOrder = 1;
        }

        private void StopSpawningEnemies()
        {
            MissionMonitor.ResetEnemyMission();
        }

        private void LoadData()
        {
            _currentMissionOrder = PlayerPrefs.GetInt(CurrentMissionOrderKey, 0);
            _highestMissionScore = PlayerPrefs.GetInt(HighestMissionScoreKey, 0);
        }

        private void SaveData()
        {
            PlayerPrefs.SetInt(CurrentMissionOrderKey, _currentMissionOrder);
            PlayerPrefs.SetInt(HighestMissionScoreKey, _highestMissionScore);
        }

        public void Finish()
        {
            StateMachine.Fire(MissionTrigger.EndDay);
        }

        public IEnumerator OnBeforeEndDate()
        {
            StopSpawningEnemies();
            SendScoreInfo();
            PreventInputIf(true);
            yield return new WaitForSeconds(HasGotNewHighestScore ? 4.5f : 4.5f);
            PreventInputIf(false);
            Fire(MissionTrigger.EndDay);
        }
    }
}