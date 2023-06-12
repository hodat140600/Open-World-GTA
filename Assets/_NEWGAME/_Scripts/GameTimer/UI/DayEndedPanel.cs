using System;
using System.Collections;
using _GAME._Scripts;
using _SDK.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _NEWGAME._Scripts.GameTimer.UI
{
    public class DayEndedPanel : AbstractPanel
    {
        [SerializeField] private Button   NextDayButton;
        [SerializeField] private TMP_Text scoreText, highestScoreText;
        
        private void OnEnable()
        {
            MissionManager.OnScoreUpdated += SetScore;
        }

        private void OnDisable()
        {
            MissionManager.OnScoreUpdated -= SetScore;
        }

        public void SetScore(ScoreInfo scoreInfo)
        {
            UpdateText(scoreInfo.ThisScore.ToString(), scoreInfo.Highscore.ToString());
        }

        private void UpdateText(string currentScore, string highestScore)
        {
            scoreText.SetText(currentScore);
            highestScoreText.SetText(highestScore);
        }

        private void Start()
        {
            NextDayButton.onClick.AddListener(StartNextDay);
        }

        private void StartNextDay()
        {
            Gameplay.Instance.MissionManager.Fire(MissionTrigger.NextDay);
        }
    }
}