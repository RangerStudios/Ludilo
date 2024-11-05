using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public List<string> publicEventList;
    // Last activated event.
    public string mostRecentEvent;
    // List of events in the level - instarted into the levelEvents hashset at start.
    public string[] levelEventArray;
    // List of previously activated events in the level. Used for saving progress.
    public string[] previousLevelEventArray;
    // Hashset containing all level events.
    public HashSet<string> levelEvents = new HashSet<string>();
    // Hashset containing all previously activated events. Activated events from levelEvents get added here.
    public HashSet<string> previousLevelEvents = new HashSet<string>();
    // Hashset containing all remaining events.
    public HashSet<string> remainingLevelEvents = new HashSet<string>();

    void Start()
    {
        CreateLevelHashSet();
    }

    // Creates the hashset of strings for elevel events based on the string array eventList.
    public void CreateLevelHashSet()
    {
        if (publicEventList.Count == 0)
        {
            Debug.LogError("Empty Event List!");
            return;
        }
        else
        {
            foreach (string eventName in publicEventList)
            {
                levelEvents.Add(eventName);
            }
        }
    }

    public void TriggerLevelEvent(string levelEventToTrigger)
    {
        if(previousLevelEvents.Contains(levelEventToTrigger))
        {
            //Event has already happened
            return;
        }
        
    }


}
