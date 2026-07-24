using UnityEngine;
using UnityEngine.AI;

public class NPCManager : MonoBehaviour
{
    [Header("Schedule")]
    [SerializeField] private DailySchedule schedule;
    private int lastAppliedHour = -1;
    public enum TaskType
    {
        Idle,
        MessHall,
        Courtyard,
        FreeTime,
        CellBlocks,
        Cells,
        None
    }

    public enum HeldItem
    {
        Knife,
        Spoon,
        None
    }

    public string[] NameArray;

    public string[] Crimes;

    public int MaxNPCNumber;

    public GameObject[] NPCList;

    public void Bump(GameObject NPC1, int NPC2)
    {
        Debug.Log("bump started");
        NPCStats Stats1 = NPC1.GetComponent<NPCStats>();

        switch (Random.Range(0, 2)) {
            case 0:
                if (Stats1.MoodList[NPC2] != NPCMoods.Moods.Evil)
                    Stats1.MoodList[NPC2] = Stats1.MoodList[NPC2] + 1; 
                break;
            case 1:
                if (Stats1.MoodList[NPC2] != NPCMoods.Moods.Happy)
                    Stats1.MoodList[NPC2] = Stats1.MoodList[NPC2] - 1; 
                break;
            case 2:
                break;
        }
        Debug.Log("NPC2 mood is " + Stats1.MoodList[NPC2]);
        Debug.Log("bump finished");
    }

    public void LoiteringCheck()
    {
        foreach ( GameObject NPC in NPCList)
        {
            if (NPC.GetComponent<NPCStats>().Loitering && !NPC.GetComponent<NPCMovement>().IsMovingTowardsDestination())
            {
                NPC.GetComponent<NavMeshAgent>().SetDestination(NPC.GetComponent<NPCMovement>().RandomNavmeshLocation(4f));
            }
        }
    }
    private void UpdateGlobalTask()
    {
        if (schedule == null || GameClock.Instance == null) return;

        int currentHour = GameClock.Instance.CurrentTimeOfDay.Hours;
        if (currentHour == lastAppliedHour) return; // only recalc on the hour boundary

        lastAppliedHour = currentHour;
        var active = schedule.GetActiveEntry(currentHour);

        foreach (GameObject npc in NPCList)
        {
            NPCStats stats = npc.GetComponent<NPCStats>();
            if (stats != null)
                stats.CurrentTask = active.taskType;
        }
    }
    private void Update()
    {

        LoiteringCheck();
        UpdateGlobalTask();
    }


}