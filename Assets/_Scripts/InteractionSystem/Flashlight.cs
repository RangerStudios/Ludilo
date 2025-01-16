using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MediumItemPickup, IActivatable
{
    public bool canBeActivated { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    [Header("Flashlight")]
    [SerializeField]
    Light theLight;


    [Header("Sound Effects")]
    [SerializeField] AudioClip clickOn;
    [SerializeField] AudioClip clickOff;



    public void OnEnable()
    {

        if ((theLight == null))
        {
            theLight = GetComponent<Light>();
        }

    }
    public void Activate()
    {
        //Code goes into here to 
        //Activate 
        theLight.enabled = true;
    }

    public void Deactivate()
    {
        //Handle what happens when flashlight turns off
        //Disable the dynamic light
        theLight.enabled = false;
    }

    
}
