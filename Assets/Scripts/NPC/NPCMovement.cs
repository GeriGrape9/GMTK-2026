using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu]
public class NPCMovement
{

    private Vector3 destinationPoint;

    public void Movement(GameObject owner)
    {
        //find destination position by finding plane and camera position and raycasting
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane xzPlane = new Plane(Vector3.up, Vector3.zero);
        //time to actually do tha raycast
        float hitDistance;
        xzPlane.Raycast(camRay, out hitDistance);
        destinationPoint = camRay.GetPoint(hitDistance);

        //move
        owner.GetComponent<NavMeshAgent>().destination = destinationPoint;
    }
}