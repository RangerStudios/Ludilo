using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public string lastEvent;
    public string[] eventList;
    [SerializeField]public HashSet<string> levelEvents = new HashSet<string>();

    void Start()
    {
        CreateLevelHashSet();
    }

    public void CreateLevelHashSet()
    {
        if (eventList.Length == 0)
        {
            Debug.Log("Empty Event List!");
            return;
        }
        else
        {
            foreach (string eventName in eventList)
            {
                levelEvents.Add(eventName);
            }
        }
    }
}
