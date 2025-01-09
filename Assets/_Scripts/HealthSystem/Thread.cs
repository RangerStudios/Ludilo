using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thread : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<HealthController>() != null)
        {
            other.gameObject.GetComponent<HealthController>().canHeal = true;
            Debug.Log("Thread Gained");
            Destroy(transform.parent.gameObject);
        }
    }
}
