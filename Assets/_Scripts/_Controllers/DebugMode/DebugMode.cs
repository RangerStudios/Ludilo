using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour
{
    public bool debugOn = false;
    
    GameObject player;
    DebugPlayerController debugController;
    PlayerController playerController;
    
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        debugController = player.GetComponent<DebugPlayerController>();
        playerController = player.GetComponent<PlayerController>();
    }

    public void SwitchDebugMode()
    {
        debugOn = !debugOn;

        if(debugOn)
        {
            //Debug Turns On
            debugController.enabled = true;
            playerController.enabled = false;

            //Debug.Log("On");
        }
        else
        {
            //Debug Turns Off
            debugController.enabled = false;
            playerController.enabled = true;

            //Debug.Log("Off");
        }
    }


}
