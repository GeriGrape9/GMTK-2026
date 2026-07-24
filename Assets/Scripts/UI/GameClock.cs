using UnityEngine;
using System;

public class GameClock : MonoBehaviour
{
    public static GameClock Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float realSecondsPerGameDay = 300f;
    [SerializeField] private int totalDays = 3;

    private float elapsedRealSeconds;
    public int CurrentDay { get; private set; } = 1;
    public TimeSpan CurrentTimeOfDay { get; private set; }
    public float NormalizedDayProgress { get; private set; } // 0-1 through current day
    public TimeSpan RunTimeRemaining { get; private set; }
    public bool RunHasEnded { get; private set; } = false;

    public event Action<int> OnDayChanged;

    void Awake() => Instance = this;

    void Update()
    {
        if (RunHasEnded) return;

        elapsedRealSeconds += Time.deltaTime;

        double totalGameSeconds = (elapsedRealSeconds / realSecondsPerGameDay) * 86400.0;
        int dayIndex = Mathf.FloorToInt((float)(totalGameSeconds / 86400.0));
        int newDay = Mathf.Min(dayIndex + 1, totalDays);

        if (newDay != CurrentDay)
        {
            CurrentDay = newDay;
            OnDayChanged?.Invoke(CurrentDay);
        }

        double daySeconds = totalGameSeconds % 86400.0;
        NormalizedDayProgress = (float)(daySeconds / 86400.0);
        CurrentTimeOfDay = TimeSpan.FromSeconds(daySeconds);

        double totalRunSeconds = totalDays * 86400.0;
        double remaining = Math.Max(0, totalRunSeconds - totalGameSeconds);
        RunTimeRemaining = TimeSpan.FromSeconds(remaining);

        if (dayIndex >= totalDays)
            RunHasEnded = true;
    }

    public string GetFormattedDayTime() =>
        string.Format("{0:D2}:{1:D2}:{2:D2}", CurrentTimeOfDay.Hours, CurrentTimeOfDay.Minutes, CurrentTimeOfDay.Seconds);

    public string GetFormattedRunTime() =>
        string.Format("{0:D2}:{1:D2}:{2:D2}", (int)RunTimeRemaining.TotalHours, RunTimeRemaining.Minutes, RunTimeRemaining.Seconds);
}