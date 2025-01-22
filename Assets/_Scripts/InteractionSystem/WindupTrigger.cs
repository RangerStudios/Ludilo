using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WindupTrigger : MonoBehaviour
{
    public UnityEvent OnKey;
    protected BoxCollider box;

    protected virtual void Awake()
    {
        box = GetComponent<BoxCollider>();
        box.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WindupKey"))
        {
            var windupKeyScript = other.GetComponent<WindupKeyPickup>();
            if(!windupKeyScript.isHeld)
            {
                Debug.Log("Key Inserted, activating event");
                other.gameObject.SetActive(false);
                OnKey?.Invoke();
            }
        }
    }
}
