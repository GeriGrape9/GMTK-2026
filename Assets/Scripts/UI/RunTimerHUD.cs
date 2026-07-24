using UnityEngine;
using TMPro;

public class RunTimerHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text runTimeText;

    void Update()
    {
        if (GameClock.Instance == null) return;
        runTimeText.text = GameClock.Instance.GetFormattedRunTime();
    }
}