//Script by Anthony C.

using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable, IPlaySounds
{
    //setup
    private Vector2 movementVector;
    public CharacterController characterController;
    public Animator playerAnimator;
    public HealthController playerHealth;
    public StuffingController playerStuffing;
    public Attacker attackScript;
    public PlayerHealthScriptableObject savedPlayerHealth;
    private Vector3 direction;
    private Camera mainCamera;
    Rigidbody rb;
    [SerializeField] bool moveInput;
    [SerializeField] bool ragdolling = false;
    [SerializeField] bool crouching = false;
    [SerializeField] bool canCrouch;
    [SerializeField] public bool canAttack;
    [SerializeField] public bool canInteract;
    [SerializeField] bool attackCooldown;
    public bool canJump = true;
    public GameObject menuUI;

    //player movement values
    [SerializeField] public float speed;
    [SerializeField] public float rotationSpeed;
    private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float jumpPower;
    private float velocity;

    public float dustTimer;
    public float currentDustTimer;

    PlayerMovementState currentState;

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

    [SerializeField] private PlayerSoundsResource playerSounds;


    void OnEnable()
    {
        PlayerInput.onMove += MovementInput;
        PlayerInput.onJump += Jump;
        PlayerInput.onRagdoll += Ragdoll;
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
        PlayerInput.onRagdoll -= Ragdoll;
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
        playerStuffing = GetComponent<StuffingController>();
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        menuUI = GameObject.Find("Canvas");
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

        if (movementVector.sqrMagnitude == 0)
        {
            playerAnimator.SetBool("isMoving", false);
        }
        else
        {
            playerAnimator.SetBool("isMoving", true);
            moveInput = true;
        }

        if(isHoldingItem == true)
        {
            playerAnimator.SetBool("isHolding", true);
        }
        else
        {
            playerAnimator.SetBool("isHolding", false);
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
            canJump = true;
        }
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
            case PlayerMovementState.Ragdolling:
                break;
            case PlayerMovementState.Hanging:
                break;
            case PlayerMovementState.OnLadder:
                break;
            case PlayerMovementState.Dragging:
                break;
            case PlayerMovementState.Grabbed:
                break;
            case PlayerMovementState.OnGround:
                break; 
        }
    }

    private void ApplyRotation()
    {

        switch(currentState)
        {
            case PlayerMovementState.Default:
                if(!ragdolling && !hanging)
                {
                    if (movementVector.sqrMagnitude == 0) return;
                    direction = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(movementVector.x, 0.0f, movementVector.y);
                    var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed *Time.deltaTime);
                }
                break;
            case PlayerMovementState.Dragging:
                if (movementVector.sqrMagnitude == 0) return;
                direction = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(movementVector.x, 0.0f, movementVector.y); 
                Debug.Log("Drag State"); 
                break;             
        }

    }

    private void ApplyMovement()
    {

        //TODO: Add in an Enum to properly track the player's state with a switch case, rather than bools and a series of if statements

        switch(currentState)
        {
            case PlayerMovementState.Ragdolling:
                gameObject.GetComponent<CapsuleCollider>().enabled = true;
                characterController.enabled = false;
                movementVector = Vector2.zero;
                GetComponent<Rigidbody>().isKinematic = false;
                rb.AddForce(transform.forward);
                //playerAnimator.SetBool("isRagdoll", true);
                canJump = false;
                canCrouch = false;
                //Debug.Log("RagdollState");
            break;
            case PlayerMovementState.Hanging:
                canCrouch = false;
                //Debug.Log("Hang State");
                playerAnimator.SetBool("isHanging", true);
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
            case PlayerMovementState.Dragging:
                characterController.Move(direction * (speed * speedModifier) * Time.deltaTime);
                canJump = false;
                speedModifier = 0.2f;
                rotationSpeed = 250f;
                break;

            default:
                
                    characterController.Move(direction * (speed * speedModifier) * Time.deltaTime);
                    gameObject.GetComponent<CapsuleCollider>().enabled = false;
                    characterController.enabled = true;
                    GetComponent<Rigidbody>().isKinematic = true;
                    canJump = true;
                    canCrouch = true;
                    speedModifier = 1f;
                    //Debug.Log("Default State");
                
               break;
        }


        if (isDusted)
        {
            speedModifier = 0.6f;
            characterController.Move(direction * (speedModifier / (grabIncrement + 1)) * Time.deltaTime);
        }
        
        
    }

    private void ApplyGravity()
    {
        if (IsGrounded() && velocity < 0.0f)
        {
            velocity = -1.0f;
            playerAnimator.SetBool("isGrounded", true);
            playerAnimator.SetBool("isJumping", false);
            playerAnimator.SetBool("isFalling", false);
        }
        else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
            playerAnimator.SetBool("isFalling", true);
            playerAnimator.SetBool("isGrounded", false);
        }
        
        direction.y = velocity;
    }
    public void MovementInput(Vector2 input)
    {
        movementVector = input;
        if(characterController.enabled)
        {
            direction = new Vector3(movementVector.x, 0.0f, movementVector.y);
            playerAnimator.SetBool("isRagdoll", false);
        }  
    }

    public void Jump()
    {

        if (!canJump)
        {
            return;
        }

        if (hanging)
        {
            gravity = -9.81f;
            hanging = false;
            PlaySoundEffect(playerSounds.JumpSounds[Random.Range(0, playerSounds.JumpSounds.Count - 1)]);
            velocity += jumpPower;
            playerAnimator.SetBool("isHanging", false);
            playerAnimator.SetBool("isJumping", true);
            ChangePlayerState(PlayerMovementState.Default);
        }
        else
        {
            if (!IsGrounded()) return;
            PlaySoundEffect(playerSounds.JumpSounds[Random.Range(0, playerSounds.JumpSounds.Count - 1)]);
            velocity += jumpPower;
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
    }

    private bool IsGrounded() => characterController.isGrounded;

    public void Damage(int damageValue)
    {
        playerHealth.Damage(damageValue);
        onDamage.Invoke(damageValue);
        PlaySoundEffect(playerSounds.HitSounds[Random.Range(0, playerSounds.HitSounds.Count - 1)]);
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
        if (ragdolling)
        {
            ChangePlayerState(PlayerMovementState.Ragdolling);
        }
        else
        {
            ChangePlayerState(PlayerMovementState.Default);
        }
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
                PlaySoundEffect(playerSounds.AttackSounds[Random.Range(0, playerSounds.AttackSounds.Count - 1)]);
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
        direction = Vector3.zero;
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
        PlaySoundEffect(playerSounds.WalkSounds[Random.Range(0, playerSounds.WalkSounds.Count - 1)]);
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
