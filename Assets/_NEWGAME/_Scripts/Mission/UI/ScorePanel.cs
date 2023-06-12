using System;
using System.Collections;
using _GAME._Scripts;
using _GAME._Scripts.Game;
using _SDK.UI;
using Assets._SDK.Ads;
using Assets._SDK.Analytics;
using DG.Tweening;
using Extensions;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _NEWGAME._Scripts.GameTimer.UI
{
    public class ScorePanel : AbstractPanel
    {
        [SerializeField] private Button   claimSpecialButton, claimButton;
        [SerializeField] private TMP_Text scoreText,          highestScoreText;
        [SerializeField] private TMP_Text rouletteRewardText, rewardText;
        [SerializeField] private TMP_Text killedCountText,    headshotCountText;

        [SerializeField] private GameObject    newHighScoreSign;
        [SerializeField] private RectTransform RouletteArrow;

        private const float x5AbsRange = 58.5f;
        private const float x3AbsRange = 168.2f;
        private const float x2AbsRange = 280f;

        private const int CASH_PER_SCORE = 200;
        private const int DEFAULT_CASH   = 500;

        private float minX = -280f;
        private float maxX = 280f;

        private int reward;
        private int currentRouletteReward;

        private void OnEnable()
        {
            MissionManager.OnScoreUpdated += SetScore;
        }

        private void Start()
        {
            claimSpecialButton.onClick.AddListener(ClaimSpecial);
            claimButton.onClick.AddListener(Claim);
        }

        private void ClaimSpecial()
        {
            AdsManager.Instance.ShowRewarded(result =>
            {
                if (result == AdsResult.Success)
                {
                    GameManager.Instance.Wallet.Deposit(currentRouletteReward);
                    StartNextDay();
                }
            }, levelIndex: Gameplay.Instance.MissionManager.CurrentMission.order, placement: AnalyticParamKey.MULTI_PRIZE);

        
        }

        private void Claim()
        {
            GameManager.Instance.Wallet.Deposit(reward);
            StartNextDay();
        }

        // called by Animator
        private void StartRoulette()
        {
            rewardText.SetText(GameFormat.Cash(reward));
            RouletteArrow.DOAnchorPosX(maxX, 2f).SetEase(Ease.InOutFlash) //.From(-302.6f)
                .From(RouletteArrow.anchoredPosition.SetX(minX))
                .SetUpdate(true).SetLoops(-1, LoopType.Yoyo)
                .OnUpdate(() =>
                {
                    int cached = currentRouletteReward;
                    currentRouletteReward = RouletteArrow.anchoredPosition.x.Abs() switch
                    {
                        < x5AbsRange => reward * 5,
                        < x3AbsRange => reward * 3,
                        < x2AbsRange => reward * 2,
                        _            => currentRouletteReward
                    };

                    if (cached != currentRouletteReward)
                        rouletteRewardText.SetText(GameFormat.Cash(currentRouletteReward));
                });
        }

        private void OnDisable()
        {
            MissionManager.OnScoreUpdated -= SetScore;
        }

        private void StopRoulette()
        {
            RouletteArrow.DOKill();
        }

        public void SetScore(ScoreInfo info)
        {
            scoreText.SetText(info.ThisScore.ToString());
            highestScoreText.SetText(info.Highscore.ToString());
            killedCountText.SetText(info.Killed.ToString());
            headshotCountText.SetText(info.Headshot.ToString());
            newHighScoreSign.SetActive(info.IsNewHighscore);

            reward                = info.ThisScore * CASH_PER_SCORE + DEFAULT_CASH;
            currentRouletteReward = reward * 2;
            rewardText.SetText(GameFormat.Cash(reward));
            rouletteRewardText.SetText(GameFormat.Cash(currentRouletteReward));
        }

        private void StartNextDay()
        {
            Gameplay.Instance.MissionManager.Fire(MissionTrigger.NextDay);
        }
    }
}

public class ScoreInfo
{
    public int  ThisScore;
    public int  Highscore;
    public bool IsNewHighscore;
    public int  Killed;
    public int  Headshot;

    public ScoreInfo(int thisScore, int highscore, bool isNewHighscore, int killed, int headshot)
    {
        ThisScore      = thisScore;
        Highscore      = highscore;
        IsNewHighscore = isNewHighscore;
        Killed         = killed;
        Headshot       = headshot;
    }
}