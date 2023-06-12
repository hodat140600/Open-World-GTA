using UnityEngine;
public static class ApplicationStart
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnStartBeforeSceneLoad()
    {
        //Debug.unityLogger.logEnabled = false;
        Application.lowMemory += OnLowMemory;
    }

        

    private static void OnLowMemory()
    {
        Resources.UnloadUnusedAssets();
    }

}