using UnityEngine;
using TMPro;

public class CCTVHud : MonoBehaviour
{
    [Header("HUD Elements")]
    [SerializeField] private TMP_Text cameraNumberText;
    [SerializeField] private TMP_Text locationNameText;
    [SerializeField] private TMP_Text locationNameandNumberText;
    public void SetInfo(CameraData data)
    {
        cameraNumberText.text = "CAM " + data.cameraNumber;
        locationNameText.text = data.locationName;
        locationNameandNumberText.text = data.locationName + System.Environment.NewLine + "CAM " + data.cameraNumber;
    }
}