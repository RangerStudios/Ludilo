using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OwlPerched : MonoBehaviour
{
    //Bool for whether the Owl is looking at the player.
    public bool isLookingAtPlayer;
    //Bool for whether the Owl is looking at the baby owl.
    public bool isLookingAtBabyOwl;
    //Float value to initialize timer with.
    public float timer = 5f;
    //Object for Sight zone. Used to disable after baby owl is activated.
    public GameObject sightZone;
    //Float value for current time on timer.
    [SerializeField]private float currentTimer;
    //Transform of player, grabbed when first looking at player.
    private Transform playerTransform;
    //Transform of baby owl to look at. Set when baby owl is activated by the player.
    private Transform eggTransform;
    //Reference to Game Manager, set on start. For respawning the player after looking at them long enough.
    private GameManager gameManager;
    public void OnEnable()
    {
        OwlPerchedSight.OnSeePlayer += SeesPlayer;
        OwlPerchedSight.OnNotSeePlayer += CantSeePlayer;
        EggTimer.EggTurnOn += SeesEgg;
        EggTimer.EggTurnOff += CantSeeEgg;
    }

    public void OnDisable()
    {
        OwlPerchedSight.OnSeePlayer -= SeesPlayer;
        OwlPerchedSight.OnNotSeePlayer -= CantSeePlayer;
        EggTimer.EggTurnOn -= SeesEgg;
        EggTimer.EggTurnOff -= CantSeeEgg;
    }

    public void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public void Update()
    {
        if (isLookingAtPlayer)
        {
            this.transform.LookAt(playerTransform);
            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0f)
            {
               OwlAttack();
            }
        }
    }

    //Sets player transform reference, sets timer to be started and isLooking to true.
    public void SeesPlayer(Transform seenTransform)
    {
        Debug.Log("Sees Player!");
        playerTransform = seenTransform;
        currentTimer = timer;
        isLookingAtPlayer = true;
    }

    //Sets isLooking to false and resets local rotation of object.
    public void CantSeePlayer()
    {
        Debug.Log("Lost Track of Player!");
        isLookingAtPlayer = false;
        this.transform.localRotation = Quaternion.identity;
    }

    public void OwlAttack()
    {
        gameManager.PlayerFell();
        CantSeePlayer();
        Debug.Log("The Owl Got You!");
    }

    //Sets look point to baby owl.
    public void SeesEgg(Transform seenTransform)
    {
        Debug.Log("Sees Egg, disabled.");
        eggTransform = seenTransform;
        this.transform.LookAt(eggTransform);
        sightZone.SetActive(false);
    }

    public void CantSeeEgg()
    {
        CantSeePlayer();
        sightZone.SetActive(true);
    }
}
