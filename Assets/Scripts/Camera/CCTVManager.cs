using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class CCTVController : MonoBehaviour
{
    [Header("CCTV Settings")]
    [SerializeField] private GameObject[] cctvCameras;
    private int currentCameraIndex = 0;
    private CameraData[] camDataArray;

    [Header("HUD")]
    [SerializeField] private CCTVHud HUDRef;

    void Start()
    {
        camDataArray = new CameraData[cctvCameras.Length];
        for (int i = 0; i < cctvCameras.Length; i++)
        {
            if (cctvCameras[i] != null)
            {
                camDataArray[i] = cctvCameras[i].GetComponent<CameraData>();
                if (camDataArray[i] != null)
                    camDataArray[i].cameraNumber = i;
            }
        }

        DisableAllCameras();
        if (cctvCameras.Length > 0)
        {
            cctvCameras[0].SetActive(true);
            UpdateHUD(0);
        }
    }
    void Update()
    {
        // Cycle cameras when the designated key is pressed
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            CycleCameraForward();
        }
        // Cycle cameras when the designated key is pressed
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            CycleCameraBack();
        }
    }

    public void CycleCameraForward()
    {
        if (cctvCameras.Length == 0)
            return;
        cctvCameras[currentCameraIndex].SetActive(false);

        currentCameraIndex = (currentCameraIndex + 1) % cctvCameras.Length;
        cctvCameras[currentCameraIndex].SetActive(true);
        UpdateHUD(currentCameraIndex);
    }

    public void CycleCameraBack()
    {
        if (cctvCameras.Length == 0)
            return;
        cctvCameras[currentCameraIndex].SetActive(false);
        currentCameraIndex = (currentCameraIndex - 1 + cctvCameras.Length) % cctvCameras.Length;
        cctvCameras[currentCameraIndex].SetActive(true);
        UpdateHUD(currentCameraIndex);
    }

    private void UpdateHUD(int index)
    {
        CameraData data = camDataArray[index];
        if (data == null || HUDRef == null) return;

        HUDRef.SetInfo(data);
    }

    private void DisableAllCameras()
    {
        foreach (GameObject cam in cctvCameras)
        {
            if (cam != null) cam.SetActive(false);
        }
    }
}