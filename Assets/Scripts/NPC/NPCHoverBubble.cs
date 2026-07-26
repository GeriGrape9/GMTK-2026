using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class NPCHoverBubble : MonoBehaviour
{
    public GameObject bubbleRoot;       // the world-space canvas object
    [SerializeField] private NPCInfoDisplay infoDisplay;  // lives on the same canvas
    [SerializeField] private Image MoodSprite;  // lives on the same canvas
    [SerializeField] private Image PrisonerFace;  // lives on the same canvas
    private CCTVManager cctvController;
    private NPCManager NPCManager;

    private void UpdateMoodIcon()
    {
        NPCMoods NPCMoods = GetComponent<NPCMoods>();
        NPCMoods.Moods HighestMood = NPCMoods.FindHighestMood();
        MoodSprite.sprite = NPCMoods.MoodIconList[(int)HighestMood];   
    }

    private void Awake()
    {
        cctvController = GetComponent<NPCStats>().CCTVManager;
        NPCManager = GetComponent<NPCStats>().NPCManager;
        PrisonerFace.sprite = transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    void Start()
    {
        bubbleRoot.SetActive(false);
        UpdateMoodIcon();
    }

    void Update()
    {
        if (bubbleRoot.activeSelf && cctvController != null && cctvController.ActiveCam != null)
        {
            Vector3 directionFromCamera = bubbleRoot.transform.position - cctvController.ActiveCam.transform.position;
            bubbleRoot.transform.rotation = Quaternion.LookRotation(directionFromCamera);
        }
    }

    public void Show(NPCStats stats)
    {
        infoDisplay.SetData(stats);
        bubbleRoot.SetActive(true);
        NPCManager.ClickedNPC = stats.gameObject;
        UpdateMoodIcon();
    }

    public void Hide()
    {
        bubbleRoot.SetActive(false);
        NPCManager.ClickedNPC = null;
    }
}