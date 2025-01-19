using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour
{
    public bool debugOn = false;
    public bool noClipOn = false;
    
    GameObject player;
    GameObject gameManager;
    [SerializeField] GameObject debugCam;
    DebugPlayerController debugController;
    PlayerController playerController;
    DebugCheckpointSpawn debugCheckpointSpawn;
    
    private void Awake()
    {
        PlayerInput.onSelect += NoClipToggle;
        player = GameObject.FindWithTag("Player");
        gameManager = GameObject.FindWithTag("GameController");
        debugController = player.GetComponent<DebugPlayerController>();
        playerController = player.GetComponent<PlayerController>();
        debugCam = GameObject.FindWithTag("NoClipCamera");
        debugCam.SetActive(false);
        debugCheckpointSpawn = gameManager.GetComponent<DebugCheckpointSpawn>();
    }

    public void SwitchDebugMode()
    {
        debugOn = !debugOn;

        if(debugOn)
        {
            //Debug Turns On
            debugCheckpointSpawn.enabled = true;
            //Debug.Log("On");
        }
        else
        {
            //Debug Turns Off
            debugCheckpointSpawn.enabled = false;
            //Debug.Log("Off");
        }
    }

    public void NoClipToggle()
    {
        if(debugOn)
        {
            noClipOn = !noClipOn;

            if(noClipOn)
            {
                debugController.enabled = true;
                debugCam.SetActive(true);
                playerController.enabled = false;
            }
            else
            {
                debugController.enabled = false;
                debugCam.SetActive(false);
                playerController.enabled = true;
            }
        }
    }


}
