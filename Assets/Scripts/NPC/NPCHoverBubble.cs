using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class NPCHoverBubble : MonoBehaviour
{
    [SerializeField] private GameObject bubbleRoot;       // the world-space canvas object
    [SerializeField] private NPCInfoDisplay infoDisplay;  // lives on the same canvas
    private CCTVManager cctvController;
    private NPCManager NPCManager;

    private void Awake()
    {
        cctvController = GetComponent<NPCStats>().CCTVManager;
        NPCManager = GetComponent<NPCStats>().NPCManager;
    }

    void Start()
    {
        bubbleRoot.SetActive(false);
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
    }

    public void Hide(NPCStats stats)
    {
        bubbleRoot.SetActive(false);
        NPCManager.ClickedNPC = null;
    }
}