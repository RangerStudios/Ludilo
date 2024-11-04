using System;
using System.Collections;
using System.Collections.Generic;
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
            heldObject.transform.position = t.position + distance * t.forward + height * t.up;
            if(!activated)
            {
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
                }
            }
        else
        {
          
        }
        }
    }

    //public void Interaction(InputAction.CallbackContext context)
    //{
        //HoldState();
        //if(!context.started) return;
        //Debug.Log("Interact");
    //}

    

    

}

