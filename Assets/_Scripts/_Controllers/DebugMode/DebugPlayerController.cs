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

    private float noclipMoveSpeed = 10;
    private float upSpeed = 3;
    private float rotationSpeed = 1000;

    [SerializeField] bool upActive = false;
    [SerializeField] bool downActive = false;
    

    void OnEnable()
    {
        PlayerInput.onMove += MovementInput;
        PlayerInput.onJump += Up;
        PlayerInput.onCrouch += Down;

         Physics.IgnoreLayerCollision(6, 12, true); // Disable collisions
    }

    void OnDisable()
    {
        PlayerInput.onMove -= MovementInput;
        PlayerInput.onJump -= Up;
        PlayerInput.onCrouch -= Down;

         Physics.IgnoreLayerCollision(6, 12, false);// Enable collisions
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

        if (upActive)
        {
            UpMove();
        }

        if (downActive)
        {
            DownMove();
        }
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

    private void Up()
    {
        upActive = !upActive;

        if (downActive)
        {
            downActive = false;
        }
    }

    private void UpMove()
    {
        Vector3 upVector = new Vector3(0, 1, 0); // Move only in the Y direction

        characterController.Move(upVector * upSpeed * Time.deltaTime); // Apply the movement
    }

    private void Down()
    {
        downActive = !downActive;

        if (upActive)
        {
            upActive = false;
        }
    }

    private void DownMove()
    {
        Vector3 downVector = new Vector3(0, -1, 0); // Move only in the Y direction

        characterController.Move(downVector * upSpeed * Time.deltaTime); // Apply the movement
    }
}
