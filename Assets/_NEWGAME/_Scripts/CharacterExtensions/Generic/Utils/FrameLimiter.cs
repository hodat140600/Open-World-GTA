using UnityEngine;

namespace _GAME._Scripts
{
    [ClassHeader("Frame Limiter", false)]
    public class FrameLimiter : ExtendMonoBehaviour
    {
        public int desiredFPS = 60;

        void Awake()
        {
            Application.targetFrameRate = desiredFPS;
            QualitySettings.vSyncCount = 0;
        }

#if UNITY_EDITOR

        //void Update()
        //{
        //    long lastTicks = DateTime.Now.Ticks;
        //    long currentTicks = lastTicks;
        //    float delay = 1f / desiredFPS;
        //    float elapsedTime;

        //    if (desiredFPS <= 0)
        //        return;

        //    while (true)
        //    {
        //        currentTicks = DateTime.Now.Ticks;
        //        elapsedTime = (float)TimeSpan.FromTicks(currentTicks - lastTicks).TotalSeconds;
        //        if (elapsedTime >= delay)
        //        {
        //            break;
        //        }
        //    }
        //}

#endif
    }
}