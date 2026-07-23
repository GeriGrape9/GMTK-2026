using UnityEngine;
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
        Bummed
    }

    public Sprite[] sprites;

    public Moods CurrentMood = Moods.Neutral;

    public GameObject NPCBubble;

    public GameObject NPCMood;

    public Animator animator;

    private void Update()
    {
        switch (CurrentMood) 
        { case Moods.Happy:
                animator.SetTrigger("Happy");
                break;

        }
    }

}
