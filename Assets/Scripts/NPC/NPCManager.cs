using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using ColorUtility = UnityEngine.ColorUtility;

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

    public GameObject NPCPrefab;

    public int MaxNPCNumber;

    public List<GameObject> NPCList;

    public int AliveNPCs;

    public GameObject ClickedNPC;

    [SerializeField] private GuardManager GuardManager;

    private string hexColor = "#FF0B00";

    public void Bump(GameObject NPC1, GameObject NPC2)
    {
        NPCStats Stats1 = NPC1.GetComponent<NPCStats>();
        int Number2 = NPC2.GetComponent<NPCStats>().Number;

        switch (Random.Range(0, 2)) {
            case 0:
                if (Stats1.MoodList[Number2] != NPCMoods.Moods.Evil)
                    Stats1.MoodList[Number2] = Stats1.MoodList[Number2] + 1;
                if (Stats1.MoodList[Number2] == NPCMoods.Moods.Evil)
                {
                    Stats1.MurderTarget = NPC2;
                    Stats1.Loitering = false;
                    Debug.Log("#" + Stats1.Number + " wants to shank #" + Number2);
                }
                break;
            case 1:
                if (Stats1.MoodList[Number2] != NPCMoods.Moods.Happy)
                    Stats1.MoodList[Number2] = Stats1.MoodList[Number2] - 1; 
                break;
            case 2:
                break;
        }
    }

    public void LoiteringCheck()
    {
        foreach ( GameObject NPC in NPCList)
        {
            if ((NPC.GetComponent<NPCStats>().Loitering && !NPC.GetComponent<NPCStats>().Dead) && (!NPC.GetComponent<NavMeshAgent>().hasPath || NPC.GetComponent<NavMeshAgent>().remainingDistance < 2))
            {
                bool success = NPC.GetComponent<NavMeshAgent>().SetDestination(NPC.GetComponent<NPCMovement>().GetRandomPoint());
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, 160.0f);
    }

    public void Kill(GameObject NPC)
    {
        NPC.GetComponent<NPCStats>().Dead = true;
        NPC.GetComponent<NavMeshAgent>().ResetPath();
        ColorUtility.TryParseHtmlString(hexColor, out Color newColor);
        NPC.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = newColor;
        AliveNPCs--;
    }

    private void Start()
    {
        AliveNPCs = MaxNPCNumber;
        for (int i = 1; i < MaxNPCNumber + 1; i++)
        {
            GameObject newNPC = Instantiate(NPCPrefab, Vector3.zero, Quaternion.identity);
            newNPC.transform.position = newNPC.GetComponent<NPCMovement>().GetRandomPoint();
            newNPC.name = "NPC #" + i;
            NPCList.Add(newNPC);
        }
    }

    private void Update()
    {

        LoiteringCheck();
        if (ClickedNPC != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            //Debug.Log("start sending guard");
            GameObject closestguard = GuardManager.FindClosestGuard(ClickedNPC.transform.position);
            //Debug.Log(closestguard != null ? "sending " + closestguard.name : "not found");
            if (closestguard != null)
                closestguard.GetComponent<GuardStats>().TargetNPC = ClickedNPC;
            else
                Debug.Log("no more guards!");
        }
        UpdateGlobalTask();

        if (AliveNPCs == 0)
        {
            SceneManager.LoadScene(2);
        }
    }


    //use this for after murder is completed.
    public void ClearTask()
    {
        //something like this: CurrentTask = NPCManager.TaskType.None; // will get picked back up on the next schedule tick
    }

}