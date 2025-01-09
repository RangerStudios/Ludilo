using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Changes recentCheckpoint on RespawnManager to specified checkpoint.
// Checkpoint transform set in editor.
// Respawn Manager listens to changeCheckpoint action.
public class ChangeCheckpoints : MonoBehaviour
{
    public static Action<Transform> changeCheckpoint;
    public Transform checkpoint;

    //If player enters trigger, change the checkpoint.
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            changeCheckpoint(checkpoint);
        }
    }
}
