using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class NPCMoods : MonoBehaviour
{
    public enum Moods
    {
        Happy,
        Neutral,
        Angry,
        Scared,
        Evil,
        Sneaky,
        Suprise,
        Question,
        Bummed,
        None
    }

    [SerializeField] private GameObject NPCBubble;

    [SerializeField] private GameObject NPCMood;

    [SerializeField] private Animator moodAnimator;

    [SerializeField] private Animator bubbleAnimator;

    public Sprite[] MoodIconList;

    private CCTVManager CCTVManager;

    private void Awake()
    {
        CCTVManager = GetComponent<NPCStats>().CCTVManager;
    }

    private void Update()
    {
        if (NPCBubble.activeSelf && CCTVManager != null && CCTVManager.ActiveCam != null)
        {
            Vector3 directionFromCamera = NPCBubble.transform.position - CCTVManager.ActiveCam.transform.position;
            NPCBubble.transform.rotation = Quaternion.LookRotation(directionFromCamera);
        }
    }

    public void UpdateEmotion(int NPC2)
    {
        bubbleAnimator.SetTrigger("BubblePopup");
        SwitchEmotion(NPC2);
    }

    public Moods FindHighestMood()
    {
        Moods[] moodList = GetComponent<NPCStats>().MoodList;
        Moods HighestMood = moodList[0];
        foreach (Moods mood in moodList)
        {
            if (mood == Moods.Evil)
                return Moods.Evil;

            if ((int) mood > (int) HighestMood)
            {
                HighestMood = mood;
            }
        }
        return HighestMood;
    }

    private void SwitchEmotion(int NPC2)
    {
        switch (GetComponent<NPCStats>().MoodList[NPC2])
        {
            case Moods.None:
                break;
            case Moods.Happy:
                moodAnimator.SetTrigger("Happy");
                break;
            case Moods.Neutral:
                moodAnimator.SetTrigger("Neutral");
                break;
            case Moods.Angry:
                moodAnimator.SetTrigger("Angry");
                break;
            case Moods.Sneaky:
                moodAnimator.SetTrigger("Sneaky");
                break;
            case Moods.Suprise:
                moodAnimator.SetTrigger("Suprised");
                break;
            case Moods.Evil:
                moodAnimator.SetTrigger("Evil");
                break;
            case Moods.Question:
                moodAnimator.SetTrigger("Question");
                break;
            case Moods.Scared:
                moodAnimator.SetTrigger("Scared");
                break;
            case Moods.Bummed:
                moodAnimator.SetTrigger("Bummed");
                break;
        }
    }

}
