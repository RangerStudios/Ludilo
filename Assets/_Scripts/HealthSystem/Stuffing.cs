using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stuffing : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<StuffingController>() != null)
        {
            other.gameObject.GetComponent<StuffingController>().stuffingCount++;
            Debug.Log("Stuffing Gained");
            Destroy(transform.parent.gameObject);
        }
    }
}
