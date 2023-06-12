using System.Collections;
using _SDK.SplashScreen;
using Assets._SDK.Ads;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : GameSingleton<LoadingManager>
{
    [SerializeField] private Image imgLoading;
    [SerializeField] private float timeLoading = 5;
    public PopupGDPR popupGDPR;
    private AppOpenAdManager appOpenAdManager;
    private string firstScene = SceneLoaderContext.LobbySceneName;

    public Slider loadingBar;

    private void Awake()
    {
        appOpenAdManager = AppOpenAdManager.Instance;
        _ = FirebaseService.Instance;
        //_ = AdsManager.Instance;
    }
  
    private void SetProgressUI(float progress) // progress is from 0 to 1
    {
        loadingBar.value = progress;
    }

    private void Start()
    {
        //DontDestroyOnLoad(gameObject);
        if (PlayerPrefs.GetInt("showGDPR", 0) == 1)
        {
            Init();
        }
        else
        {
            PlayerPrefs.SetInt("showGDPR", 1);
            popupGDPR.gameObject.SetActive(true);
            popupGDPR.SetUp();
        }
    }
    public void Init()
    {
        Debug.Log("LOADING-----");
        //RunLoadingBar();
        LoadAppOpen();
        AdsManager.Instance.Init();
        StartCoroutine(Load());
    }
    private void LoadAppOpen()
    {
        MobileAds.Initialize(initStatus => { appOpenAdManager.LoadAd(); AppStateEventNotifier.AppStateChanged += OnAppStateChanged; });
    }
    //private void RunLoadingBar()
    //{
    //    imgLoading.DOFillAmount(1, timeLoading)
    //        .SetEase(Ease.Linear)
    //        .OnComplete(() => {
    //            appOpenAdManager.ShowAdIfAvailable();
    //            SceneManager.LoadScene(firstScene); 
    //        });
    //}

    IEnumerator Load()
    {
        // Note: Doi 0.5sec truoc khi load thi se nhanh hon. 
        yield return new WaitForSeconds(0.5f);

        loadingBar.value = 0;
        float progress = 0;
        var loadGameScene = SceneManager.LoadSceneAsync(firstScene);

        loadGameScene.allowSceneActivation = false;

        // TODO: SplashScreen se show qua timelimit
        //if (AdsConfig.EnabledTypeAds.HasFlag(TypeAds.AppOpen))
        //{
        float _timeToRun = AdsConfig.CONST_TIME_WAIT_FOR_SHOW_FIRST_AOA;
        while ((_timeToRun -= Time.deltaTime) >= 0)
        {
            progress = Mathf.Clamp01((1 - _timeToRun / AdsConfig.CONST_TIME_WAIT_FOR_SHOW_FIRST_AOA) * .7f);

            SetProgressUI(progress);
            yield return null;
        }

        //AdsManager.Instance.AdsClient.ShowAOA();
        //appOpenAdManager.ShowAdIfAvailable();
        //}


        while (!loadGameScene.isDone)
        {
            progress = Mathf.MoveTowards(progress, loadGameScene.progress, Time.deltaTime);
            SetProgressUI(progress);

            //if ( !appOpenAdManager.IsShowed )
            //   appOpenAdManager.ShowAdIfAvailable();
            
            if (progress >= 0.9f)
                loadGameScene.allowSceneActivation = true;
            
            yield return null;
        }

        //AdManager.Instance.SetRemoteConfigValue();
    }
    private void OnAppStateChanged(AppState state)
    {
        // Display the app open ad when the app is foregrounded.
        Debug.Log("App State is " + state);
        if (state == AppState.Foreground)
        {
            appOpenAdManager.ShowAdIfAvailable();
        }
    }
}