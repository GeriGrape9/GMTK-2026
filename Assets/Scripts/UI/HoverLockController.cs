using UnityEngine;
using UnityEngine.InputSystem;

public class HoverLockController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask npcLayer;

    [Header("Hover Bubble (world-space, follows NPC)")]
    [SerializeField] private GameObject hoverBubble;
    [SerializeField] private NPCInfoDisplay hoverDisplay;
    [SerializeField] private Vector3 bubbleWorldOffset = new Vector3(0, 2f, 0);

    [Header("Locked Panel (fixed HUD position)")]
    [SerializeField] private GameObject lockedPanel;
    [SerializeField] private NPCInfoDisplay lockedDisplay;

    [Header("SFX")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] scanSfx;   // played on hover-start
    [SerializeField] private AudioClip[] lockSfx;   // played on click-lock

    private NPCStats hoveredStats;
    private Transform hoveredTransform;
    private NPCStats lockedStats;

    void Start()
    {
        hoverBubble.SetActive(false);
        lockedPanel.SetActive(false);
    }

    void Update()
    {
        HandleHover();
        HandleClick();

        if (hoverBubble.activeSelf && hoveredTransform != null)
        {
            PositionBubbleOnScreen();
        }
    }

    private void HandleHover()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, npcLayer))
        {
            NPCStats stats = hit.collider.GetComponent<NPCStats>();
            if (stats != null)
            {
                // don't show the hover bubble for whichever NPC is currently locked
                if (stats == lockedStats)
                {
                    ClearHover();
                    return;
                }

                if (stats != hoveredStats)
                {
                    hoveredStats = stats;
                    hoveredTransform = hit.collider.transform;
                    hoverDisplay.SetData(stats);
                    hoverBubble.SetActive(true);
                    PlayRandomSfx(scanSfx);
                }
                return;
            }
        }

        ClearHover();
    }

    private void ClearHover()
    {
        if (hoveredStats != null)
        {
            hoveredStats = null;
            hoveredTransform = null;
            hoverBubble.SetActive(false);
        }
    }

    private void HandleClick()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        if (hoveredStats != null)
        {
            // Lock onto the currently hovered NPC
            lockedStats = hoveredStats;
            lockedDisplay.SetData(lockedStats);
            lockedPanel.SetActive(true);
            hoverBubble.SetActive(false);
            PlayRandomSfx(lockSfx);
        }
        else
        {
            // Clicked empty space to unlock
            lockedStats = null;
            lockedPanel.SetActive(false);
        }
    }

    private void PositionBubbleOnScreen()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(hoveredTransform.position + bubbleWorldOffset);

        if (screenPos.z < 0)
        {
            hoverBubble.SetActive(false);
            return;
        }

        hoverBubble.transform.position = screenPos;
    }

    private void PlayRandomSfx(AudioClip[] clips)
    {
        if (clips.Length == 0 || audioSource == null) return;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(clip);
    }
}