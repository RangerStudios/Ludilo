using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlayerController : MonoBehaviour
{

    GameObject menuUI;
    GameObject player;

    void Awake()
    {
         menuUI = GameObject.Find("Canvas");
    }

    public void DisableBaseController()
    {
        //Disabling basic player controller, enable movement logic in this script

    }

    public void EnableBaseController()
    {
        //the inverse of the above; disable movement logic in this controller, enable PlayerController.cs

    }
}
