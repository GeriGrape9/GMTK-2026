using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

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
        SearchingForWeapon,
        Loitering,
        None
    }

    public enum TaskPriority
    {
        Scheduled = 0,   // anything set by the daily schedule
        Behavioral = 1,  // loitering
        Critical = 2     // weapon search, alerted, fleeing, etc this stuff doesnt get overwritten by the lower tier tasks
    }


    //this one sorts searchingforweapon as a higher task, and loitering as a behaivoral task
    public static class TaskPriorityMap
    {
        public static TaskPriority GetPriority(NPCManager.TaskType task)
        {
            switch (task)
            {
                case NPCManager.TaskType.SearchingForWeapon:
                    return TaskPriority.Critical;
                case NPCManager.TaskType.Loitering:
                    return TaskPriority.Behavioral;
                default:
                    return TaskPriority.Scheduled;
            }
        }
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

    public GameObject ClickedNPC;

    [SerializeField] private GuardManager GuardManager;

    public void Bump(GameObject NPC1, int NPC2)
    {
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
        if (currentHour == lastAppliedHour) return;

        lastAppliedHour = currentHour;
        var active = schedule.GetActiveEntry(currentHour);

        foreach (GameObject npc in NPCList)
        {
            NPCStats stats = npc.GetComponent<NPCStats>();
            stats?.TrySetTask(active.taskType);
        }
    }
    private void Update()
    {

        LoiteringCheck();
        if (ClickedNPC != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            GameObject closestguard = GuardManager.FindClosestGuard(ClickedNPC.transform.position);
            Debug.Log(closestguard != null ? "sending " + closestguard.name : "not found");
            closestguard.GetComponent<GuardStats>().TargetNPC = ClickedNPC;
        }
        UpdateGlobalTask();
    }


    //use this for after murder is completed.
    public void ClearTask()
    {
        //something like this: CurrentTask = NPCManager.TaskType.None; // will get picked back up on the next schedule tick
    }

}