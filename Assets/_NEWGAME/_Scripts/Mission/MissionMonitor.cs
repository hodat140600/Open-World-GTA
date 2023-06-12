using System;
using System.Collections;
using System.Linq;
using _GAME._Scripts.Game;
using _GAME._Scripts.Npc.Group;
using _NEWGAME._Scripts.Game;
using Assets._SDK.Logger;
using GleyTrafficSystem;
using MyBox;
using O3DWB;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using static System.Single;

namespace _GAME._Scripts
{
    public class MissionMonitor : ObservableObject
    {
        [SerializeField] private GameObject Tommy;

        // used to navigate player at destination during go to mission;
        [SerializeField] private Target NavigatorTarget;

        // used to spawn npc during missions;
        [SerializeField] private SpawnerController Spawner;

        [SerializeField] private MissionMonitorPanel missionMonitorPanel;

        public UnityEvent spawnStarted;

        public int HighestScore { get; set; }

        public bool HasGotNewHighestScore => Score > HighestScore;


        private CompositeDisposable MissionEventSubscriptions { get; set; }

        private const int QUEST_LIMIT_COUNT = 3;
        private       int currentQuestCount = 0;

        public int KilledCount => Mission.subMissions
            .Where(sm => sm is SpawnNpc)
            .Select(mission => ((SpawnNpc)mission).KilledCount)
            .Sum();

        public int HeadshotCount => Mission.subMissions
            .Where(sm => sm is SpawnNpc)
            .Select(mission => ((SpawnNpc)mission).HeadshotCount)
            .Sum();

        public int Score => KilledCount + HeadshotCount;
        private Mission Mission;

        public void Start()
        {
            spawnStarted.AddListener(() => SfxManager.Instance.Play(Sounds.MissionReceive));
        }

        public IEnumerator MissionRoutine(Mission mission, int currentHighestScore)
        {
            HighestScore      = currentHighestScore;
            Mission           = mission;
            currentQuestCount = 0;

            ResetEnemyMission();
            RenewSubscription();
            missionMonitorPanel.CleanUp();
            ResetMissionProgress(mission);

            foreach (SubMission subMission in mission.subMissions)
            {
                if (subMission.hasQuest)
                {
                    yield return new WaitUntil(() => currentQuestCount < QUEST_LIMIT_COUNT);

                    // add this to let the old quest finished it animator
                    if (currentQuestCount == QUEST_LIMIT_COUNT - 1)
                        yield return new WaitForSeconds(3f);

                    AddQuest(subMission);
                }

                subMission.Accept(this);

                if (subMission.blockBehind)
                    yield return new WaitUntil(() => subMission.Done);
            }
        }

        private void ResetMissionProgress(Mission mission)
        {
            foreach (SubMission subMission in mission.subMissions)
            {
                subMission.ResetProgress();
            }
        }

        private void RenewSubscription()
        {
            MissionEventSubscriptions?.Dispose();
            MissionEventSubscriptions = new CompositeDisposable();
        }

        public void ClearNpcAtTommyArea()
        {
            // TODO @DAT reset all nextbots position, let them far away again 
            NotifyObservers(new ClearEnemyCurrent());
        }

        public void ResetEnemyMission()
        {
            NotifyObservers();
        }

        public void ConfigureKillQuest(KillNpcQuest mission)
        {
            mission.CurrentCount = 0;

            ListenTo<NpcKilledEvent>(onNext: killedEnemy =>
            {
                if (mission.Type != killedEnemy.Type) return;

                mission.CurrentCount++;
                missionMonitorPanel.UpdateQuestProgress(mission);

                if (mission.CurrentCount == mission.Count)
                {
                    MarkDone(mission);
                }
            });
        }

        public void DisableNpcAndTraffic()
        {
            // todo NPC run out
            TrafficManager.Instance.SetTrafficDensity(0);
        }

        public void EnableNpcAndTraffic()
        {
            // todo NPC walk in
            TrafficManager.Instance.SetTrafficDensity(10);
        }

        public void ConfigureHeadshotQuest(HeadshotQuest mission)
        {
            mission.CurrentCount = 0;

            ListenTo<NpcKilledEvent>(onNext: killedEnemy =>
            {
                if (!killedEnemy.IsHeadshot) return;
                // if (mission.Type != killedEnemy.Type) return;

                mission.CurrentCount++;
                missionMonitorPanel.UpdateQuestProgress(mission);

                if (mission.CurrentCount == mission.Count)
                {
                    MarkDone(mission);
                }
            });
        }

        public void ConfigureKillWithWeaponQuest(KillWithWeaponQuest mission)
        {
            mission.CurrentCount = 0;

            ListenTo<NpcKilledEvent>(onNext: killedEnemy =>
            {
                if (mission.WeaponType != killedEnemy.KilledByWeapon) return;

                mission.CurrentCount++;
                missionMonitorPanel.UpdateQuestProgress(mission);

                if (mission.CurrentCount == mission.Count)
                {
                    MarkDone(mission);
                }
            });
        }

