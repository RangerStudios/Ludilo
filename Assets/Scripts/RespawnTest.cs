using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RespawnTest : MonoBehaviour
{
    public UnityEvent onRespawn;
    public RespawnTracker respawnTracker;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            onRespawn?.Invoke();
            other.gameObject.GetComponent<IDamageable>().Damage(1);
        }
    }
}
