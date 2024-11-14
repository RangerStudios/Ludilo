using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToySoldierAttack : MonoBehaviour
{
    public static Action<Transform> OnSeePlayer;
    public static Action OnPlayerNotSeen;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnSeePlayer(other.gameObject.transform);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnPlayerNotSeen();
        }
    }
}
