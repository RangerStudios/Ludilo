using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// MOVE TO GAMEMANAGER
public class RespawnManager : MonoBehaviour
{
    public Transform recentCheckpoint;
    public Transform recentSave;
    public GameObject player;

    void OnEnable()
    {
        GameManager.Instance.SpawnPlayer += OnSpawnPlayer;
    }

    private void OnSpawnPlayer()
    {

        //if(SpawnCondition.RespawnFromDeath
        // RespawnAtCheckpoint)
        throw new NotImplementedException();
    }

    public void Awake()
    {
        //recentCheckpoint = GameObject.FindGameObjectWithTag("Checkpoint").transform;
        //player = GameObject.FindGameObjectWithTag("Player");
        
    }

    public void Update()
    {
        //RespawnAtCheckpoint();
    }

    public void RespawnAtCheckpoint()
    {
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        cc.transform.position = recentCheckpoint.position;
        cc.enabled = true;
    }

    public void RespawnAtSave()
    {
        // Needs save system implemented
    }

    
}
