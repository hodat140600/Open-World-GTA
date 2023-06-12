using Assets._SDK.Ads.AdsClient;
using IronSourceAd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IronSourceManager : MonoBehaviour, IMediationManager
{
    private string appKey = AdsConfig.ironSourceAppKey;

    void InterstitialReady()
    {
        Debug.Log("LOAD INTER DONE!");
    }
    void RewardedReady()
    {
        Debug.Log("LOAD REWARD DONE!");
    }
    private void OnEnable()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;

        //Add ImpressionSuccess Event
        //IronSourceEvents.onImpressionSuccessEvent += ImpressionSuccessEvent;
        IronSourceEvents.onImpressionDataReadyEvent += ImpressionDataReadyEvent;
    }
    public void Init()
    {
        //Add Init Event
        IronSource.Agent.init(appKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.BANNER);
        IronSource.Agent.shouldTrackNetworkState(true);
        IronSourceConfig.Instance.setClientSideCallbacks(true);
        IronSource.Agent.setAdaptersDebug(false);
        if (PlayerPrefs.GetInt("IronSource_Consent") == 1)
            IronSource.Agent.setConsent(true);

        else
            IronSource.Agent.setConsent(false);
        RegisterBannerAdsCallback();
        RegisterInterstitialAdsCallback();
        RegisterRewardAdsCallback();

        Debug.Log("INIT APP KEY: " + appKey);
    }
    public void SetConsent()
    {
        var consent = PlayerPrefs.GetInt("IronSource_Consent", 1) == 1;
        IronSource.Agent.setConsent(consent);
        Debug.Log(consent);
    }
    void SdkInitializationCompletedEvent()
    {
        Debug.Log("unity-script: I got SdkInitializationCompletedEvent");
        IronSource.Agent.validateIntegration();
        LoadBanner();
        LoadInterstitialAd();
        //LoadRewardedAd();
    }
    #region Banner ADS
    private bool bannerAdsAvailable = false;
    void RegisterBannerAdsCallback()
    {
        IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
        IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
        IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
        IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
        IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
        IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;
    }
    public void LoadBanner()
    {
        var size = IronSourceBannerSize.SMART;
        size.SetAdaptive(true);
        IronSource.Agent.loadBanner(size, IronSourceBannerPosition.BOTTOM);
        Debug.Log("Start load Banner Ad...");
    }
    void RetryLoadBannerAds()
    {
        if (!bannerAdsAvailable)
        {
            Debug.Log("Banner Ad Load Failed. Try again ");
            //Invoke(nameof(LoadBanner), 3f);
            LoadBanner();
        }
    }
    public bool ShowBanner()
    {
        if (bannerAdsAvailable)
        {
            IronSource.Agent.displayBanner();
            return true;
        }
        else
        {
            RetryLoadBannerAds();
            return false;
        }
    }
    public void HideBanner()
    {
        IronSource.Agent.hideBanner();
    }
    //Invoked once the banner has loaded
    void BannerAdLoadedEvent()
    {
        bannerAdsAvailable = true;
        Debug.Log("unity-script: I got BannerAdLoadedEvent");
        //CancelInvoke(nameof(RetryLoadBannerAds));
        ShowBanner();
    }
    //Invoked when the banner loading process has failed.
    //@param description - string - contains information about the failure.
    void BannerAdLoadFailedEvent(IronSourceError error)
    {
        bannerAdsAvailable = false;
        RetryLoadBannerAds();
        Debug.Log("unity-script: I got BannerAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
    }
    // Invoked when end user clicks on the banner ad
    void BannerAdClickedEvent()
    {
        Debug.Log("unity-script: I got BannerAdClickedEvent");
    }
    //Notifies the presentation of a full screen content following user click
    void BannerAdScreenPresentedEvent()
    {
        Debug.Log("unity-script: I got BannerAdScreenPresentedEvent");
    }
    //Notifies the presented screen has been dismissed
    void BannerAdScreenDismissedEvent()
    {
        Debug.Log("unity-script: I got BannerAdScreenDismissedEvent");
    }
    //Invoked when the user leaves the app
    void BannerAdLeftApplicationEvent()
    {
        Debug.Log("unity-script: I got BannerAdLeftApplicationEvent");
    }
    #endregion

    #region Interstitial ADS
    private float RETRY_LOAD_AD = 5f;
    void RegisterInterstitialAdsCallback()
    {
        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;
    }

    private IEnumerator RetryLoadInterAds(float time)
    {
        yield return new WaitForSeconds(time);
        LoadInterstitialAd();
    }
    // Invoked when the initialization process has failed.
    // @param description - string - contains information about the failure.
    void InterstitialAdLoadFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
        StartCoroutine(RetryLoadInterAds(5f));
    }
    // Invoked when the ad fails to show.
    // @param description - string - contains information about the failure.
    void InterstitialAdShowFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
        LoadInterstitialAd();
    }
    // Invoked when end user clicked on the interstitial ad
    void InterstitialAdClickedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdClickedEvent");
    }
    // Invoked when the interstitial ad closed and the user goes back to the application screen.
    void InterstitialAdClosedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdClosedEvent");
        LoadInterstitialAd();
    }
    // Invoked when the Interstitial is Ready to shown after load function is called
    void InterstitialAdReadyEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdReadyEvent");
        InterstitialReady();
    }
    // Invoked when the Interstitial Ad Unit has opened
    void InterstitialAdOpenedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdOpenedEvent");
    }
    // Invoked right before the Interstitial screen is about to open.
    // NOTE - This event is available only for some of the networks. 
    // You should not treat this event as an interstitial impression, but rather use InterstitialAdOpenedEvent
    void InterstitialAdShowSucceededEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdShowSucceededEvent");
    }
    public bool IsLoadInterstitialAd()
    {
        return IronSource.Agent.isInterstitialReady();
    }
    public void LoadInterstitialAd()
    {
        Debug.Log("INTER loading...");
        IronSource.Agent.loadInterstitial();
    }
    public void ShowInterstitialAd(string placement)
    {
        IronSource.Agent.showInterstitial(placement);
    }
    #endregion

    #region Rewarded ADS
    private bool isCompeleteAdsRV = false;
    void GetReward()
    {
        if (AdManager.Instance.onGetReward != null)
        {
            AdManager.Instance.onGetReward(AdManager.REWARD_SUCCESS);
        }
    }
    void RegisterRewardAdsCallback()
    {
        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
        IronSourceEvents.onRewardedVideoAdLoadFailedEvent += RewardedVideoAdLoadFailed;
    }
    //Invoked when the RewardedVideo ad view has opened.
    //Your Activity will lose focus. Please avoid performing heavy 
    //tasks till the video ad will be closed.
    void RewardedVideoAdOpenedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent");
    }

    //Invoked when the RewardedVideo ad view is about to be closed.
    //Your activity will now regain its focus.
    void RewardedVideoAdClosedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdClosedEvent");
        //LoadRewardedAd();
    }
    //Invoked when there is a change in the ad availability status.
    //@param - available - value will change to true when rewarded videos are available. 
    //You can then show the video by calling showRewardedVideo().
    //Value will change to false when no videos are available.
    void RewardedVideoAvailabilityChangedEvent(bool available)
    {
        //Change the in-app 'Traffic Driver' state according to availability.
        bool rewardedVideoAvailability = available;
        if (available)
        {
            RewardedReady();
        }
        else
        {
            Debug.Log("reward Loading...");
        }
        Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " + rewardedVideoAvailability);
    }

    //Invoked when the user completed the video and should be rewarded. 
    //If using server-to-server callbacks you may ignore this events and wait for 
    // the callback from the  ironSource server.
    //@param - placement - placement object which contains the reward data
    void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
    {
        Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent, amount = " + placement.getRewardAmount() + " name = " + placement.getRewardName());
        isCompeleteAdsRV = true;
        GetReward();

    }
    //Invoked when the Rewarded Video failed to show
    //@param description - string - contains information about the failure.
    void RewardedVideoAdShowFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent");
        //if (!IsLoadRewardedAd())
        //    LoadRewardedAd();
    }
    void RewardedVideoAdLoadFailed(IronSourceError error)
    {
        Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
        //LoadRewardedAd();
    }
    // ----------------------------------------------------------------------------------------
    // Note: the events below are not available for all supported rewarded video ad networks. 
    // Check which events are available per ad network you choose to include in your build. 
    // We recommend only using events which register to ALL ad networks you include in your build. 
    // ----------------------------------------------------------------------------------------

    //Invoked when the video ad starts playing. 
    void RewardedVideoAdStartedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdStartedEvent");
    }
    //Invoked when the video ad finishes playing. 
    void RewardedVideoAdEndedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdEndedEvent");
    }
    //Invoked when the video ad is clicked. 
    void RewardedVideoAdClickedEvent(IronSourcePlacement placement)
    {
    }

    public bool IsLoadRewardedAd()
    {
        return IronSource.Agent.isRewardedVideoAvailable();
    }
    //public void LoadRewardedAd()
    //{
    //    Debug.Log("reward Loading...");
    //    IronSource.Agent.loadRewardedVideo();
    //}
    public void ShowRewardedAd(string placement)
    {
        IronSource.Agent.showRewardedVideo(placement);
    }
    #endregion


    #region Ad Impression
    //void ImpressionSuccessEvent(IronSourceImpressionData impressionData)
    //{
    //    Debug.Log("unity - script: I got ImpressionSuccessEvent ToString(): " + impressionData.ToString());
    //    Debug.Log("unity - script: I got ImpressionSuccessEvent allData: " + impressionData.allData);

    //    if (impressionData != null && !string.IsNullOrEmpty(impressionData.adNetwork))
    //    {
    //        AnalyticsRevenueAds.SendEventFB(impressionData);
    //    }
    //}

    void ImpressionDataReadyEvent(IronSourceImpressionData impressionData)
    {
        Debug.Log("unity - script: I got ImpressionDataReadyEvent ToString(): " + impressionData.ToString());
        Debug.Log("unity - script: I got ImpressionDataReadyEvent allData: " + impressionData.allData);

        if (impressionData != null && !string.IsNullOrEmpty(impressionData.adNetwork))
        {
            AnalyticsRevenueAds.SendEventFB(impressionData);
            AnalyticsRevenueAds.SendEventAF(impressionData);
        }
    }

    #endregion
    void OnApplicationPause(bool isPaused)
    {
        Debug.Log("unity-script: OnApplicationPause = " + isPaused);
        IronSource.Agent.onApplicationPause(isPaused);
    }
}
