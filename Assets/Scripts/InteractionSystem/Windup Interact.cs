using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WindupInteract : MonoBehaviour
{
    [SerializeField] public GameObject windupKey;
    [SerializeField] public WindupKeyPickup windupKeyScript;
    void Awake()
    {
        windupKey = GameObject.FindWithTag("WindupKey");
        windupKeyScript = windupKey.GetComponent<WindupKeyPickup>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WindupKey"))
        {
            if(!windupKeyScript.isHeld)
            {
                Debug.Log("Inserted Key");
            }
            else
            {
                
            }
        }
    }
}