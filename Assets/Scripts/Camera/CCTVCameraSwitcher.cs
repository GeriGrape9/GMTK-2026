using UnityEngine;
using UnityEngine.InputSystem;

public class CCTVCameraSwitcher : MonoBehaviour
{
    [SerializeField] private CCTVManager CCTVManager;
    [SerializeField] private CameraNode[] cameraNodes; // all camera props in the scene
    [SerializeField] private LayerMask cameraLayer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] hoverSfx;
    [SerializeField] private LayerMask occlusionMask;
    [SerializeField] private CCTVSwitchPrompt switchPrompt;

    private CameraNode hoveredNode;

    void Awake()
    {
        cameraNodes = FindObjectsByType<CameraNode>(FindObjectsSortMode.None);
    }

    void Update()
    {
        Camera activeCam = CCTVManager.ActiveCam;
        if (activeCam == null) return;

        UpdatePingVisibility(activeCam);
        HandleHover(activeCam);
        HandleInteract();
    }

    private void UpdatePingVisibility(Camera activeCam)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(activeCam);

        foreach (var node in cameraNodes)
        {
            if (node == null || node.ModelRenderer == null) continue;

            if (node.cameraIndex == CCTVManager.CurrentCameraIndex)
            {
                node.HidePing();
                continue;
            }

            bool inFrustum = GeometryUtility.TestPlanesAABB(planes, node.ModelRenderer.bounds);
            bool visible = inFrustum && !IsOccluded(activeCam, node);

            if (visible)
            {
                node.ShowPing();
                node.FaceCamera(activeCam);
            }
            else
            {
                node.HidePing();
            }
        }
    }

    private bool IsOccluded(Camera cam, CameraNode node)
    {
        CameraNode selfNode = cam.GetComponentInParent<CameraNode>();

        Vector3 targetPos = node.ModelRenderer.bounds.center;
        Vector3 dir = targetPos - cam.transform.position;
        float dist = dir.magnitude;

        RaycastHit[] hits = Physics.RaycastAll(cam.transform.position, dir.normalized, dist, occlusionMask);

        foreach (var hit in hits)
        {
            CameraNode hitNode = hit.collider.GetComponentInParent<CameraNode>();
            if (hitNode == node) continue;
            if (hitNode == selfNode) continue;
            return true;
        }

        return false;
    }

    private void HandleHover(Camera activeCam)
    {
        CameraNode selfNode = activeCam.GetComponentInParent<CameraNode>();
        Ray ray = activeCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        CameraNode newHover = null;

        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, cameraLayer);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (var hit in hits)
        {
            CameraNode node = hit.collider.GetComponentInParent<CameraNode>();
            if (node == null || node == selfNode) continue;
            if (!node.gameObject.activeInHierarchy) continue;

            newHover = node;
            break;
        }

        if (newHover != hoveredNode)
        {
            hoveredNode = newHover;

            if (hoveredNode != null)
            {
                PlayRandomSfx(hoverSfx);
                switchPrompt.Show($"[E] View {hoveredNode.LocationName} - Camera {hoveredNode.CameraNumber:D2}");
            }
            else
            {
                switchPrompt.Hide();
            }
        }
    }

    private void HandleInteract()
    {
        if (hoveredNode == null) return;

        bool pressed = Keyboard.current.eKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame;
        if (pressed)
        {
            CCTVManager.SwitchToCamera(hoveredNode.cameraIndex);
            hoveredNode = null;
            switchPrompt.Hide();
        }
    }

    private void PlayRandomSfx(AudioClip[] clips)
    {
        if (clips.Length == 0 || audioSource == null) return;
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}