using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RespawnTest : MonoBehaviour
{
    public UnityEvent<RespawnCondition> onRespawn;
    public RespawnManager respawnManager;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            onRespawn?.Invoke(RespawnCondition.FALL);
            other.gameObject.GetComponent<IDamageable>().Damage(1);
        }
    }
}
