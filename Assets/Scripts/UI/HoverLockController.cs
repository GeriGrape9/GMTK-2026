using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class HoverLockController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private CCTVManager CCTVManager;
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
    [SerializeField] private AudioClip[] scanSfx;   // played on hover
    [SerializeField] private AudioClip[] lockSfx;   // played on click

    private NPCStats hoveredStats;
    private Transform hoveredTransform;
    private NPCHoverBubble hoveredBubble;
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
    }

    private void HandleHover()
    {
        Camera activeCam = CCTVManager.ActiveCam;
        if (activeCam == null) return;

        Ray ray = activeCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, npcLayer))
        {
            NPCStats stats = hit.collider.GetComponent<NPCStats>();
            if (stats != null)
            {
                if (stats == lockedStats)
                {
                    ClearHover();
                    return;
                }

                if (stats != hoveredStats)
                {
                    ClearHover(); // hide whatever was previously hovered first

                    hoveredStats = stats;
                    hoveredBubble = hit.collider.GetComponent<NPCHoverBubble>();
                    hoveredBubble?.Show(stats);
                    hoverBubble = hoveredBubble.bubbleRoot;
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
            hoveredBubble.Hide();
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
            ClearHover();
        }
    }

    private void PlayRandomSfx(AudioClip[] clips)
    {
        if (clips.Length == 0 || audioSource == null) return;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(clip);
    }
}