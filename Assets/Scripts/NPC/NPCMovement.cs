using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class NPCMovement : MonoBehaviour
{

    private Vector3 destinationPoint;
    private static float prevAreaIndex;
    private static bool wasClicked;
    private NavMeshAgent agent;

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
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, 1))
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

    void Start()
    {
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
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log(gameObject.name);
                    if (!wasClicked)
                    {
                        wasClicked = true;
                        // apply highlight
                    }
                } else if (wasClicked)
                {
                    wasClicked = false;
                    Movement(gameObject);
                }
            }
        }              

        float newIndex = GetCurrentAreaIndex();
        if (prevAreaIndex != newIndex)
        {
            prevAreaIndex = newIndex;
            Debug.Log("Entering area: #" +  newIndex);
        }
    }
}