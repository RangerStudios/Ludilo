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
    //public float height = 0.3f;
    public PlayerController playerController;    
    public Interactor playerInteractor;
    public GameObject player;
    public Transform playerTransform;
    
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
            playerController.isHoldingItem = true;
        }
        else
        {
            activated = false;
            playerController.isHoldingItem = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
    
        var t = playerTransform;
        if(heldObject)
        {
            
            playerController.canJump = false;
            playerController.speed = 3;
            playerController.rotationSpeed = 250f;
            if(!activated)
            {
                var rigidbody = heldObject.GetComponent<Rigidbody>();
                rigidbody.drag = 1f;
                rigidbody.useGravity = true;
                rigidbody.constraints = RigidbodyConstraints.None;
                heldObject = null;
                playerController.speed = 5.7f;
                playerController.rotationSpeed = 1000f;
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
                        var hitObject = hits.transform.gameObject;
                        heldObject = hitObject;
                        var rigidbody = heldObject.GetComponent<Rigidbody>();
                        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                        rigidbody.drag = 25f;
                        rigidbody.useGravity = false;
                    }
                }
            }
        else
        {
          
        }
        }
    }

    private void FixedUpdate()
    {
        var t = playerTransform;
        if(heldObject)
        {
            var rigidbody = heldObject.GetComponent<Rigidbody>();
            var moveTo = t.position + distance * t.forward;
            var difference = moveTo - heldObject.transform.position;
            rigidbody.AddForce(difference * 500);
            heldObject.transform.rotation = t.rotation;

            if(playerInteractor.numFound == 0)
        {
            DropMediumItem();
        }
        }
    }

    public void DropMediumItem()
    {
        if(heldObject)
        {
            activated = false;
            playerController.isHoldingItem = false;
            playerController.canJump = true;
        }
    }
}
