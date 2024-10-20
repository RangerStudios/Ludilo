using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code by Anthony C.
//Apply this script to the player for picking up small items
public class PickUpTest : MonoBehaviour
{
    [SerializeField] bool activated = false;
    public GameObject heldObject;
    public float radius = 2f;
    public float distance = 1.2f;
    public float height = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void HoldState()
    {
        activated = !activated;
    }

    // Update is called once per frame
    void Update()
    {
    
        var t = transform;
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

    

    

}

