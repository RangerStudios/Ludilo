using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindupKeyPickup : Interactable
{
    [SerializeField] bool activated = false;
    [SerializeField] public bool isHeld;
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
                Physics.IgnoreLayerCollision(9, 6, false);
                heldObject = null;
            }
        }
        else
        {
            if (activated && playerController.isHoldingItem == false)
            {
                var hits = Physics.SphereCastAll(t.position + t.forward, radius, t.forward, radius);
                var windUpHitIndex = Array.FindIndex(hits, hit => hit.transform.tag == "WindupKey");
                
                if (windUpHitIndex != -1)
                {
                    var hitObject = hits[windUpHitIndex].transform.gameObject;
                    heldObject = hitObject;
                    var rigidbody = heldObject.GetComponent<Rigidbody>();
                    rigidbody.drag = 25f;
                    rigidbody.useGravity = false;
                    Physics.IgnoreLayerCollision(9, 6, true);
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
            isHeld = true;
            var rigidbody = heldObject.GetComponent<Rigidbody>();
            var moveTo = t.position + distance * t.forward + height * t.up;
            var difference = moveTo - heldObject.transform.position;
            rigidbody.AddForce(difference * 800);
            heldObject.transform.rotation = t.rotation;
            playerController.isHoldingItem = true;
            activated = true;
        }
        else
        {
            isHeld = false;
        }
    }

    public void DropSmallItem()
    {
        if(heldObject)
        {
            activated = false;
            playerController.isHoldingItem = false;
        }
    }
}
