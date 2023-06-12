using System;
using UnityEngine;

namespace Assets._SDK.Ads
{
    public class AdsManager : GameSingleton<AdsManager>, IAdsManager
    {
        private IAdsManager _adsManager;

        public bool MustShowInterBackup { get => _adsManager.MustShowInterBackup; set => _adsManager.MustShowInterBackup = value; }

        public void Init()
        {
            if (AdsConfig.AdManagerType == AdManagerEnum.MockAds)
                _adsManager = new MockAdsManager();
            else
                _adsManager = IronSourceAd.AdManager.Instance;
            _adsManager.Init();
        }

        public void ShowRewarded(Action<AdsResult> OnCalled, int levelIndex = 0, string placement = "")
        {
            _adsManager.ShowRewarded(OnCalled, levelIndex, placement);
        }

        public void ShowInterstitial(int levelIndex = 0, string placement = "")
        {
            _adsManager.ShowInterstitial(levelIndex, placement);
        }
    }
}