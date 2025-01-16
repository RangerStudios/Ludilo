using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DebugPlayerController : MonoBehaviour
{

    GameObject menuUI;
    GameObject player;
    Rigidbody rb;
    public float noclipMoveSpeed;
    

    void OnEnable()
    {
        //PlayerInput.onMove += MovementInput;
        //PlayerInput.onJump += Up;
        //PlayerInput.onCrouch += Down;
    }

    void OnDisable()
    {
        //PlayerInput.onMove -= MovementInput;
        //PlayerInput.onJump -= Up;
        //PlayerInput.onCrouch -= Down;
    }
    
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
