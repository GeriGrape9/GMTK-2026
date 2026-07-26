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
    private CCTVManager CCTVManager;
    private NPCManager NPCManager;
    private NPCStats stats;
    private bool murderPathFound;
    public float GetCurrentAreaIndex()
    {
        GetComponent<NavMeshAgent>().SamplePathPosition(NavMesh.AllAreas, 1, out NavMeshHit h);
        return Mathf.Log(h.mask, 2.0f);
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

    public GameObject FindClosestWeapon()
    {
        GameObject[] WeaponList = GameObject.FindGameObjectsWithTag("Weapon");
        
        if (WeaponList.Length == 0)
            return null;

        GameObject targetWeapon = WeaponList[0];
        float minDistance = Vector3.Distance(targetWeapon.transform.position, transform.position);
        foreach (GameObject curWeapon in WeaponList)
        {
            float curDistance = Vector3.Distance(curWeapon.transform.position, transform.position);
            if (curDistance < minDistance)
            {
                minDistance = curDistance;
                targetWeapon = curWeapon;
            }
        }
        return targetWeapon;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CompareTag(collision.gameObject.tag) && 
            CompareTag("NPC") && 
            !collision.gameObject.GetComponent<NPCStats>().Dead)
        {
            if (stats.MurderTarget == null)
            {
                GameObject otherNPC = collision.gameObject;
                if (stats.BumpTimer == 0 && otherNPC.GetComponent<NPCStats>().BumpTimer == 0)
                {
                    NPCManager.Bump(gameObject, otherNPC);
                    GetComponent<NPCMoods>().UpdateEmotion(otherNPC.GetComponent<NPCStats>().Number);

                    NPCManager.Bump(otherNPC, gameObject);
                    otherNPC.GetComponent<NPCMoods>().UpdateEmotion(stats.Number);
                }
                stats.BumpTimer = 3.0f;
                otherNPC.GetComponent<NPCStats>().BumpTimer = 3.0f;
            } else
            {
                if (collision.gameObject == stats.MurderTarget && stats.HeldItem == NPCManager.HeldItem.Knife)
                {
                    NPCManager.Kill(collision.gameObject);
                    stats.HeldItem = NPCManager.HeldItem.None;
                    stats.MoodList[collision.gameObject.GetComponent<NPCStats>().Number] = NPCMoods.Moods.None;
                    collision.gameObject.GetComponent<BoxCollider>().enabled = false;
                }
            }
            
        }

        if (collision.gameObject.CompareTag("Weapon") && stats.MurderTarget != null)
        {
            stats.HeldItem = NPCManager.HeldItem.Knife;
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Debug.Log("OnDrawGizmosSelected called");

        if (agent == null)
        {
            Debug.Log("Agent is null");
            return;
        }

        if (agent.path == null)
        {
            Debug.Log("Path is null");
            return;
        }

        Debug.Log(
            $"HasPath: {agent.hasPath}, " +
            $"PathStatus: {agent.pathStatus}, " +
            $"Pending: {agent.pathPending}, " +
            $"Remaining: {agent.remainingDistance}, " +
            $"Corners: {agent.path.corners.Length}"
        );

        Gizmos.color = Color.cyan;

        Vector3[] corners = agent.path.corners;

        for (int i = 0; i < corners.Length - 1; i++)
        {
            Gizmos.DrawLine(corners[i], corners[i + 1]);
            Gizmos.DrawSphere(corners[i], 0.1f);
        }

        if (corners.Length > 0)
            Gizmos.DrawSphere(corners[corners.Length - 1], 0.1f);

        Gizmos.DrawSphere(agent.destination, 5.0f);
    }

    private void Start()
    {
        prevAreaIndex = GetCurrentAreaIndex();
    }

    void Awake()
    {
        stats = GetComponent<NPCStats>();
        CCTVManager = stats.CCTVManager;
        NPCManager = stats.NPCManager;
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on this GameObject.");
            enabled = false; // Disable script if no NavMeshAgent
        }
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

        if (stats.MurderTarget != null)
        {
            if (!murderPathFound)
            {
                Vector3 target = stats.HeldItem != NPCManager.HeldItem.Knife
                ? FindClosestWeapon().transform.position
                : stats.MurderTarget.transform.position;

                bool success = agent.SetDestination(target);
                murderPathFound = true;
            }
            else
            {
                if (stats.HeldItem == NPCManager.HeldItem.Knife)
                {
                    murderPathFound = false;
                }
            }
        }

        if (prevAreaIndex != newIndex)
        {
            prevAreaIndex = newIndex;
            //Debug.Log("Entering area: #" +  newIndex);
        }
    }
}