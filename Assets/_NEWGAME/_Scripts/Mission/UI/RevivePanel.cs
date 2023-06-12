using System;
using _GAME._Scripts;
using _SDK.UI;
using Assets._SDK.Ads;
using Assets._SDK.Analytics;
using UnityEngine;
using UnityEngine.UI;

namespace _NEWGAME._Scripts.Mission.UI
{
    public class RevivePanel : AbstractPanel
    {
        [SerializeField] private Button buttonRevive, buttonSkip;


        private void Start()
        {
            buttonRevive.onClick.AddListener(Revive);
            buttonSkip.onClick.AddListener(Skip);
        }

        private void Skip()
        {
            Gameplay.Instance.MissionManager.Fire(MissionTrigger.EndDay);
        }

        private void Revive()
        {
            AdsManager.Instance.ShowRewarded(result =>
            {
                if (result == AdsResult.Success)
                    Gameplay.Instance.MissionManager.Fire(MissionTrigger.Retry);
            }, levelIndex: Gameplay.Instance.MissionManager.CurrentMission.order, placement: AnalyticParamKey.PLAYER_REVIVE);
        }
    }
}