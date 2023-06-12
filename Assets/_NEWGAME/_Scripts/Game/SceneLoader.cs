using System;
using System.Collections;
using _SDK.SplashScreen;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _NEWGAME._Scripts.Game
{
    public class SceneLoader : GameSingleton<SceneLoader>
    {
        public Slider loadingBar;

        public void LoadGameScene(Action onSuccess)
        {
            Time.timeScale = 1;
            Load(SceneLoaderContext.GameSceneName, onSuccess).StartCoroutine();
        }

        public void LoadLobbyScene(Action onSuccess)
        {
            Time.timeScale = 1;
            Load(SceneLoaderContext.LobbySceneName, onSuccess).StartCoroutine();
        }

        IEnumerator Load(string name, Action onSuccess)
        {
            var loadingScene = SceneManager.LoadSceneAsync(SceneLoaderContext.LoadingSceneInGameName);
            yield return new WaitUntil(() => loadingScene.isDone);
            loadingBar = FindObjectOfType<Slider>();

            // Note: Doi 0.5sec truoc khi load thi se nhanh hon. 
            yield return new WaitForSeconds(0.5f);

            loadingBar.value = 0;
            float progress      = 0;
            var   loadGameScene = SceneManager.LoadSceneAsync(name);

            while (!loadGameScene.isDone)
            {
                progress = Mathf.MoveTowards(progress, loadGameScene.progress, Time.deltaTime);
                SetProgressUI(progress);

                yield return null;
            }

            onSuccess?.Invoke();
        }

        private void SetProgressUI(float progress) // progress is from 0 to 1
        {
            loadingBar.value = progress;
        }
    }
}