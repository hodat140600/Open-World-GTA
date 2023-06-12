using Assets._SDK.Analytics;
using System;
using UniRx;
using UnityEngine;

public enum AdsType
{
    NO_AD = 0,
    INTERSTITIAL = 1,
    VIDEO_REWARD = 2,
    BANNER = 3

}
namespace IronSourceAd
{
    public class AdManager : MonoBehaviour, IAdsManager
    {
        public static readonly int REWARD_SUCCESS = 1;
        public static AdManager Instance { get; set; }
        private IMediationManager mediation;
        public Action onLoaded { get; set; }
        public Action<string> onFailedToLoad { get; set; }
        public Action onOpening { get; set; }
        public Action onClosed { get; set; }
        public Action onAdClicked { get; set; }
        public Action<int> onGetReward { get; set; }

        private FirebaseService firebaseService;
        private CompositeDisposable disposables = new CompositeDisposable();
        public bool MustShowInterBackup { get; set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                if (this != Instance)
                {
                    Destroy(gameObject);
                }
            }
        }
        public void Init()
        {
            GameObject ads = transform.GetChild(0).gameObject;
            mediation = ads.GetComponent<IMediationManager>();
            mediation.Init();
            Debug.Log("INIT MEDIATION!");
        }
        public bool ShowAd(string placement, int id = 1)
        {
            switch (id)
            {
                case (int)AdsType.NO_AD:
                    return true;
                case (int)AdsType.INTERSTITIAL:
                    mediation.ShowInterstitialAd(placement);
                    return true;
                case (int)AdsType.VIDEO_REWARD:
                    mediation.ShowRewardedAd(placement);
                    return true;
                default:
                    return false;
            }
        }
        private bool ShowAd(string _placement, int id = 1, Action onOpened = null, Action onClosed = null,
        Action onAdClicked = null, Action<int> onGetReward = null)
        {
            RegisterInterstitialListener(onOpened, onClosed, onAdClicked,
                onGetReward);
            return ShowAd(_placement, id);
        }
        public bool ShowInterstitialAd(int levelIndex, string placement)
        {
            if (!mediation.IsLoadInterstitialAd())
            {
                //mediation.LoadInterstitialAd();
                return false;
            }
            else
            {
                var result = ShowAd(placement, (int)AdsType.INTERSTITIAL);
                AnalyticsService.LogEventInterstitialShow(levelIndex, placement);
                return result;
            }
        }
        public bool ShowRewardedAd(Action<int> OnRewarded, int levelIndex, string placement)
        {
            if (!mediation.IsLoadRewardedAd())
            {
                //mediation.LoadRewardedAd();
                return false;
            }
            else
            {
                var result = ShowAd(placement, (int)AdsType.VIDEO_REWARD, null, null, null, _x =>
                {
                    if (OnRewarded != null)
                    {
                        OnRewarded(_x);
                    }
                });
                AnalyticsService.LogEventRewardedVideoShow(levelIndex, placement);
                return result;
            }
        }
        public void RegisterInterstitialListener(Action onOpened, Action onClosed, Action onAdClicked,
        Action<int> onGetReward)
        {
            this.onOpening = onOpened;
            this.onClosed = onClosed;
            this.onAdClicked = onAdClicked;
            this.onGetReward = onGetReward;
        }
        public bool ShowBanner(int bannerId = 1)
        {
            try
            {

                return mediation.ShowBanner();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                return false;
            }
        }
        public bool HideBanner(int bannerId = 1)
        {
            mediation.HideBanner();
            return true;
        }

        private void SetShowInterBackupFromFirebase()
        {
            MustShowInterBackup = firebaseService.GetRemoteConfigByKey(AdsConfig.InterBackupRemoteConfigKey).BooleanValue;
        }
        private void UnSubscribe()
        {
            disposables.Clear();
        }

        public void ShowRewarded(Action<AdsResult> onCallback, int levelIndex = 0, string placement = "")
        {
            ShowRewardedAd(result =>
            {
                AdsResult adsResult = result == REWARD_SUCCESS ? AdsResult.Success : AdsResult.Failed;
                onCallback(adsResult);

            }, levelIndex, placement);
        }

        public void ShowInterstitial(int levelIndex = 0, string placement = "")
        {
            ShowInterstitialAd(levelIndex, placement);
        }
    }
}

