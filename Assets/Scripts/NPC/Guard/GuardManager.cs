using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GuardManager : MonoBehaviour
{
    public GameObject GuardPrefab;
    public int MaxGuardNumber;
    public List<GameObject> GuardList;

    private void Awake()
    {

    }

    public GameObject FindClosestGuard(Vector3 NPCPosition)
    {
        Debug.Log(GuardList.Count);
        float minDistance = Vector3.Distance(GuardList[0].transform.position, NPCPosition);
        GameObject targetGuard = null;
        foreach (GameObject Guard in GuardList)
        {
            if (Vector3.Distance(Guard.transform.position, NPCPosition) < minDistance && !Guard.GetComponent<GuardStats>().busy)
            {
                minDistance = Vector3.Distance(Guard.transform.position, NPCPosition);
                targetGuard = Guard;
                Guard.GetComponent<GuardStats>().busy = true;
            }
        }

        Debug.Log($"closest guard: {targetGuard}");
        return targetGuard;
    }

    private void Start()
    {
        for (int i = 0; i < MaxGuardNumber; i++)
        {
            GameObject newGuard = Instantiate(GuardPrefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
            GuardList.Add(newGuard);
        }
    }
}


