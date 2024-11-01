using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Pinhead : MonoBehaviour, IDamageable
{
    public UnityEvent onGrab;
    public UnityEvent onRelease;
    public static Action GrabPlayer;
    public static Action ReleasePlayer;
    public HealthController EnemyHealth;
    public GameObject behaviorObject;
    public GameObject player;
    public NavMeshAgent agent;
    public float grabTimer = 5f;
    public float waitTimer = 3f;
    public float currentGrabTimer;
    public float currentWaitTimer;
    private bool hasGrabbed = false;
    private bool hasReleased = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (hasGrabbed)
        {
            currentGrabTimer -= Time.deltaTime;
            if (currentGrabTimer <= 0)
            {
                onRelease.Invoke();
            }
        }

        if (hasReleased)
        {
            currentWaitTimer -= Time.deltaTime;
            if (currentWaitTimer <= 0)
            {
                hasReleased = false;
                behaviorObject.SetActive(true);
            }
        }
    }

    public void Damage(int damageValue)
    {
        EnemyHealth.Damage(damageValue);
    }

    public void Die()
    {
        this.gameObject.SetActive(false);
    }

    public void GrabAction()
    {
        onGrab.Invoke();
    }

    public void Grab()
    {
        Debug.Log("Grabbed!");
        GrabPlayer();
        currentGrabTimer = grabTimer;
        hasGrabbed = true;
        behaviorObject.SetActive(false);
        player.GetComponent<PlayerController>().Damage(1);
        this.gameObject.transform.SetParent(player.transform);
    }

    public void Release()
    {
        Debug.Log("Released!");
        ReleasePlayer();
        currentWaitTimer = waitTimer;
        hasGrabbed = false;
        hasReleased = true;
        this.gameObject.transform.SetParent(null);
    }

    public void ManualMove(Transform moveSpot)
    {
        agent.isStopped = false;
        agent.SetDestination(moveSpot.position);
    }
}
