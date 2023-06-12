//using Assets._SDK.Ads.AdsClient;

using System.Collections;
using _SDK.SplashScreen;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(1)]
public class LoadingPanel : MonoBehaviour
{
    public Slider loadingBar;

    [SerializeField] private AudioMixer SfxMixer, BgmMixer;


    private void Awake()
    {
        bool isFirstTimeLoadGame = SceneLoaderContext.NameOfCurrentSceneToLoad == "";

        if (isFirstTimeLoadGame)
        {
            SceneLoaderContext.NameOfCurrentSceneToLoad = SceneLoaderContext.LobbySceneName;
            UpdateVolume();
        }

        StartCoroutine(LoadNextScene(SceneLoaderContext.NameOfCurrentSceneToLoad));
    }

    private void UpdateVolume()
    {
        string SfxVolumeKey     = nameof(SfxVolumeKey);
        string BgmVolumeKey     = nameof(BgmVolumeKey);
        string Volume_Parameter = "Volume";
        SfxMixer.SetFloat(Volume_Parameter, PlayerPrefs.GetFloat(SfxVolumeKey, 0));
        BgmMixer.SetFloat(Volume_Parameter, PlayerPrefs.GetFloat(BgmVolumeKey, 0));
    }

    public IEnumerator LoadNextScene(string sceneName)
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.5f);
        var operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;
            yield return null;
        }
    }

    public IEnumerator LoadNextSceneWithAOA(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);
        var loadGameScene = SceneManager.LoadSceneAsync(sceneName);
        while (!loadGameScene.isDone)
        {
            float progress = Mathf.Clamp01(loadGameScene.progress / 0.9f);
            loadingBar.value = progress;
            yield return null;
        }
    }
}