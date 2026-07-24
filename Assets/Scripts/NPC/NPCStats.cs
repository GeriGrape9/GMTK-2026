using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCStats : MonoBehaviour
{
    [SerializeField] private NPCManager manager;

    public string Name;
    public int Number;
    public string Crime;
    public bool Loitering; // placeholder for testing
    public float BumpTimer;
    public NPCMoods.Moods[] MoodList;
    public NPCManager.TaskType CurrentTask;
    public NPCManager.HeldItem HeldItem;

    private void Start()
    {
        Name = manager.NameArray[Random.Range(0, manager.NameArray.Length)] + " " + manager.NameArray[Random.Range(0, manager.NameArray.Length)];
        Crime = manager.Crimes[Random.Range(0, manager.Crimes.Length)];
        Number = Random.Range(1, manager.MaxNPCNumber + 1);
        System.Array.Fill(MoodList, NPCMoods.Moods.Neutral);
        CurrentTask = (NPCManager.TaskType) Random.Range(0, (int)NPCManager.TaskType.None);
        HeldItem = NPCManager.HeldItem.None;
    }
}