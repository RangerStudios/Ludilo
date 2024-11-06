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
    private Vector2 movementVector;
    public CharacterController characterController;
    public HealthController playerHealth;
    public StuffingController playerStuffing;
    public Attacker attackScript;
    public PlayerHealthScriptableObject savedPlayerHealth;
    private Vector3 direction;
    private Camera mainCamera;
    Rigidbody rb;
    [SerializeField] bool ragdolling = false;
    [SerializeField] bool crouching = false;
    [SerializeField] bool attackCooldown;
    bool canJump = true;

    //player movement values
    [SerializeField] public float speed;
    [SerializeField] public float rotationSpeed = 500f; //smoothtime
    private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    [SerializeField] private float jumpPower;
    private float velocity;

    public float dustTimer;
    public float currentDustTimer;

    PlayerMovementState currentState = PlayerMovementState.Default;

    //interaction
    public delegate void Interact();
    public bool isDraggingMedium;
    public bool isDraggingLarge;
    public bool hanging;
    public bool isGrabbed;
    public bool isDusted;
    public int grabIncrement;
    public bool isHoldingItem;

    //Ladder logic - Jacob D
    [SerializeField] float climbSpeed = 10;
    public bool onLadder;
    Ladder activeLadder;
    bool exitLadder;

    public UnityEvent<int> onDamage;

    public float defaultSpeedModifier = 1;

    public float speedModifier;


    void OnEnable()
    {
        PlayerInput.onMove += MovementInput;
        PlayerInput.onJump += Jump;
        PlayerInput.onRagdoll += Ragdoll;
        PlayerInput.onCrouch += Crouch;
        PlayerInput.onAttack += Attack;
        Pinhead.GrabPlayer += Grabbed;
        Pinhead.ReleasePlayer += Released;
        DustExplode.DustPlayer += Dusted;
    }

    void OnDisable()
    {
        PlayerInput.onMove -= MovementInput;
        PlayerInput.onJump -= Jump;
        PlayerInput.onRagdoll -= Ragdoll;
        PlayerInput.onCrouch -= Crouch;
        PlayerInput.onAttack -= Attack;
        Pinhead.GrabPlayer -= Grabbed;
        Pinhead.ReleasePlayer -= Released;
        DustExplode.DustPlayer -= Dusted;
    }
    private void Awake()
    {
        speedModifier = defaultSpeedModifier;
        characterController = GetComponent<CharacterController>();
        playerHealth = GetComponent<HealthController>();
        playerStuffing = GetComponent<StuffingController>();
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
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
        if (isDusted)
        {
            currentDustTimer -= Time.deltaTime;
            if (currentDustTimer <= 0)
            {
                isDusted = false;
            }
        }
        ApplyRotation();
        ApplyGravity();
        ApplyMovement();
        LedgeGrab();

        if(ragdolling)
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = true;
            characterController.enabled = false;
            movementVector = Vector2.zero;
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

    void ChangePlayerState(PlayerMovementState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case PlayerMovementState.Ragdolling:
                rb.AddForce(transform.forward);
                break;
            case PlayerMovementState.Hanging:
                break;
            case PlayerMovementState.OnLadder:
                
                break;
        }
    }

    private void ApplyRotation()
    {
        if(!hanging && !isDraggingLarge && !ragdolling)
        {
            if (movementVector.sqrMagnitude == 0) return;
            direction = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(movementVector.x, 0.0f, movementVector.y);
            var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed *Time.deltaTime);
        }

        if(isDraggingLarge)
        {
            if (movementVector.sqrMagnitude == 0) return;
            direction = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(movementVector.x, 0.0f, movementVector.y);
        }
    }

    private void ApplyMovement()
    {

        //TODO: Add in an Enum to properly track the player's state with a switch case, rather than bools and a series of if statements

        switch(currentState)
        {
            case PlayerMovementState.Ragdolling:
                rb.AddForce(transform.forward);
            break;
            case PlayerMovementState.Hanging:
                //Debug.Log("Hang State");
                //Hanging anim here
                break;
            case PlayerMovementState.OnLadder:
                if (onLadder && !exitLadder)
                {
                    float vertInput = movementVector.y;
                    direction = new Vector3(0, vertInput, 0);
                    characterController.Move(direction * climbSpeed * Time.deltaTime);
                }
                else if (onLadder && exitLadder)
                {
                    //This is where all effects are applied when exiting a "ladder"
                    characterController.enabled = false;
                    //Set the animation trigger for 
                }
                break;

            default:
               characterController.Move(direction * (speed * speedModifier) * Time.deltaTime);
                //Debug.Log("Default State");
               break;
        }

        /*if(ragdolling)
        {
            rb.AddForce(transform.forward);
            //PlayerInput.onMove -= MovementInput;
        }*/

            if(!ragdolling && !hanging)
            {
            //PlayerInput.onMove += MovementInput;
            if (isDusted)
            {
                characterController.Move(direction * ((speed * 0.8f) / (grabIncrement + 1)) * Time.deltaTime);
            }

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
    public void MovementInput(Vector2 input)
    {
        movementVector = input;
        if(characterController.enabled)
        {
            direction = new Vector3(movementVector.x, 0.0f, movementVector.y);
        }  
    }

    public void Jump()
    {

        if (!canJump)
        {
            return;
        }
        //Debug.Log("Jump");
        //if (!context.started) return;
        if (ragdolling) return;
        if (isDraggingMedium) return;
        if (isDraggingLarge) return;

        if (hanging)
        {
            gravity = -9.81f;
            hanging = false;
            velocity += jumpPower;
            ChangePlayerState(PlayerMovementState.Default);
        }
        else
        {
            if (!IsGrounded()) return;
            velocity += jumpPower;
        }
        
    }

    public void Crouch()
    {
        //Debug.Log("ButtonPress");
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
    

    public void Ragdoll()
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
                    ChangePlayerState(PlayerMovementState.Hanging);

                    Vector3 hangPos = new Vector3(fwdHit.point.x, downHit.point.y, fwdHit.point.z);
                    Vector3 offset = transform.forward * -0.1f + transform.up * -0.5f;
                    hangPos += offset;
                    transform.position = hangPos;
                    transform.forward = -fwdHit.normal;
                }
            }
        }
    }

    void Attack()
    {
        if (!isGrabbed && !attackCooldown)
        {
            //Logic, anim trigger, etc.
            attackCooldown = true;
            attackScript.AttackCheck();
            StartCoroutine(AttackCooldown());
        }
        else
        {
            Pinhead[] latchedPinheads = GetComponentsInChildren<Pinhead>();
            foreach(Pinhead p in latchedPinheads)
            {
                p.currentGrabTimer -= 0.25f;
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1.0f); //1 second is a little sluggish, I know. Planning on tuning it.
        attackCooldown = false;

    }


    public void OnLadder(Vector3 position, Ladder currentLadder)
    {
        //Animation SetBool for being on ladder/ climbing a rope
        //Animation SetFloat for the speed of the player
        characterController.enabled = false;
        characterController.transform.position = position;
        characterController.enabled = true;
        activeLadder = currentLadder;
        onLadder = true;
        ChangePlayerState(PlayerMovementState.OnLadder);
    }
    public void ExitLadder()
    {
        exitLadder = true;
        LadderExitComplete();
    }

    public void LadderExitComplete()
    {
        characterController.enabled = false;
        characterController.transform.position = activeLadder.GetEndPosition();
        direction = Vector3.zero;
        onLadder = false;
        exitLadder = false;
        characterController.enabled = true;
    }


    void Grabbed()
    {
        isGrabbed = true;
        grabIncrement += 1;
    }


    void Released()
    {
        grabIncrement -= 1;
        if (grabIncrement <= 0)
        {
            isGrabbed = false;
        }
    }

    void Dusted()
    {
        Debug.Log("Hello I am Active");
        isDusted = true;
        currentDustTimer = dustTimer;
    }
}

public enum PlayerMovementState{
    Default,
    Ragdolling,
    OnGround,
    Hanging,
    Dragging,
    Grabbed,
    OnLadder
}
