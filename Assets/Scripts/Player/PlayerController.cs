//Script by Anthony C.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //setup
    private Vector2 input;
    private CharacterController characterController;
    private Vector3 direction;
    private Camera mainCamera;

    //player movement values
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed = 500f; //smoothtime
    private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    [SerializeField] private float jumpPower;
    private float velocity;

    //interaction
    public delegate void Interact();
    public event Interact OnInteraction;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        ApplyRotation();
        ApplyGravity();
        ApplyMovement();
    }

    private void ApplyRotation()
    {
        if (input.sqrMagnitude == 0) return;
        direction = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(input.x, 0.0f, input.y);
        var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed *Time.deltaTime);
        //var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        //var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
        //transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    private void ApplyMovement()
    {
        characterController.Move(direction * speed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (IsGrounded() && velocity < 0.0f)
        {
            velocity = -1.0f;
        }
        else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        
        direction.y = velocity;
    }
    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0.0f, input.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log("Jump");
        if (!context.started) return;
        if (!IsGrounded()) return;

        velocity += jumpPower;
    }

    private bool IsGrounded() => characterController.isGrounded;

    public void Interaction(InputAction.CallbackContext context)
    {
        
        OnInteraction?.Invoke();
        Debug.Log("Interaction");
        //if(!context.started) return;
    }
}
