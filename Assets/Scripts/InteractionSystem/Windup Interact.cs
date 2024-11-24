using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WindupInteract : MonoBehaviour
{
    // Reference to the key that has been inserted.
    public GameObject insertedKey;
    // Reference to the egg that this Interact is attached to.
    public EggTimer egg;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WindupKey"))
        {
            var windupKeyScript = other.GetComponent<WindupKeyPickup>();
            if(!windupKeyScript.isHeld)
            {
                Debug.Log("Inserted Key");
                insertedKey = other.gameObject;
                insertedKey.SetActive(false);
                this.gameObject.SetActive(false);
                egg.ActivateEgg();
            }
        }
    }

    public void returnKey()
    {
        insertedKey.transform.position += transform.forward * 2;
        insertedKey.SetActive(true);
    }
}