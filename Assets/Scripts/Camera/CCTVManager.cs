using UnityEngine;
using UnityEngine.InputSystem;

public class CCTVController : MonoBehaviour
{
    [Header("CCTV Settings")]
    [SerializeField] private GameObject[] cctvCameras; // Array to hold your CCTV cameras
    private int currentCameraIndex = 0;

    void Start()
    {
        // Ensure all cameras are off at the start, and then enable the first one
        DisableAllCameras();
        if (cctvCameras.Length > 0)
        {
            cctvCameras[0].SetActive(true);
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
        if (cctvCameras.Length == 0) return;

        // Disable current camera
        cctvCameras[currentCameraIndex].SetActive(false);

        // Advance to next index (loops back to 0 if at the end)
        currentCameraIndex = (currentCameraIndex + 1) % cctvCameras.Length;

        // Enable new camera
        cctvCameras[currentCameraIndex].SetActive(true);
    }
    public void CycleCameraBack()
    {
        if (cctvCameras.Length == 0) return;

        // Disable current camera
        cctvCameras[currentCameraIndex].SetActive(false);

        currentCameraIndex = (currentCameraIndex - 1 + cctvCameras.Length) % cctvCameras.Length;

        // Enable new camera
        cctvCameras[currentCameraIndex].SetActive(true);
    }

    private void DisableAllCameras()
    {
        foreach (GameObject cam in cctvCameras)
        {
            if (cam != null)
            {
                cam.SetActive(false);
            }
        }
    }
}