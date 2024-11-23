using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggTimer : MonoBehaviour
{
    public static Action<Transform> EggTurnOn;
    public static Action EggTurnOff;

    public WindupInteract attachedWindup;

    public float timer = 15f;
    public bool activeBool;

    [SerializeField]private float currentTimer;

    public void Awake()
    {
        attachedWindup = GetComponentInChildren<WindupInteract>();
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
    }

    public void DeactivateEgg()
    {
        activeBool = false;
        currentTimer = timer;
        EggTurnOff();
        attachedWindup.gameObject.SetActive(true);
        attachedWindup.returnKey();
    }
}
