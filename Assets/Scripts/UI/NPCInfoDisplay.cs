using UnityEngine;
using TMPro;

public class NPCInfoDisplay : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private TMP_Text crimeText;
    //[SerializeField] private TMP_Text taskText;
    //[SerializeField] private TMP_Text heldItemText;

    public void SetData(NPCStats stats)
    {
        if (stats == null)
            return;

        nameText.text = stats.Name;
        numberText.text = "ID #" + stats.Number;
        crimeText.text = stats.Crime;
        /*
        taskText.text = stats.CurrentTask.ToString();
        heldItemText.text = stats.HeldItem == NPCManager.HeldItem.None
            ? ""
            : stats.HeldItem.ToString();
        */
    }
}