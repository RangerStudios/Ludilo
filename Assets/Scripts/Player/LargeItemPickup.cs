using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LargeItemPickup : MonoBehaviour
{
public bool activated;
    public GameObject heldObject;
    public float radius;
    public float distance;
    //public float height = 0.3f;
    public PlayerController playerController;    
    
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void DragState()
    {
        if(activated == false)
        {
            activated = true;
        }
        else
        {
            activated = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
    
        var t = transform;
        if(heldObject)
        {
            
            playerController.isDraggingLarge = true;
            playerController.speed = 1;
            playerController.rotationSpeed = 250f;
            if(!activated)
            {
                var rigidbody = heldObject.GetComponent<Rigidbody>();
                playerController.isDraggingLarge = false;
                rigidbody.drag = 1f;
                rigidbody.useGravity = true;
                rigidbody.constraints = RigidbodyConstraints.None;
                heldObject = null;
                playerController.speed = 5;
                playerController.rotationSpeed = 500f;
            }
        }
        else
        {
            if (activated)
            {
                var hits = Physics.SphereCastAll(t.position + t.forward, radius, t.forward, radius);
                var hitIndex = Array.FindIndex(hits, hit => hit.transform.tag == "LargePickUp");

                if (hitIndex != -1)
                {
                    var hitObject = hits[hitIndex].transform.gameObject;
                    heldObject = hitObject;
                    var rigidbody = heldObject.GetComponent<Rigidbody>();
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    rigidbody.drag = 25f;
                    rigidbody.useGravity = false;
                }
            }
        else
        {
          
        }
        }
    }

    private void FixedUpdate()
    {
        var t = transform;
        if(heldObject)
        {
            var rigidbody = heldObject.GetComponent<Rigidbody>();
            var moveTo = t.position + distance * t.forward;
            var difference = moveTo - heldObject.transform.position;
            rigidbody.AddForce(difference * 500);
            heldObject.transform.rotation = t.rotation;
        }
    }

    public void Interaction(InputAction.CallbackContext context)
    {
        DragState();
        if(!context.started) return;
        Debug.Log("Interact");
    }
}