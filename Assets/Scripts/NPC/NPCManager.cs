using NUnit.Framework.Constraints;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public enum TaskType
    {
        Test,
        None
    }

    public string[] NameArray = 
    {
        "Carter", 
        "Sam", 
        "David",
        "Kasper",
        "Irons",
        "Kane",
        "Testament",
        "Caviera",
        "Willow",
        "Kallisto",
        "Slate",
        "Droizen",
        "Nezeret",
        "Baizen"
    };

    public string[] Crimes =
    {
        "Intergalactic slave trade",
        "Glassing",
        "Meteorite trafficker",
        "Possession of stardust with intent to distrbute",
        "FUI",
        "Space Murder",
        "Planetary hostage",
        "Grand Theft Spaceship",
        "Possession of stolen blaster",
        "Procuring tentacle prostitution"
    };

    public int MaxNPCNumber = 34;

    public enum HeldItem
    {
        Knife,
        Spoon,
        None
    }

    public void Bump(GameObject NPC1, GameObject NPC2)
    {
        NPCStats Stats1 = NPC1.GetComponent<NPCStats>();
        NPCStats Stats2 = NPC2.GetComponent<NPCStats>();
        int Number1 = Stats1.Number; 
        int Number2 = Stats2.Number;

        switch (Random.Range(0, 2)) {
            case 0:
                Stats1.MoodList[Number2] = Stats1.MoodList[Number2] + 1; 
                Stats2.MoodList[Number1] = Stats2.MoodList[Number2] - 1; 
                break;
            case 1:
                Stats1.MoodList[Number2] = Stats1.MoodList[Number2] - 1; 
                Stats2.MoodList[Number1] = Stats2.MoodList[Number2] + 1; 
                break;
            case 2:
                break;
        }
    }
}