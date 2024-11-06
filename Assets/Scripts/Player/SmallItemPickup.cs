using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

//Code by Anthony C.
//Apply this script to the player for picking up small items
public class SmallItemPickup : Interactable
{
    [SerializeField] bool activated = false;
    public GameObject heldObject;
    public float radius = 2f;
    public float distance = 1.2f;
    public float height = 0.3f;
    public PlayerController playerController; 
    public GameObject player;
    public Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    public void HoldState()
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
            
            if(!activated)
            {
                var rigidbody = heldObject.GetComponent<Rigidbody>();
                rigidbody.drag = 1f;
                rigidbody.useGravity = true;
                heldObject = null;
            }
        }
        else
        {
            if (activated)
            {
                var hits = Physics.SphereCastAll(t.position + t.forward, radius, t.forward, radius);
                var hitIndex = Array.FindIndex(hits, hit => hit.transform.tag == "PickUp");

                if (hitIndex != -1)
                {
                    var hitObject = hits[hitIndex].transform.gameObject;
                    heldObject = hitObject;
                    var rigidbody = heldObject.GetComponent<Rigidbody>();
                    rigidbody.drag = 25f;
                    rigidbody.useGravity = false;
                }
            }
        else
        {
          
        }
        }
    }

    void FixedUpdate()
    {
        var t = playerTransform;

        if(heldObject)
        {
            var rigidbody = heldObject.GetComponent<Rigidbody>();
            var moveTo = t.position + distance * t.forward + height * t.up;
            var difference = moveTo - heldObject.transform.position;
            rigidbody.AddForce(difference * 500);
            heldObject.transform.rotation = t.rotation;
        }
    }

    //public void Interaction(InputAction.CallbackContext context)
    //{
        //HoldState();
        //if(!context.started) return;
        //Debug.Log("Interact");
    //}

    

    

}

