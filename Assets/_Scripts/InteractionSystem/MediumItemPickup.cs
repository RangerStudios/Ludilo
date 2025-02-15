using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MediumItemPickup : Interactable
{
    public bool activated;
    public GameObject heldObject;
    public float radius = 2f;
    public float distance = 1.4f;
    public float rayDist;
    float diffForce = 500;
    //public float height = 0.3f;
    public PlayerController playerController;    
    public Interactor playerInteractor;
    public GameObject player;
    public Transform playerTransform;
    Transform heldObjectTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        playerInteractor = player.GetComponent<Interactor>();
        
    }

    public void DragState()
    {
        if(activated == false && playerController.isHoldingItem == false)
        {
            activated = true;
            //playerController.isHoldingItem = true;
            //playerController.ChangePlayerState(PlayerMovementState.HoldingMediumItem);
        }
        else
        {
            activated = false;
            //playerController.isHoldingItem = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
    
        var t = playerTransform;
        if(heldObject) //Object Holding Rework: Make the heldObject a child of the player, make sure objects face forward when picked up
                       //make sure theres no physics funny business (i.e. object stops player when hitting a wall as opposed to object getting stuck and leaving player range)
        {   
            heldObjectTransform = heldObject.transform;            
            playerController.canJump = false;

            var rigidbody = heldObject.GetComponent<Rigidbody>();
            heldObject.transform.parent = player.transform;
            //heldObjectTransform.rotation = playerTransform.rotation;
            
            
            if(playerInteractor.numFound == 0)
        {
            DropMediumItem();
        }
            if(!activated)
            {
                //var rigidbody = heldObject.GetComponent<Rigidbody>();
                rigidbody.drag = 1f;
                rigidbody.useGravity = true;
                rigidbody.constraints = RigidbodyConstraints.None;
                DropMediumItem();
            }
        }
        else
        {
            if (activated)
            {
                RaycastHit hits;

                if (Physics.Raycast(t.position, t.forward, out hits, rayDist))
                {
                    if (hits.transform.tag == "MediumPickUp")
                    {
                        Debug.Log("Fucks sake");
                        var hitObject = hits.transform.gameObject;
                        heldObject = hitObject;
                        var rigidbody = heldObject.GetComponent<Rigidbody>();
                        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                        rigidbody.drag = 25f;
                        rigidbody.useGravity = false;
                        playerController.isHoldingItem = true;
                        playerController.ChangePlayerState(PlayerMovementState.HoldingMediumItem);
                    }
                }
            }
        }
        if (!heldObject && activated)
        {
            activated = false;
        } 
    }

    private void FixedUpdate()
    {
        var t = playerTransform;
        if(heldObject)
        {
            
        }
    }

    public void DropMediumItem()
    {
        if(heldObject) //remove object as child here
        {
            heldObject.transform.parent = null;
            heldObject = null;
            activated = false;
            playerController.isHoldingItem = false;
            playerController.canJump = true;
            playerController.ChangePlayerState(PlayerMovementState.Default);
        }
        //else //Old failsafe, not even sure why it was here because it was fucking stuff up. Leaving it here in case I need it for some reason.
        //{
            //playerController.ChangePlayerState(PlayerMovementState.HoldingMediumItem);
        //}
    }
}
