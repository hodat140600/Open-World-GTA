using System.Collections;
using _GAME._Scripts;
using _GAME._Scripts.Game;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;

public class DaytimeBehaviour : MonoBehaviour
{
    [SerializeField] private Transform sky;
    [SerializeField] private TMP_Text  timerText, periodText;

    private Vector3   defaultEulerAngles;
    private Coroutine currentDateCoroutine;

    private MissionManager MissionManager => Gameplay.Instance.MissionManager;

    private int currentDate;
    private int currentHour;
    private int currentMinute;

    private bool wonBeforeDateEnded;
    private bool HasDateEnded => currentHour >= STARTING_HOUR + HOURS_PER_INGAME_DAY;
    private bool HasAnHourPassed => currentMinute >= MINUTES_PER_HOUR;

    private static readonly int REALTIME_SECONDS_PER_INGAME_DAY = 60;
    private static readonly int HOURS_PER_INGAME_DAY            = 12;
    private static readonly int MINUTES_PER_HOUR                = 60;

    private static readonly int STARTING_HOUR = 9;
    private static readonly int DEFAULT_DATE  = 1;

    private static int REALTIME_SECONDS_PER_INGAME_HOUR =>
        REALTIME_SECONDS_PER_INGAME_DAY / HOURS_PER_INGAME_DAY;

    private static float REALTIME_SECONDS_PER_INGAME_MINUTES =>
        (float)REALTIME_SECONDS_PER_INGAME_HOUR / MINUTES_PER_HOUR;

    private const string CURRENT_DATE_KEY = nameof(CURRENT_DATE_KEY);

    public void Init()
    {
        defaultEulerAngles = sky.eulerAngles;

        // MissionManager.CurrentMissionState
        //     .Where(state => state == MissionState.Running)
        //     .Subscribe(_ => StartDate());

        Gameplay.Instance.PreviousState.Where(state => state is GameplayState.Init)
            .Subscribe(_ => StartDate());
        
        MissionManager.PreviousMissionState.Where(state => state is MissionState.DayEnded)
            .Subscribe(_ => StartNextDate());
        
        MissionManager.CurrentMissionState
            .Subscribe(state => wonBeforeDateEnded = state == MissionState.DayEnded);

        LoadDate();
        UpdateTimerText();
    }

    private void StartNextDate()
    {
        currentDate += 1;
        SaveDate();
        StartDate();
    }

    private void StartDate()
    {
        if (currentDateCoroutine != null)
            StopCoroutine(currentDateCoroutine);

        currentDateCoroutine = StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        sky.eulerAngles = defaultEulerAngles;
        currentHour     = STARTING_HOUR;
        UpdateSunCycle();
        UpdateTimerText();

        while (!wonBeforeDateEnded && !HasDateEnded)
        {
            yield return WaitForAnHour();
            UpdateSunCycle();
        }

        if (!wonBeforeDateEnded)
            EndDate();
    }

    private IEnumerator WaitForAnHour()
    {
        currentMinute = 0;
        while (!HasAnHourPassed)
        {
            yield return new WaitForSeconds(REALTIME_SECONDS_PER_INGAME_MINUTES);
            currentMinute += 1;
            UpdateTimerText();
        }

        currentMinute =  0;
        currentHour   += 1;
        UpdateTimerText();
        yield return null;
    }

    private void UpdateSunCycle()
    {
        switch (currentHour)
        {
            case 9:
                RotateSun(1, 20);
                break;
            case 10:
                RotateSun(9, 135);
                break;
            case 18:
                RotateSun(3, 50);
                break;
        }
    }

    private void RotateSun(int hours, int xDegree)
    {
        sky.DORotate(xDegree * Vector3.right, REALTIME_SECONDS_PER_INGAME_HOUR * hours, RotateMode.WorldAxisAdd)
            .SetEase(Ease.Linear);
    }

    private void EndDate()
    {
        Gameplay.Instance.MissionManager.Fire(MissionTrigger.EndDay);
    }

    private void UpdateTimerText()
    {
        timerText.SetText(GameFormat.Time(currentDate, currentHour, currentMinute));
        periodText.SetText(GameFormat.TimePeriod(currentHour));
    }

    private void LoadDate()
    {
        currentDate = PlayerPrefs.GetInt(CURRENT_DATE_KEY, DEFAULT_DATE);
    }

    private void SaveDate()
    {
        PlayerPrefs.SetInt(CURRENT_DATE_KEY, currentDate);
    }
}