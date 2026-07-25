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
            bool success = GetComponent<NavMeshAgent>().SetDestination(TargetNPC.transform.position);
            Debug.Log($"SetDestination: {success}, target: {TargetNPC}");
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
