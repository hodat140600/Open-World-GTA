using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using Assets._SDK.Analytics;

public class AppOpenAdManager
{
    private const string test_Key = "ca-app-pub-3940256099942544/3419835294";
    private bool isEnableTest = false;

    private string ID_TIER_1 = AdsConfig.ID_TIER_1;
    private string ID_TIER_2 = AdsConfig.ID_TIER_2;
    private string ID_TIER_3 = AdsConfig.ID_TIER_3;

    private static AppOpenAdManager instance;

    private AppOpenAd ad;

    private int tierIndex = 1;

    private bool isShowingAd = false;

    private bool isFirstShow = false;
    public bool IsShowed => isFirstShow;

    public static AppOpenAdManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AppOpenAdManager();
            }

            return instance;
        }
    }

    private bool IsAdAvailable => ad != null;

    public void LoadAd()
    {
        LoadAOA();
    }

    public void LoadAOA()
    {
        string id = ID_TIER_1;
        if (tierIndex == 2)
            id = ID_TIER_2;
        else if (tierIndex == 3)
            id = ID_TIER_3;
        if (isEnableTest)
        {
            id = test_Key;
        }
        Debug.Log($"Start request Open App Ads: Tier{tierIndex}- ID:{id}");

        AdRequest request = new AdRequest.Builder().Build();

        AppOpenAd.LoadAd(id, ScreenOrientation.Portrait, request, ((appOpenAd, error) =>
        {
            if (error != null)
            {
                // Handle the error.
                Debug.LogFormat($"Failed to load AOA tier {tierIndex} - id: {id}. Reason: {error.LoadAdError.GetMessage()}");
                tierIndex++;
                if (tierIndex <= 3)
                    LoadAOA();
                else
                    tierIndex = 1;
                return;
            }

            // App open ad is loaded.
            ad = appOpenAd;
            tierIndex = 1;
            if (!isFirstShow)
            {
                ShowAdIfAvailable();
                isFirstShow = true;
            }
        }));
    }

    public void ShowAdIfAvailable()
    {
        if (isShowingAd )
        {
            return;
        }
        if (!IsAdAvailable)
        {
            LoadAd();
        }

        ad.OnAdDidDismissFullScreenContent += HandleAdDidDismissFullScreenContent;
        ad.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresentFullScreenContent;
        ad.OnAdDidPresentFullScreenContent += HandleAdDidPresentFullScreenContent;
        ad.OnAdDidRecordImpression += HandleAdDidRecordImpression;
        ad.OnPaidEvent += HandlePaidEvent;
        isFirstShow = true;
        ad.Show();

        AnalyticsService.LogEventAppOpenAdsShow();
    }

    private void HandleAdDidDismissFullScreenContent(object sender, EventArgs args)
    {
        Debug.Log("Closed app open ad");
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        isShowingAd = false;
        LoadAd();
    }

    private void HandleAdFailedToPresentFullScreenContent(object sender, AdErrorEventArgs args)
    {
        Debug.LogFormat("Failed to present the ad (reason: {0})", args.AdError.GetMessage());
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        LoadAd();
    }

    private void HandleAdDidPresentFullScreenContent(object sender, EventArgs args)
    {
        Debug.Log("Displayed app open ad");
        isShowingAd    = true;
    }

    private void HandleAdDidRecordImpression(object sender, EventArgs args)
    {
        Debug.Log("Recorded ad impression");
    }

    private void HandlePaidEvent(object sender, AdValueEventArgs args)
    {
        Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
            args.AdValue.CurrencyCode, args.AdValue.Value);
    }
}
