using System;

public enum AdsResult
{
    Success = 1,
    Failed = 0
}

public interface IAdsManager
{
    public void Init();
    public void ShowRewarded(Action<AdsResult> OnCalled, int levelIndex = 0, string placement = "");
    public void ShowInterstitial(int levelIndex = 0, string placement = "");
    public bool MustShowInterBackup { get; set; }
}