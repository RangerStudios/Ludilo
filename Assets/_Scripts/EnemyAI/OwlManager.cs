using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Manages the clock room's various owl objects. Only one is meant to be active at a time.
public class OwlManager : MonoBehaviour
{
    // The array of all Owls in the scene. Set in inspector.
    public GameObject[] owls;

    public void Start()
    {
        enableOwl(0);
    }

    // Enables the owl in the specified index, while disabling all others.
    public void enableOwl(int owlIndex)
    {
        if (owls.Length > 0)
        {
            for (int i = 0; i < owls.Length; i++)
            {
                if (i == owlIndex)
                {
                    owls[i].gameObject.SetActive(true);
                }
                else
                {
                    owls[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.Log("No Owls Found! Got all your owls in a row? Wait, wrong fowl...");
        }
    }
}
