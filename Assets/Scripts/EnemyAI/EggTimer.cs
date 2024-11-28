using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggTimer : MonoBehaviour, IDamageable
{
    // Action that is sent out when the Egg activates. Sends the transform of the egg for the perched owl.
    public static Action<Transform> EggTurnOn;
    // Action that is sent out when the Egg deactivates.
    public static Action EggTurnOff;

    // Reference to attached windup trigger. Used to reactivate it after Egg is deactivated.
    public WindupInteract attachedWindup;
    // Reference to health controller. Used to damage the egg when attacked while activated, and make sure it can't be damaged when disabled.
    public HealthController eggHealth;

    // Float for the timer value that gets set to currentTimer.
    public float timer = 15f;
    // Boolean for whether the egg is activated.
    public bool activeBool;

    // Float for the timer. Runs down while Egg is activated, reset when deactivated.
    [SerializeField]private float currentTimer;

    // Sets the two references on Awake.
    public void Awake()
    {
        attachedWindup = GetComponentInChildren<WindupInteract>();
        eggHealth = GetComponent<HealthController>();
    }

    // Sets timer on Start.
    public void Start()
    {
        currentTimer = timer;
    }

    // Runs timer down and deactivates egg when it reaches 0.
    public void Update()
    {
        if (activeBool)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0f)
            {
                DeactivateEgg();
            }
        }
    }

    // Run when the Egg is actiivated by the windup script.
    public void ActivateEgg()
    {
        activeBool = true;
        EggTurnOn(this.transform);
        eggHealth.health = 1;
    }

    // Run when the Egg is deactivated by the timer or is hit while activated.
    public void DeactivateEgg()
    {
        activeBool = false;
        currentTimer = timer;
        EggTurnOff();
        attachedWindup.gameObject.SetActive(true);
        attachedWindup.returnKey();
    }

    // Run when the Egg is attacked. Ensures that when the egg has no health.
    public void Damage(int damageValue)
    {
        if (eggHealth.health > 0)
        {
            eggHealth.Damage(damageValue);
        }
        
    }
}
