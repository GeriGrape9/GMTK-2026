using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class NPCMovement : MonoBehaviour
{

    private Vector3 destinationPoint;
    private static float prevAreaIndex;
    private NavMeshAgent agent;
    [SerializeField] private CCTVManager CCTVManager;
    [SerializeField] private NPCManager NPCManager;

    private NPCStats stats;
    public float GetCurrentAreaIndex()
    {
        GetComponent<NavMeshAgent>().SamplePathPosition(NavMesh.AllAreas, 1, out NavMeshHit h);
        return Mathf.Log(h.mask, 2.0f);
    }

    public void Movement(GameObject owner)
    {
        //find destination position by finding plane and camera position and raycasting
        Ray camRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane xzPlane = new Plane(Vector3.up, Vector3.zero);
        //time to actually do tha raycast
        xzPlane.Raycast(camRay, out float hitDistance);
        destinationPoint = camRay.GetPoint(hitDistance);

        //move
        owner.GetComponent<NavMeshAgent>().destination = destinationPoint;
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, GetComponent<NavMeshAgent>().areaMask))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public bool IsMovingTowardsDestination()
    {
        return agent.hasPath &&
               !agent.pathPending &&
               agent.remainingDistance > agent.stoppingDistance &&
               agent.velocity.sqrMagnitude > 0.1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CompareTag(collision.gameObject.tag))
        {
            GameObject otherNPC = collision.gameObject;
            if (stats.BumpTimer == 0 && otherNPC.GetComponent<NPCStats>().BumpTimer == 0) 
            {
                Debug.Log("collided with #" + otherNPC.GetComponent<NPCStats>().Number);
                NPCManager.Bump(gameObject, otherNPC.GetComponent<NPCStats>().Number);
                GetComponent<NPCMoods>().UpdateEmotion(otherNPC.GetComponent<NPCStats>().Number);

                NPCManager.Bump(otherNPC, stats.Number);
                otherNPC.GetComponent<NPCMoods>().UpdateEmotion(stats.Number);
            }
            stats.BumpTimer = 3.0f;
            otherNPC.GetComponent<NPCStats>().BumpTimer = 3.0f;
        }
    }

    void Start()
    {
        stats = GetComponent<NPCStats>();
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on this GameObject.");
            enabled = false; // Disable script if no NavMeshAgent
        }

        prevAreaIndex = GetCurrentAreaIndex();
    }

    void Update()
    {
        float newIndex = GetCurrentAreaIndex();
        if (stats.BumpTimer > 0) 
        {
            stats.BumpTimer -= Time.deltaTime;
        } else
        {
            stats.BumpTimer = 0;
        }

        if (prevAreaIndex != newIndex)
        {
            prevAreaIndex = newIndex;
            //Debug.Log("Entering area: #" +  newIndex);
        }
    }
}