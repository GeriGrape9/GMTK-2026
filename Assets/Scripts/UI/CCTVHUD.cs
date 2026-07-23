using UnityEngine;
using TMPro;

public class CCTVHud : MonoBehaviour
{
    [Header("HUD Elements")]
    [SerializeField] private TMP_Text cameraNumberText;
    [SerializeField] private TMP_Text locationNameText;

    public void SetInfo(CameraData data)
    {
        cameraNumberText.text = "CAM " + data.cameraNumber;
        locationNameText.text = data.locationName;
    }
}