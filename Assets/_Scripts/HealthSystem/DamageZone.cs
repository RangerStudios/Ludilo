using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.GetComponent<IDamageable>() != null && other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IDamageable>().Damage(1);
        }
    }
}
