using UnityEngine;
using UnityEngine.AI;

public class NPCManager : MonoBehaviour
{
    public enum TaskType
    {
        Test,
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

    public void Bump(GameObject NPC1, GameObject NPC2)
    {
        NPCStats Stats1 = NPC1.GetComponent<NPCStats>();
        NPCStats Stats2 = NPC2.GetComponent<NPCStats>();
        int Number1 = Stats1.Number; 
        int Number2 = Stats2.Number;

        switch (Random.Range(0, 2)) {
            case 0:
                Stats1.MoodList[Number2] = Stats1.MoodList[Number2] + 1; 
                Stats2.MoodList[Number1] = Stats2.MoodList[Number2] - 1; 
                break;
            case 1:
                Stats1.MoodList[Number2] = Stats1.MoodList[Number2] - 1; 
                Stats2.MoodList[Number1] = Stats2.MoodList[Number2] + 1; 
                break;
            case 2:
                break;
        }
    }

    public void LoiteringCheck()
    {
        foreach ( GameObject NPC in NPCList)
        {
            if (NPC.GetComponent<NPCStats>().loitering && !NPC.GetComponent<NPCMovement>().IsMovingTowardsDestination())
            {
                NPC.GetComponent<NavMeshAgent>().SetDestination(NPC.GetComponent<NPCMovement>().RandomNavmeshLocation(4f));
            }
        }
    }

    private void Update()
    {
        LoiteringCheck();
    }
}