using UnityEngine;

public class CameraData : MonoBehaviour
{
    [Header("Camera Identification")]
    public int cameraNumber = 1;
    public string locationName;

    [Header("Position Data")]
    public Vector3 currentCoordinates;

    private void Update()
    {
        
        currentCoordinates = transform.position;
    }

    
    public string GetCameraInfo()
    {
        return $"Cam {cameraNumber} [{locationName}] - Pos: {currentCoordinates}";
    }
}
