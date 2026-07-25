using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static NPCManager;

public class NPCStats : MonoBehaviour
{
    public NPCManager NPCManager;
    public CCTVManager CCTVManager;

    public string Name;
    public int Number;
    public string Crime;
    public bool Loitering; // placeholder for testing
    public float BumpTimer = 0;
    public GameObject MurderTarget;
    public bool Dead = false;
    public NPCMoods.Moods[] MoodList;
    public TaskType CurrentTask;

    public TaskPriority CurrentPriority => TaskPriorityMap.GetPriority(CurrentTask);

    public HeldItem HeldItem;

    private void Awake()
    {
        NPCManager = FindAnyObjectByType<NPCManager>();
        CCTVManager = FindAnyObjectByType<CCTVManager>();
    }


    private void Start()
    {
        Name = NPCManager.NameArray[Random.Range(0, NPCManager.NameArray.Length)] + " " + NPCManager.NameArray[Random.Range(0, NPCManager.NameArray.Length)];
        Crime = NPCManager.Crimes[Random.Range(0, NPCManager.Crimes.Length)];
        int.TryParse(gameObject.name.Substring(gameObject.name.IndexOf('#') + 1), out Number);
        System.Array.Fill(MoodList, NPCMoods.Moods.Neutral);
        CurrentTask = (TaskType) Random.Range(0, (int)TaskType.None);
        HeldItem = HeldItem.None;     
    }

    public bool TrySetTask(TaskType newTask)
    {
        TaskPriority newPriority = TaskPriorityMap.GetPriority(newTask);
        if (newPriority < CurrentPriority) return false; // blocked, current task outranks it

        CurrentTask = newTask;
        return true;
    }
}