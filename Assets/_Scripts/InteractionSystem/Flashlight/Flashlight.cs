using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MediumItemPickup, IActivatable
{
    public bool canBeActivated { get; set; }

    bool isOn;

    [Header("Flashlight")]
    [SerializeField]
    Light theLight;

    [Tooltip("Radius of the light cone")]    
    [SerializeField]
    float beamRadius;
    [SerializeField]
    int enemiesLayer;


    [Header("Sound Effects")]
    [SerializeField] AudioClip clickOn;
    [SerializeField] AudioClip clickOff;

    Ray lightBeam;
    RaycastHit hit;





    public void OnEnable()
    {

        if ((theLight == null))
        {
            theLight = GetComponent<Light>();
        }

        theLight.enabled = false;

        //Player Input for Activation
        PlayerInput.onAttack += Activate;
    }
    
    public void Activate()
    {
        //Code goes into here to 
        //Activate 
        if(heldObject)
        {
            
            if(isOn)
            {
                TurnOffFlashlight();
            }
            else
            {
                TurnOnFlashlight();
            }
        }
        else
        {
            return;
        }
        
    }

    public void Deactivate()
    {
        //Handle what happens when flashlight turns off
        //Disable the dynamic light
    }

    void FixedUpdate()
    {
        //If the flashlight is enabled
            //Shoot out a raycast and get the point of contact.
        if(isOn)
        {
            //Creating the physical shape that will check for enemies
            //Enemies layer is 7
            RaycastHit[] hits=Physics.CapsuleCastAll(theLight.transform.position, hit.point, beamRadius, transform.forward, 200, enemiesLayer);

            foreach(RaycastHit hit in hits)
            {
                IFlashable flashable = hit.collider.GetComponent<IFlashable>();
                flashable?.Flash();
            }
        }

    }

    void TurnOnFlashlight()
    {
        //Create these when the flashlight is turned on
        //Store this point in the world and use it a base for creating the checking cylinder and lure spot
        lightBeam = new Ray(theLight.transform.position, transform.forward);
        Physics.Raycast(lightBeam, out hit);
        theLight.enabled = true;
        isOn = true;
    }

    void TurnOffFlashlight()
    {
        theLight.enabled = false;
        isOn = false;
    }

    
}
