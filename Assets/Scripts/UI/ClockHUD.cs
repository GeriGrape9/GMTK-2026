using UnityEngine;
using TMPro;

public class ClockHud : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text dayText;

    void Update()
    {
        if (GameClock.Instance == null) return;

        timeText.text = GameClock.Instance.GetFormattedDayTime();
        dayText.text = "DAY " + GameClock.Instance.CurrentDay;
    }
}