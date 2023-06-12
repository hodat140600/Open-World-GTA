using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMediationManager
{
    void Init();

    bool IsLoadInterstitialAd();
    void LoadInterstitialAd();
    void ShowInterstitialAd(string placement);

    bool IsLoadRewardedAd();
    //void LoadRewardedAd();
    void ShowRewardedAd(string placement);

    void HideBanner();
    bool ShowBanner();
}
