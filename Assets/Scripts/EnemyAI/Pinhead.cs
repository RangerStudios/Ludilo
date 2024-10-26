using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pinhead : MonoBehaviour, IDamageable
{
    public UnityEvent<int> onGrab;
    public UnityEvent onRelease;
    public HealthController EnemyHealth;
    public GameObject behaviorObject;
    public GameObject player;
    public float grabTimer = 5f;
    public float waitTimer = 3f;
    private float currentTimer;
    private bool hasGrabbed = false;
    private bool hasReleased = false;

    void Update()
    {
        if (hasGrabbed)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer < 0)
            {
                onRelease.Invoke();
            }
        }

        if (hasReleased)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer < 0)
            {
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
        onGrab.Invoke(1);
    }

    public void Grab()
    {
        Debug.Log("Grabbed!");
        currentTimer = grabTimer;
        hasGrabbed = true;
        behaviorObject.SetActive(false);
        player.GetComponent<PlayerController>().Damage(1);
        this.gameObject.transform.SetParent(player.transform);
    }

    public void Release()
    {
        Debug.Log("Released!");
        currentTimer = waitTimer;
        hasGrabbed = false;
        hasReleased = true;
        this.gameObject.transform.SetParent(null);
    }
}
