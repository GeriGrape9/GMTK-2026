using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NPCInfoDisplay : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private TMP_Text crimeText;
    [SerializeField] private Image moodIcon;
    [SerializeField] private Image PrisonerFace;
    //[SerializeField] private TMP_Text taskText;
    //[SerializeField] private TMP_Text heldItemText;

    public void SetData(NPCStats stats)
    {
        if (stats == null)
            return;

        nameText.text = stats.Name;
        numberText.text = "ID #" + stats.Number;
        crimeText.text = stats.Crime;
        NPCMoods moodscript = stats.gameObject.GetComponent<NPCMoods>();
        moodIcon.sprite = moodscript.MoodIconList[(int)moodscript.FindHighestMood()];
        PrisonerFace.sprite = stats.gameObject.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite;
        /*
        taskText.text = stats.CurrentTask.ToString();
        heldItemText.text = stats.HeldItem == NPCManager.HeldItem.None
            ? ""
            : stats.HeldItem.ToString();
        */
    }
}