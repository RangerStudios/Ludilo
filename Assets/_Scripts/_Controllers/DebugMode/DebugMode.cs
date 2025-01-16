using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour
{
    public bool debugOn = false;
    
    GameObject player;
    GameObject gameManager;
    DebugPlayerController debugController;
    PlayerController playerController;
    DebugCheckpointSpawn debugCheckpointSpawn;
    
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        gameManager = GameObject.FindWithTag("GameController");
        debugController = player.GetComponent<DebugPlayerController>();
        playerController = player.GetComponent<PlayerController>();
        debugCheckpointSpawn = gameManager.GetComponent<DebugCheckpointSpawn>();
    }

    public void SwitchDebugMode()
    {
        debugOn = !debugOn;

        if(debugOn)
        {
            //Debug Turns On
            debugController.enabled = true;
            debugCheckpointSpawn.enabled = true;
            //Debug.Log("On");
        }
        else
        {
            //Debug Turns Off
            debugController.enabled = false;
            debugCheckpointSpawn.enabled = false;
            //Debug.Log("Off");
        }
    }


}
