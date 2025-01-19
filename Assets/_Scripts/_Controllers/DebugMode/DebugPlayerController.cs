using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DebugPlayerController : MonoBehaviour
{

    GameObject menuUI;
    GameObject player;
    Rigidbody rb;
    public CharacterController characterController;
    private Camera mainCamera;
    GameObject noclipCam;
    private Vector2 movementVector;
    private Vector3 direction;
    public Transform orientation;
    public Transform playerTransform;
    public Transform playerTransformObj;

    public float noclipMoveSpeed;
    public float rotationSpeed;
    

    void OnEnable()
    {
        PlayerInput.onMove += MovementInput;
        //PlayerInput.onJump += Up;
        //PlayerInput.onCrouch += Down;
    }

    void OnDisable()
    {
        PlayerInput.onMove -= MovementInput;
        //PlayerInput.onJump -= Up;
        //PlayerInput.onCrouch -= Down;
    }
    
    void Awake()
    {
         menuUI = GameObject.Find("Canvas");
         characterController = GetComponent<CharacterController>();
         mainCamera = Camera.main;
    }

    void Update()
    {
        ApplyRotation();
        ApplyMovement();
    }

    void MovementInput(Vector2 input)
    {
        movementVector = input;
        direction = new Vector3(movementVector.x, 0.0f, movementVector.y);
    }

    private void ApplyMovement()
    {
        characterController.Move(direction * noclipMoveSpeed * Time.deltaTime);
    }
    
    private void ApplyRotation()
    {
        if (movementVector.sqrMagnitude == 0) return;
        direction = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(movementVector.x, 0.0f, movementVector.y);
        var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed *Time.deltaTime);
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
