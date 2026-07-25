using UnityEngine;
using UnityEngine.AI;

public class GuardStats : MonoBehaviour
{
    public GameObject TargetNPC;
    public bool busy = false;

    private void Update()
    {
        if (TargetNPC != null)
        {
            if (GetComponent<NPCStats>().Loitering == true)
                GetComponent<NPCStats>().Loitering = false;
            GetComponent<NavMeshAgent>().SetDestination(TargetNPC.transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == TargetNPC) 
        { 
            // apply logic
            TargetNPC = null;
            busy = false;
        }
    }
}
