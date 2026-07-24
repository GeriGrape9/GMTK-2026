using UnityEngine;
using TMPro;

public class ClockScroller : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private TMP_Text contentTemplate;
    [SerializeField] private RectTransform pointerAnchor; // fixed marker in the viewport
    [Header("Schedule")]
    [SerializeField] private DailySchedule schedule;
    private RectTransform copyA;
    private RectTransform copyB;
    private float contentHeight;
    private float pointerY;

    void Start()
    {
        contentTemplate.ForceMeshUpdate();
        contentHeight = contentTemplate.preferredHeight;

        copyA = contentTemplate.rectTransform;
        GameObject dupe = Instantiate(contentTemplate.gameObject, copyA.parent);
        copyB = dupe.GetComponent<RectTransform>();

        pointerY = pointerAnchor.anchoredPosition.y;

        GameClock.Instance.OnDayChanged += HandleDayChanged;
        RebuildContentForDay(GameClock.Instance.CurrentDay);
    }

    void OnDestroy()
    {
        if (GameClock.Instance != null)
            GameClock.Instance.OnDayChanged -= HandleDayChanged;
    }

    void Update()
    {
        if (GameClock.Instance == null) return;

        // Where in the content the "current moment" sits
        float offsetIntoContent = GameClock.Instance.NormalizedDayProgress * contentHeight;

        // Position content so that point lands exactly under the pointer
        float baseY = pointerY + offsetIntoContent;

        copyA.anchoredPosition = new Vector2(copyA.anchoredPosition.x, Mod(baseY, contentHeight));
        copyB.anchoredPosition = new Vector2(copyB.anchoredPosition.x, Mod(baseY, contentHeight) - contentHeight);
    }

    private float Mod(float value, float mod)
    {
        float r = value % mod;
        return r < 0 ? r + mod : r;
    }

    private void HandleDayChanged(int newDay)
    {
        RebuildContentForDay(newDay);
    }

    private void RebuildContentForDay(int day)
    {
        string content = BuildDayContent(day);
        copyA.GetComponent<TMP_Text>().text = content;
        copyB.GetComponent<TMP_Text>().text = content;

        copyA.GetComponent<TMP_Text>().ForceMeshUpdate();
        contentHeight = copyA.GetComponent<TMP_Text>().preferredHeight;
    }
    private string BuildDayContent(int day)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int hour = 0; hour < 24; hour++)
        {
            var entry = schedule.entries.Find(e => e.hour == hour);
            if (!string.IsNullOrEmpty(entry.label))
                sb.AppendLine($"{hour:D2}:00 - {entry.label}");
            else
                sb.AppendLine($"{hour:D2}:00");
        }
        return sb.ToString();
    }
}