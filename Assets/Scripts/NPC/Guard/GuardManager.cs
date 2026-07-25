using UnityEngine;
using UnityEngine.UIElements;

public class GuardManager : MonoBehaviour
{
    public GameObject[] GuardList;

    public GameObject FindClosestGuard(Vector3 NPCPosition)
    {
        float minDistance = Vector3.Distance(GuardList[0].transform.position, NPCPosition);
        GameObject targetGuard = GuardList[0];
        foreach (GameObject Guard in GuardList)
        {
            if (Vector3.Distance(Guard.transform.position, NPCPosition) < minDistance && !Guard.GetComponent<GuardStats>().busy)
            {
                minDistance = Vector3.Distance(Guard.transform.position, NPCPosition);
                targetGuard = Guard;
                Guard.GetComponent<GuardStats>().busy = true;
            }
        }

        return targetGuard;
    }
}
