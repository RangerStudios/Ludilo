using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DustExplode : MonoBehaviour
{
    public float explodeTimer = 0.5f;
    float currentTimer;
    public static Action DustPlayer;

    public void OnEnable()
    {
        currentTimer = explodeTimer;
    }
    public void Update()
    {
        currentTimer -= Time.deltaTime;
        if (currentTimer <= 0)
        {
            this.gameObject.SetActive(false);
            this.transform.parent.gameObject.SetActive(false);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by Dust!");
            DustPlayer();
        }
    }
}
