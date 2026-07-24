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

    public void InitiateBump()
    {

    }

    public void Bump(GameObject NPC1, int NPC2)
    {
        NPCStats Stats1 = NPC1.GetComponent<NPCStats>();

        switch (Random.Range(0, 2)) {
            case 0:
                Stats1.MoodList[NPC2] = Stats1.MoodList[NPC2] + 1; 
                break;
            case 1:
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

    private void Update()
    {
        LoiteringCheck();
    }
}