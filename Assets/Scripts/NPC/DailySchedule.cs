using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DailySchedule", menuName = "CCTV/Daily Schedule")]
public class DailySchedule : ScriptableObject
{
    [System.Serializable]
    public struct ScheduleEntry
    {
        [Range(0, 23)] public int hour;
        public string label;          // "Mess Hall", "Courtyard", "Cell Block" are the options
        public NPCManager.TaskType taskType; // what NPCs actually do
    }

    public List<ScheduleEntry> entries = new List<ScheduleEntry>();

    // Returns the task that's active at a given hour (tasks persist until the next entry)
    public ScheduleEntry GetActiveEntry(int hour)
    {
        ScheduleEntry active = entries[0];
        foreach (var entry in entries)
        {
            if (entry.hour <= hour)
                active = entry;
            else
                break;
        }
        return active;
    }
}