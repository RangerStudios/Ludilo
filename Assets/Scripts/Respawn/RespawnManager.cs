using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Handles the respawning of the player.
public class RespawnManager : MonoBehaviour
{
    public Transform recentCheckpoint;
    public Transform recentSave;

    public Transform[] allCheckpoints; 
    public GameObject player;

    void OnEnable()
    {
        GameManager.SpawnPlayer += OnSpawnPlayer;
        ChangeCheckpoints.changeCheckpoint += OnChangeCheckpoint;
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        allCheckpoints = new Transform[transform.childCount];
        for (int i = 0; i < allCheckpoints.Length; i++)
        {
            allCheckpoints[i] = transform.GetChild(i);
        }
    }

    public void OnChangeCheckpoint(Transform newCheckpoint)
    {
        recentCheckpoint = newCheckpoint;
    }

    private void OnSpawnPlayer(RespawnCondition respawnInstructions)
    {
        switch (respawnInstructions)
        {
            case RespawnCondition.GAMESTART:
                RespawnAtCheckpoint(allCheckpoints[0]);
                break;
            case RespawnCondition.FALL:
                RespawnAtCheckpoint(recentCheckpoint);
                break;
            case RespawnCondition.DEATH:
                RespawnAtSave();
                break;
            default:
                Debug.Log("No Conditions Received, respawning at recent checkpoint.");
                break;
        };
    }

    public void RespawnAtCheckpoint(Transform checkpoint)
    {
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        cc.transform.position = checkpoint.position;
        cc.enabled = true;
    }

    public void RespawnAtSave()
    {
        if (recentSave == null)
        {
            player.SetActive(true);
            RespawnAtCheckpoint(allCheckpoints[0]);
        }

        // Needs further functionality for respawn at save point.
    }

    
}
