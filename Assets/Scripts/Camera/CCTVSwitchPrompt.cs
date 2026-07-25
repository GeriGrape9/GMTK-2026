using UnityEngine;
using TMPro;

public class CCTVSwitchPrompt : MonoBehaviour
{
    [SerializeField] private GameObject promptRoot;
    [SerializeField] private TMP_Text promptText;

    void Start()
    {
        promptRoot.SetActive(false);
    }

    public void Show(string label)
    {
        promptText.text = label;
        promptRoot.SetActive(true);
    }

    public void Hide()
    {
        promptRoot.SetActive(false);
    }
}