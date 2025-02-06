using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PullDrawer : Interactable
{
     public bool activated = false;
    public GameObject heldObject;
    public float radius;
    public float distance;
    public float rayDist;
    //public float height = 0.3f;
    public PlayerController playerController;
    public Interactor playerInteractor;
    public GameObject player;
    public Transform playerTransform;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerInteractor = player.GetComponent<Interactor>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
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
            //playerController.speed = 1;
            //playerController.rotationSpeed = 250f;
            
            if(!activated)
            {
                var rigidbody = heldObject.GetComponent<Rigidbody>();
                rigidbody.drag = 1f;
                rigidbody.isKinematic = true;
                //rigidbody.constraints = RigidbodyConstraints.None;
                heldObject = null;
                playerController.speed = 5.7f;
                playerController.rotationSpeed = 500f;
            }
        }
        else
        {
            
            if (activated)
            {
                RaycastHit hits;
                //hits = Physics.Raycast(t.position, t.forward, hits);
                //var hitIndex = Array.FindIndex(hits, hit => hit.transform.tag == "PullDrawer");

                if (Physics.Raycast(t.position, t.forward, out hits, rayDist))
                {
                    if (hits.transform.tag == "PullDrawer")
                    {
                        var hitObject = hits.transform.gameObject;
                        heldObject = hitObject;
                        var rigidbody = heldObject.GetComponent<Rigidbody>();
                        rigidbody.isKinematic = false;
                        //rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                        rigidbody.drag = 25f;
                        //rigidbody.useGravity = false;
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
        if (heldObject)
        {
            var rigidbody = heldObject.GetComponent<Rigidbody>();
            var moveTo = t.position + distance * t.forward;
            var difference = moveTo - heldObject.transform.position;
            rigidbody.AddForce(difference * 500);
            //heldObject.transform.rotation = t.rotation;
        
        if(playerInteractor.numFound == 0)
        {
            ReleaseDrawer();
        }
        }
    }

    public void ReleaseDrawer()
    {
        if(heldObject)
        {
            activated = false;
            playerController.isHoldingItem = false;
            playerController.canJump = true;
            playerController.ChangePlayerState(PlayerMovementState.Default);
        }
        else
        {
            playerController.ChangePlayerState(PlayerMovementState.Dragging);
        }
    }
}
