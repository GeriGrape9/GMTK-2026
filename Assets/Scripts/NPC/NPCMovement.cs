using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class NPCMovement : MonoBehaviour
{

    private Vector3 destinationPoint;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on this GameObject.");
            enabled = false; // Disable script if no NavMeshAgent
        }
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame) 
        {
            Movement(gameObject);
        }
        NavMeshHit h;
        GetComponent<NavMeshAgent>().SamplePathPosition(NavMesh.AllAreas, 1, out h);
        Debug.Log(h.mask / 8);
    }

    public void Movement(GameObject owner)
    {
        //find destination position by finding plane and camera position and raycasting
        Ray camRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane xzPlane = new Plane(Vector3.up, Vector3.zero);
        //time to actually do tha raycast
        float hitDistance;
        xzPlane.Raycast(camRay, out hitDistance);
        destinationPoint = camRay.GetPoint(hitDistance);

        //move
        owner.GetComponent<NavMeshAgent>().destination = destinationPoint;
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
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
}