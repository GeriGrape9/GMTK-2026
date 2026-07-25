using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GuardManager : MonoBehaviour
{
    public GameObject GuardPrefab;
    public int MaxGuardNumber;
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

    private void Start()
    {
        for (int i = 0; i < MaxGuardNumber; i++)
        {
            GuardList.Append(Instantiate(GuardPrefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity));
        }
    }
}


