//Script by Anthony C.

using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    //setup
    private Vector2 input;
    public CharacterController characterController;
    public HealthController playerHealth;
    public StuffingController playerStuffing;
    public PlayerHealthScriptableObject savedPlayerHealth;
    private Vector3 direction;
    private Camera mainCamera;
    [SerializeField] bool ragdolling = false;
    [SerializeField] bool crouching = false;
    // crouching: Gonna need to disable the basic capsule collider unless ragdolling, and move the CController center to y: -0.4 and the height to 1 while active

    //player movement values
    [SerializeField] public float speed;
    [SerializeField] public float rotationSpeed = 500f; //smoothtime
    private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    [SerializeField] private float jumpPower;
    private float velocity;

    //interaction
    public delegate void Interact();
    public event Interact OnInteraction;
    public bool isDraggingMedium;
    public bool hanging;

    public UnityEvent<int> onDamage;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerHealth = GetComponent<HealthController>();
        playerStuffing = GetComponent<StuffingController>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        playerHealth.health = savedPlayerHealth.currentHealth;
        playerStuffing.stuffingCount = savedPlayerHealth.currentStuffing;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Debug.Log("Player Health is: " + playerHealth.health);
    }

    private void Update()
    {
        ApplyRotation();
        ApplyGravity();
        ApplyMovement();
        LedgeGrab();

        if(ragdolling)
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = true;
            characterController.enabled = false;
            input = Vector2.zero;
            GetComponent<Rigidbody>().isKinematic = false;
        }
        if(!ragdolling)
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            characterController.enabled = true;
            GetComponent<Rigidbody>().isKinematic = true;
        }

        if(crouching)
        {
            characterController.height = 1.0f;
            characterController.center = new Vector3(0f, -0.4f, 0f);
        }
        if(!crouching)
        {
            characterController.height = 2.0f;
            characterController.center = new Vector3(0f, 0f, 0f);
        }
    }

    private void ApplyRotation()
    {
        if(!hanging)//&& !isDraggingLarge
        {
            if (input.sqrMagnitude == 0) return;
            direction = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(input.x, 0.0f, input.y);
            var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed *Time.deltaTime);
        }

        //if(isDraggingLarge)
        //{
            //if (input.sqrMagnitude == 0) return;
            //direction = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(input.x, 0.0f, input.y);
        //}
    }

    private void ApplyMovement()
    {
        if(!ragdolling && !hanging)
        {
            characterController.Move(direction * speed * Time.deltaTime);
        }
        
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
        if(characterController.enabled)
        {
            input = context.ReadValue<Vector2>();
            direction = new Vector3(input.x, 0.0f, input.y);
        }  
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log("Jump");
        if (!context.started) return;
        if (ragdolling) return;
        if (isDraggingMedium) return;

        if (hanging)
        {
            gravity = -9.81f;
            hanging = false;
            velocity += jumpPower;
        }
        else
        {
            if (!IsGrounded()) return;
            velocity += jumpPower;
        }
        
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        Debug.Log("ButtonPress");
        CrouchState();
    }

    public void CrouchState()
    {
        crouching = !crouching;
    }

    private bool IsGrounded() => characterController.isGrounded;

    public void Damage(int damageValue)
    {
        playerHealth.Damage(damageValue);
        onDamage.Invoke(damageValue);
    }

    public void Die()
    {
        Debug.Log("Player Dies");
        this.gameObject.SetActive(false);
    }
    

    public void Interaction(InputAction.CallbackContext context)
    {
        
        OnInteraction?.Invoke();
        if(!context.started) return;
        //Debug.Log("Interaction");
    }

    public void Ragdoll(InputAction.CallbackContext context)
    {
        RagdollState();
    }

    public void RagdollState()
    {
        ragdolling = !ragdolling;
    }

    void LedgeGrab()
    {
        direction.y = velocity;

        if(velocity < 0.0f && !hanging && !IsGrounded())
        {
            RaycastHit downHit;
            Vector3 lineDownStart = (transform.position + Vector3.up * 1.8f) + transform.forward;
            Vector3 lineDownEnd = (transform.position + Vector3.up * 0.7f) + transform.forward;
            Physics.Linecast(lineDownStart, lineDownEnd, out downHit, LayerMask.GetMask("Ground"));
            //Debug.DrawLine(lineDownStart, lineDownEnd);

            if(downHit.collider != null)
            {
                RaycastHit fwdHit;
                Vector3 lineFwdStart = new Vector3(transform.position.x, downHit.point.y-0.1f, transform.position.z);
                Vector3 lineFwdEnd = new Vector3(transform.position.x, downHit.point.y-0.1f, transform.position.z) + transform.forward;
                Physics.Linecast(lineFwdStart, lineFwdEnd, out fwdHit, LayerMask.GetMask("Ground"));
                //Debug.DrawLine(lineFwdStart, lineFwdEnd);

                if(fwdHit.collider != null)
                {
                    gravity = 0.0f;
                    velocity = 0.0f;

                    hanging = true;
                    //NEED TO FORCE ABILITY TO JUMP DUE TO GROUNDED BEING FALSE
                    //Animator.SetTrigger("HangAnim")

                    Vector3 hangPos = new Vector3(fwdHit.point.x, downHit.point.y, fwdHit.point.z);
                    Vector3 offset = transform.forward * -0.1f + transform.up * -0.5f;
                    hangPos += offset;
                    transform.position = hangPos;
                    transform.forward = -fwdHit.normal;
                }
            }
        }
    }

    void Attack(InputAction.CallbackContext context)
    {
        //insert attack code here
        //Logic, anim trigger, etc.
    }


}
