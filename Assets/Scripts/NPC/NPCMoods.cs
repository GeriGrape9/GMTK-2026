using UnityEngine;
using UnityEngine.InputSystem;
public class NPCMoods : MonoBehaviour
{
    public enum Moods
    {
        None,
        Happy,
        Neutral,
        Angry,
        Scared,
        Evil,
        Sneaky,
        Suprise,
        Question,
        Bummed
    }

    [SerializeField] private Moods CurrentMood = Moods.Neutral;

    [SerializeField] private GameObject NPCBubble;

    [SerializeField] private GameObject NPCMood;

    [SerializeField] private Animator moodAnimator;

    [SerializeField] private Animator bubbleAnimator;

    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            Debug.Log("Mood Test");

            bubbleAnimator.SetTrigger("BubblePopup");
            
            SwitchEmotion();
        }
       
        
        NPCBubble.transform.LookAt(Camera.main.transform);
    }

    private void SwitchEmotion()
    {
        switch (CurrentMood)
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
