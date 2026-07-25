using UnityEngine;

public class CameraNode : MonoBehaviour
{
    public int cameraIndex;

    [SerializeField] private GameObject pingIconRoot;
    [SerializeField] private Renderer modelRenderer;
    private CameraData camData;

    public Renderer ModelRenderer => modelRenderer;
    public int CameraNumber => camData != null ? camData.cameraNumber : cameraIndex;
    public string LocationName => camData != null ? camData.locationName : "";

    void Awake()
    {
        camData = GetComponent<CameraData>();
    }
    void Start()
    {
        pingIconRoot.SetActive(false);
    }

    public void ShowPing() => pingIconRoot.SetActive(true);
    public void HidePing() => pingIconRoot.SetActive(false);

    public void FaceCamera(Camera cam)
    {
        Vector3 dir = pingIconRoot.transform.position - cam.transform.position;
        pingIconRoot.transform.rotation = Quaternion.LookRotation(dir);
    }
}