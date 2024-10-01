using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    void OnTriggerStay (Collider other)
    {
        if(other.gameObject.GetComponent<HealthController>() != null)
        {
            other.gameObject.GetComponent<HealthController>().Damage();
        }
    }
}
