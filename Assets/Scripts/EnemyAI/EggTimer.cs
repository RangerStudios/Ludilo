using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggTimer : MonoBehaviour, IDamageable
{
    public static Action<Transform> EggTurnOn;
    public static Action EggTurnOff;

    public WindupInteract attachedWindup;
    public HealthController eggHealth;

    public float timer = 15f;
    public bool activeBool;

    [SerializeField]private float currentTimer;

    public void Awake()
    {
        attachedWindup = GetComponentInChildren<WindupInteract>();
        eggHealth = GetComponent<HealthController>();
    }

    public void Start()
    {
        currentTimer = timer;
    }

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

    public void ActivateEgg()
    {
        activeBool = true;
        EggTurnOn(this.transform);
        eggHealth.health = 1;
    }

    public void DeactivateEgg()
    {
        activeBool = false;
        currentTimer = timer;
        EggTurnOff();
        attachedWindup.gameObject.SetActive(true);
        attachedWindup.returnKey();
    }

    public void Damage(int damageValue)
    {
        if (eggHealth.health > 0)
        {
            eggHealth.Damage(damageValue);
        }
        
    }
}