        private IEnumerator GoToSubMissionCoroutine(GoToQuest mission)
        {
            NavigatorTarget.transform.position = mission.Position;
            NavigatorTarget.Activate(true);
            float distance = PositiveInfinity;

            while (distance > 5f)
            {
                if (!Tommy)
                    yield break;

                distance = Vector3.Distance(Tommy.transform.position, mission.Position);

                yield return _waitForFixedUpdate;
            }

            spawnStarted.Invoke();
            NavigatorTarget.Activate(false);
            MarkDone(mission);
        }

        public void ConfigureGoToSubMission(GoToQuest mission)
        {
            GoToSubMissionCoroutine(mission).StartCoroutine();
        }

        public void ConfigureSpawnTommyAtSubMission(SpawnTommyAtSubMission mission)
        {
            Tommy.transform.position = mission.Position + Vector3.up * 2;
            MarkDone(mission);
        }

        public void ConfigureSpawnNpcAtSubMission(SpawnNpcGroupSubMission spawn)
        {
            //spawn.NpcGroupSettings.npcGroup.startingWaypointPosition = spawn.AtPosition;

            //SpawnNpcGroup(spawn.NpcGroupSettings.npcGroup);
        }

        private void SpawnNpcGroup(NpcGroup groupToSpawn)
        {
            Spawner.InitGroupGameObjects(groupToSpawn);
            Spawner.SpawnGroup(groupToSpawn);
        }

        public void SpawnNpc(SpawnNpc mission)
        {
            NotifyObservers(mission.SpawnEnemySetting);

            missionMonitorPanel.ShowScore(Score.ToString());
            DisableNpcAndTraffic();

            ListenTo<NpcKilledEvent>(onNext: killedEnemy =>
            {
                mission.KilledCount++;
                if (killedEnemy.IsHeadshot)
                    mission.HeadshotCount++;

                if (HasGotNewHighestScore)
                    missionMonitorPanel.ShowNewHighscore();

                missionMonitorPanel.UpdateScore(Score.ToString());
            });
        }

        private WaitForFixedUpdate _waitForFixedUpdate = new();

        private IEnumerator RunQuestCoroutine(RunQuest mission)
        {
            Vector3 cachedPosition = Tommy.transform.position;
            Vector3 tommyPosition;

            while (mission.CurrentDistance < mission.Distance)
            {
                if (!Tommy)
                    yield break;

                tommyPosition = Tommy.transform.position;

                mission.CurrentDistance += Vector3.Distance(cachedPosition, tommyPosition);
                cachedPosition          =  tommyPosition;

                missionMonitorPanel.UpdateQuestProgress(mission);
                yield return _waitForFixedUpdate;
            }

            MarkDone(mission);
        }


        public void ConfigureRunQuest(RunQuest mission)
        {
            RunQuestCoroutine(mission).StartCoroutine();
        }

        public void Delay(DelaySubMission delay)
        {
            DelayCoroutine(delay).StartCoroutine();
        }

        private IEnumerator DelayCoroutine(DelaySubMission delay)
        {
            yield return new WaitForSeconds(delay.DelaySeconds);
            MarkDone(delay);
        }

        private void MarkDone(SubMission subMission)
        {
            subMission.Done = true;
            if (subMission.hasQuest)
                FinishQuest(subMission);
        }

        private void AddQuest(SubMission thisSubMission)
        {
            currentQuestCount = Mathf.Min(currentQuestCount + 1, QUEST_LIMIT_COUNT);
            missionMonitorPanel.AddQuest(thisSubMission);
        }

        private void FinishQuest(SubMission thisSubMission)
        {
            currentQuestCount = Mathf.Max(currentQuestCount - 1, 0);
            missionMonitorPanel.RemoveQuest(thisSubMission);
            GameManager.Instance.Wallet.Deposit(thisSubMission.rewardCash);
        }

        private void ListenTo<TEvent>(Action<TEvent> onNext) where TEvent : IMissionEvent
        {
            MessageBroker.Default.Receive<TEvent>().Subscribe(onNext).AddTo(MissionEventSubscriptions);
        }

        private void OnDestroy()
        {
            MissionEventSubscriptions?.Dispose();
        }

        public void CleanUpPanel()
        {
            missionMonitorPanel.CleanUp();
        }

        public void ShowChangeWeapon(ChangeWeaponSubMission mission)
        {
            missionMonitorPanel.ShowChangeWeaponPanel();
            CompositeDisposable oneTimeSubscription = new();
            GameManager.Instance.ItemManager.onEquipItem
                .AsObservable().Subscribe(_ =>
                {
                    missionMonitorPanel.HideChangeWeaponPanel();
                    MarkDone(mission);
                    oneTimeSubscription.Dispose();
                }).AddTo(oneTimeSubscription);
        }
    }

    public class ClearEnemyCurrent : IDataObserver
    {
    }
}