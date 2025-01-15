using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCheckpointSpawn : MonoBehaviour
{
    RespawnManager respawnManager;

    //Gets the respawn manager at the start of the game.
    void Start()
    {
        GetRespawnManager();
    }

    //Checks for input from each number on the keyboard. Respawns at corresponding checkpoint.
    // Note: corresponding checkpoint is actual number of checkpoint, and not index.
    // Example: 1 on keyboard corresponds to index 0 in allCheckpoints, the first checkpoint.
    // Note 2: 0 is 10.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnAtSetCheckpoint(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnAtSetCheckpoint(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnAtSetCheckpoint(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SpawnAtSetCheckpoint(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SpawnAtSetCheckpoint(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SpawnAtSetCheckpoint(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SpawnAtSetCheckpoint(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SpawnAtSetCheckpoint(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SpawnAtSetCheckpoint(8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SpawnAtSetCheckpoint(9);
        }
    }    

    //Gets the current stage's respawn manager, and assigns it to the respawnManager variable.
    public void GetRespawnManager()
    {
        respawnManager = GameObject.Find("RespawnManager").GetComponent<RespawnManager>();

        if (respawnManager != null)
        {
            Debug.Log("Found Respawn Manager");
        }
    }

    //Spawns the player at a specified checkpoint. Does not set current checkpoint.
    public void SpawnAtSetCheckpoint(int checkpointIndex)
    {
        if (checkpointIndex >= respawnManager.allCheckpoints.Length)
        {
            Debug.Log("Number is not assigned to available checkpoint");
        }
        else
        {
            respawnManager.RespawnAtCheckpoint(respawnManager.allCheckpoints[checkpointIndex]);
        }
    }
}
