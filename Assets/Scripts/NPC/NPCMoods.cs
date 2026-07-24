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

    [SerializeField] private Moods CurrentMood = Moods.Neutral;

    [SerializeField] private GameObject NPCBubble;

    [SerializeField] private GameObject NPCMood;

    [SerializeField] private Animator moodAnimator;

    [SerializeField] private Animator bubbleAnimator;

    [SerializeField] private CCTVManager CCTVManager;

    private void Update()
    {
        NPCBubble.transform.LookAt(CCTVManager.ActiveCam.transform);
    }

    public void UpdateEmotion(int NPC2)
    {
        bubbleAnimator.SetTrigger("BubblePopup");
        SwitchEmotion(NPC2);
    }

    private void SwitchEmotion(int NPC2)
    {
        Debug.Log("new emotion is " + GetComponent<NPCStats>().MoodList[NPC2]);
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
