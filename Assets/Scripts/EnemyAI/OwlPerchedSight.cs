using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OwlPerchedSight : MonoBehaviour
{
    public static Action<Transform> OnSeePlayer;
    public static Action OnNotSeePlayer;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnSeePlayer(other.transform);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnNotSeePlayer();
        }
    }
}
