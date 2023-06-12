using System.Collections.Generic;
using System.Linq;
using _GAME._Scripts.Game;
using _GAME._Scripts.Npc;
using _NEWGAME._Scripts.Mission.UI;
using _SDK.UI;
using DG.Tweening;
using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _GAME._Scripts
{
    public class MissionMonitorPanel : AbstractPanel
    {
        // used to spawn npc during missions;
        [SerializeField] private QuestPanel questPanelPrefab;
        [SerializeField] private EnemyPanel enemyPanelPrefab;

        [SerializeField] private Transform questPanelHolder;
        [SerializeField] private Transform enemyPanelHolder;

        [SerializeField] private TMP_Text    scoreText;
        [SerializeField] private TMP_Text    scoreDescriptionText;
        [SerializeField] private CanvasGroup newHighscore;

        [SerializeField] private GameObject changeWeaponPanel;

        private List<EnemyPanel>                   enemyPanels;
        private Dictionary<SubMission, QuestPanel> SubMissionToQuestPanel;

        private List<QuestPanel> questPanels;

        private Dictionary<ModelType, Sprite> AvatarByModelType => GameManager.Instance.Resources.NpcResources.AvatarByModelType;

        private Dictionary<ModelType, EnemyPanel> enemyCountPanels;

        public void AddEnemiesPanel(KillNpcQuest mission)
        {
            enemyPanels = new();
            for (int i = 0; i < mission.Count; i++)
            {
                EnemyPanel panel = Instantiate(enemyPanelPrefab, enemyPanelHolder);
                enemyPanels.Add(panel);
                panel.Init(mission.Type, AvatarByModelType[mission.Type]);
            }
        }

        public void AddQuest(SubMission subMission)
        {
            SubMissionToQuestPanel ??= new Dictionary<SubMission, QuestPanel>();

            QuestPanel questPanel = Instantiate(questPanelPrefab, questPanelHolder)
                .Init(subMission.questDescription, subMission.Progress, GameFormat.RewardCash(subMission.rewardCash));
            SubMissionToQuestPanel.Add(subMission, questPanel);
        }

        public void RemoveQuest(SubMission subMission)
        {
                SubMissionToQuestPanel[subMission].FinishProgress();
        }

        public void UpdateQuestProgress(SubMission subMission)
        { 
                SubMissionToQuestPanel[subMission].UpdateProgress(subMission.Progress);
        }

        public void UpdateAnimator(int index, ModelType missionType)
        {
            enemyPanels.SkipWhile(panel => panel.modelType != missionType).Skip(index).First().Die();
        }

        public void ShowDescription(string description)
        {
            scoreText.ChangeTextTo(description, 60f);
        }

        public void CleanUp()
        {
            if (enemyPanels != null)
            {
                foreach (EnemyPanel enemyPanel in enemyPanels)
                {
                    enemyPanel.Out();
                    Destroy(enemyPanel.gameObject, .4f);
                }

                enemyPanels.Clear();
            }

            if (SubMissionToQuestPanel != null)
            {
                foreach (QuestPanel questPanel in SubMissionToQuestPanel.Values)
                {
                    Destroy(questPanel.gameObject);
                }

                SubMissionToQuestPanel?.Clear();
            }

            HideScore();
        }

        public EnemyPanel AddEnemyCountPanel(SpawnNpc mission)
        {
            // enemyCountPanels ??= new();
            enemyPanels ??= new();
            EnemyPanel panel = Instantiate(enemyPanelPrefab, enemyPanelHolder);

            panel.Init(mission.SpawnEnemySetting.GetModelType(0), AvatarByModelType[mission.SpawnEnemySetting.GetModelType(0)]);
            enemyPanels.Add(panel);
            // enemyCountPanels.Add(mission.Type, panel);
            return panel;
        }

        public void UpdateScore(string count)
        {
            // enemyCountPanels[type].UpdateCount(count);
            scoreText.SetText(count);
        }

        public void ShowScore(string score)
        {
            scoreDescriptionText.ChangeTextTo("SCORE:", 20f);
            scoreText.ChangeTextTo(score, 20f);
        }

        private void HideScore()
        {
            scoreDescriptionText.ChangeTextTo("", 20f);
            scoreText.ChangeTextTo("", 20f);
            newHighscore.DOFade(0, .2f);
        }

        public void ShowChangeWeaponPanel()
        {
            Gameplay.Instance.Fire(GameplayTrigger.Pause);
            changeWeaponPanel.SetActive(true);
            changeWeaponPanel.GetComponent<CanvasGroup>().DOFade(1f, .2f);
        }

        public void HideChangeWeaponPanel()
        {
            Gameplay.Instance.Fire(GameplayTrigger.Unpause);
            changeWeaponPanel.SetActive(false);
        }

        public void ShowNewHighscore()
        {
            newHighscore.DOFade(1, .2f);
        }
    }
}