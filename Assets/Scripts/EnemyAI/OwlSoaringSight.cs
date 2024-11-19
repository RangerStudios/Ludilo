using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlSoaringSight : MonoBehaviour
{
    // Bool of whether the owl sees the player
    public bool seesBool;
    // Reference for Owl transform.
    public Transform self;
    // Layermask for what the Raycast interacts with.
    public LayerMask sightMask;
    // Variable for how long timer should last.
    public float timer = 5f;
    
    // Private variable for current timer.
    [SerializeField]private float currentTimer;

    // Update timer over time. 

    // As long as player remains in the sight and is hit by the raycast, set seesBool to true
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Waow");
            // Shoot Raycast to other
            // if it connects, set seen bool to true
            // if not, set bool to false.
        }
    }

    public void SeesPlayer()
    {
        // sets bool to true
    }

    public void DoesntSeePlayer()
    {
        // if not already false, refreshes timer
        // sets bool to false
    }

    public void OwlGrab()
    {
        // Owl Gets You, call game manager for PlayerFell.
    }
}
