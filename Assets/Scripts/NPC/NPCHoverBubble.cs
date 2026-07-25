using UnityEngine;

public class NPCHoverBubble : MonoBehaviour
{
    [SerializeField] private GameObject bubbleRoot;       // the world-space canvas object
    [SerializeField] private NPCInfoDisplay infoDisplay;  // lives on the same canvas
    [SerializeField] private CCTVManager cctvController;

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
    }

    public void Hide()
    {
        bubbleRoot.SetActive(false);
    }
}