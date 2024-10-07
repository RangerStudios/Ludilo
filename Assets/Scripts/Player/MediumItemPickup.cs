using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumItemPickup : MonoBehaviour
{
  [SerializeField] bool activated = false;
    public GameObject heldObject;
    public float radius = 2f;
    public float distance = 1.4f;
    //public float height = 0.3f;
    public PlayerController playerController;    
    
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void DragState()
    {
        activated = !activated;
    }

    // Update is called once per frame
    void Update()
    {
    
        var t = transform;
        if(heldObject)
        {
            heldObject.transform.position = t.position + distance * t.forward;
            playerController.isDragging = true;
            playerController.speed = 2;
            playerController.rotationSpeed = 50f;
            if(!activated)
            {
                playerController.isDragging = false;
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
                var hitIndex = Array.FindIndex(hits, hit => hit.transform.tag == "MediumPickUp");

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
}
