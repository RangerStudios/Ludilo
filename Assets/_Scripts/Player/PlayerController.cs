//Script by Anthony C.

using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable, IPlaySounds
{
    //setup
    [Header("Component References")]
    private Vector2 movementVector;
    public CharacterController characterController;
    public Animator playerAnimator;
    public HealthController playerHealth;
    public Attacker attackScript;
    public PlayerHealthScriptableObject savedPlayerHealth;
    private Vector3 inputDirection;
    private Vector3 movementDirection;
    private Camera mainCamera;
    Rigidbody rb;

    [Space(10)]
    [Header("Events")]
    public static Action OnDie;

    [Space(10)]
    [Header("Booleans")]
    [SerializeField] bool crouching = false;
    [SerializeField] bool canCrouch;
    [SerializeField] public bool canAttack;
    [SerializeField] public bool canInteract;
    [SerializeField] bool attackCooldown;
    //[SerializeField] bool injured;
    public bool canJump = true;
    public GameObject menuUI;

    //player movement values
    [Space(10)]
    [Header("Player Movement Values")]
    [SerializeField] public float speed;
    [SerializeField] public float rotationSpeed;
    [SerializeField] float acceleration;
    private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float jumpPower;
    private float downForce;

    [Space(10)]
    [Header("Timers")]
    public float dustTimer;
    public float currentDustTimer;
    public float healTimer = 5f;
    public float currentHealTimer;

    [SerializeField] PlayerMovementState currentState;

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

    //public UnityEvent<int> onDamage;

    public float defaultSpeedModifier = 1;

    public float speedModifier;

    [SerializeField] private PlayerSoundsResource playerSounds;


    void OnEnable()
    {
        PlayerInput.onMove += MovementInput;
        PlayerInput.onJump += Jump;
        PlayerInput.onCrouch += Crouch;
        PlayerInput.onAttack += Attack;
        //PlayerInput.onSelect +=
        Pinhead.GrabPlayer += Grabbed;
        Pinhead.ReleasePlayer += Released;
        DustExplode.DustPlayer += Dusted;
    }

    void OnDisable()
    {
        PlayerInput.onMove -= MovementInput;
        PlayerInput.onJump -= Jump;
        PlayerInput.onCrouch -= Crouch;
        PlayerInput.onAttack -= Attack;
        //PlayerInput.onSelect +=
        Pinhead.GrabPlayer -= Grabbed;
        Pinhead.ReleasePlayer -= Released;
        DustExplode.DustPlayer -= Dusted;
    }
    private void Awake()
    {
        speedModifier = defaultSpeedModifier;
        characterController = GetComponent<CharacterController>();
        playerHealth = GetComponent<HealthController>();
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        menuUI = GameObject.Find("Canvas");
    }

    private void Start()
    {
        playerHealth.health = savedPlayerHealth.currentHealth;
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
        if (playerHealth.health == 1)
        {
            currentHealTimer += Time.deltaTime;
            if (currentHealTimer >= healTimer)
            {
                playerHealth.health += 1;
                currentHealTimer = 0f;
            }
        }
        
        ApplyRotation();
        LedgeGrab();

        //NOTE: Animator parameters don't need if-statements between them and can just be assigned like so
        playerAnimator.SetBool("isHolding", isHoldingItem);

        //Don't make an if-statement if you don't have to. Makes the code cleaner and easier to read

        if (movementVector.sqrMagnitude == 0)
        {
            playerAnimator.SetBool("isMoving", false);
        }
        else
        {
            playerAnimator.SetBool("isMoving", true);
        }



        if(crouching)
        {
            characterController.height = 1.0f;
            characterController.center = new Vector3(0f, -0.4f, 0f);
            canAttack = false;
            canInteract = false;
            playerAnimator.SetBool("isStandingUp", false);
            playerAnimator.SetBool("isCrouching", true);
            StartCoroutine(IdleCrouchBool());
            canJump = false;
        }
        else
        {
            characterController.height = 2.0f;
            characterController.center = new Vector3(0f, 0f, 0f);
            canAttack = true;
            canInteract = true;
            playerAnimator.SetBool("isStandingUp", true);
            playerAnimator.SetBool("isCrouched", false);
            //canJump = true;
        }
    }

    void FixedUpdate()
    {
        //All movement calculations and similar should be in fixed update, same with gravity

        ApplyGravity();
        ApplyMovement();
    }

    IEnumerator IdleCrouchBool()
    {
        yield return new WaitForSeconds(0.5f);
        playerAnimator.SetBool("isCrouched", true);
        playerAnimator.SetBool("isCrouching", false);
    }

    public void ChangePlayerState(PlayerMovementState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case PlayerMovementState.Hanging:
                //This is where we call everything that needs to happen when the player first starts hanging
                break;
            case PlayerMovementState.OnLadder:
                break;
            case PlayerMovementState.Dragging:
                break;
            case PlayerMovementState.HoldingMediumItem:
                break;
            case PlayerMovementState.Grabbed:
                break;
            case PlayerMovementState.OnGround:
                //Location we can use to trigger hard-fall animations or similar effects when the player FIRST touches the ground
                break; 
        }
    }

    private void ApplyRotation()
    {

        switch(currentState)
        {
            default:
                    if (movementVector.sqrMagnitude == 0) return;
                    movementDirection = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(movementVector.x, 0.0f, movementVector.y);
                    var targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed *Time.deltaTime);
                break;
            case PlayerMovementState.Dragging:
                if (movementVector.sqrMagnitude == 0) return;
                movementDirection = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(movementVector.x, 0.0f, movementVector.y); 
                //Debug.Log("Drag State"); 
                break;
            case PlayerMovementState.HoldingMediumItem:
                //Debug.Log("Medium State");
                goto default;             
        }

    }

    private void ApplyMovement()
    {

        //TODO: Add in an Enum to properly track the player's state with a switch case, rather than bools and a series of if statements

        switch(currentState)
        {
            case PlayerMovementState.Hanging:
                canCrouch = false;
                //Debug.Log("Hang State");
                playerAnimator.SetBool("isHanging", true);
                break;
            case PlayerMovementState.OnLadder:
                if (onLadder && !exitLadder)
                {
                    float vertInput = inputDirection.y;
                    inputDirection = new Vector3(0, vertInput, 0);
                    characterController.Move(inputDirection * climbSpeed * Time.deltaTime);
                }
                else if (onLadder && exitLadder)
                {
                    //This is where all effects are applied when exiting a "ladder"
                    characterController.enabled = false;
                    //Set the animation trigger for 
                }
                break;
            case PlayerMovementState.Dragging:
                //characterController.Move(inputDirection * (speed * speedModifier) * Time.deltaTime);
                speedModifier = 0.2f;
                rotationSpeed = 250f;
                goto default;
            case PlayerMovementState.HoldingMediumItem:
                speedModifier = 0.4f;
                rotationSpeed = 250f;
                goto default;
            default:

                    var factor = acceleration * Time.fixedDeltaTime;
                    Vector3 playerVelocity = new();

                    playerVelocity.x = Mathf.Lerp(playerVelocity.x, movementDirection.x * speed * speedModifier, factor);
                    playerVelocity.z = Mathf.Lerp(playerVelocity.z, movementDirection.z * speed * speedModifier, factor);
                    playerVelocity.y = movementDirection.y;

                    characterController.Move(playerVelocity * Time.fixedDeltaTime);
                    gameObject.GetComponent<CapsuleCollider>().enabled = false;
                    characterController.enabled = true;
                    GetComponent<Rigidbody>().isKinematic = true;
                    //canJump = true;
                    canCrouch = true;
                    speedModifier = 1f;
                    //Debug.Log("Default State");
                
               break;
        }

        //TO:DO Adjust the application of the speed debuff so it actually works
        if (isDusted)
        {
            speedModifier = 0.6f;
            characterController.Move(inputDirection * (speedModifier / (grabIncrement + 1)) * Time.deltaTime);
        }
        
        
    }

    private void ApplyGravity()
    {
        if (IsGrounded() && downForce < 0.0f)
        {
            downForce = -1.0f;
            playerAnimator.SetBool("isGrounded", true);
            playerAnimator.SetBool("isJumping", false);
            playerAnimator.SetBool("isFalling", false);
        }
        else
        {
            downForce += gravity * gravityMultiplier * Time.fixedDeltaTime;
            playerAnimator.SetBool("isFalling", true);
            playerAnimator.SetBool("isGrounded", false);
        }
        
        movementDirection.y = downForce;
    }
    public void MovementInput(Vector2 input)
    {

        movementVector = input;
        if(characterController.enabled)
        {
            inputDirection = new Vector3(input.x, 0.0f, input.y);
            movementDirection = inputDirection;
        }  
    }

    public void Jump()
    {

        if (!canJump)
        {
            return;
        }
        if(currentState == PlayerMovementState.Hanging)
        {
            gravity = -9.81f;
            hanging = false;
            PlaySoundEffect(playerSounds.JumpSounds[UnityEngine.Random.Range(0, playerSounds.JumpSounds.Count - 1)]);
            downForce += jumpPower;
            playerAnimator.SetBool("isHanging", false);
            playerAnimator.SetBool("isJumping", true);
            ChangePlayerState(PlayerMovementState.Default);
        }
        else
        {
            if (!IsGrounded()) return;
            PlaySoundEffect(playerSounds.JumpSounds[UnityEngine.Random.Range(0, playerSounds.JumpSounds.Count - 1)]);
            downForce += jumpPower;
            playerAnimator.SetBool("isJumping", true);
        }
        
    }

    public void Crouch()
    {
        if (canCrouch)
        {
            //Debug.Log("ButtonPress");
            CrouchState();
        }
        
    }

    public void CrouchState()
    {
        crouching = !crouching;
        canJump = !canJump;
    }

    private bool IsGrounded() => characterController.isGrounded;

    public void Damage(int damageValue)
    {
        playerHealth.Damage(damageValue);
        //onDamage.Invoke(damageValue);
        PlaySoundEffect(playerSounds.HitSounds[UnityEngine.Random.Range(0, playerSounds.HitSounds.Count - 1)]);
    }

    public void Die()
    {
        currentHealTimer = 0f;
        OnDie();
    }
    


    void LedgeGrab()
    {
        inputDirection.y = downForce;

        if(downForce < 0.0f && !hanging && !IsGrounded())
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
                    downForce = 0.0f;

                    ChangePlayerState(PlayerMovementState.Hanging);
                    hanging = true;

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
        if (!isGrabbed && !attackCooldown && !isHoldingItem)
        {
            if (canAttack)
            {
                attackCooldown = true;
                playerAnimator.SetBool("isAttacking", true);
                PlaySoundEffect(playerSounds.AttackSounds[UnityEngine.Random.Range(0, playerSounds.AttackSounds.Count - 1)]);
                StartCoroutine(AttackAnimDelay());
                StartCoroutine(AttackCooldown());
            }
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
        playerAnimator.SetBool("isAttacking", false);
        attackCooldown = false;

    }

    IEnumerator AttackAnimDelay()
    {
        yield return new WaitForSeconds(0.5f);
        attackScript.AttackCheck();
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
        inputDirection = Vector3.zero;
        onLadder = false;
        exitLadder = false;
        characterController.enabled = true;
        ChangePlayerState(PlayerMovementState.Default);
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

    public void PlaySoundEffect(AudioClip soundEffect)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(soundEffect);
    }

    public void PlayWalkSound()
    {
        PlaySoundEffect(playerSounds.WalkSounds[UnityEngine.Random.Range(0, playerSounds.WalkSounds.Count - 1)]);
    }
}

public enum PlayerMovementState{
    Default,
    OnGround,
    Hanging,
    Dragging,
    HoldingMediumItem,
    Grabbed,
    OnLadder
}
