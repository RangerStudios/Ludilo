using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// "Weeping Angel" style enemy. Chases the player when not shined with the flashlight. Attacks when close enough.

public class Nutcracker : MonoBehaviour, IFlashable
{
    // Reference to behavior object. Behavior has enemy move towards the player.
    public GameObject behaviorObject;
    // Reference to attack object. Briefly enabled in order to attack player.
    public GameObject attackObject;

    // Bools for states. One for while the enemy is attacking, one for while the enemy is in cooldown.
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool hasAttacked;

    // Timer float that ticks up towards a specified float value.
    [SerializeField] private float currentTimer = 0f;
    // Float representing the amount of time the attack hitbox should be active, in seconds, when attacking.
    public float attackTimeLength = 0.5f;
    // Float representing the amount of time, in seconds, the enemy will remain in action cooldown from attacking.
    public float attackCooldown = 4f;

    // Ticks down a respective timer based on the current state boolean.
    void Update()
    {
        if (isAttacking)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer >= attackTimeLength)
            {
                EnterCooldown();
            }
        }
        else if (hasAttacked)
        {
            currentTimer += Time.deltaTime;
            if  (currentTimer >= attackCooldown)
            {
                ExitCooldown();
            }
        }
    }

    // Enemy is lit up by Flashlight. Stop its movements. Only works when enemy is not attacking or in cooldown.
    public void Flash()
    {
        if (!isAttacking && !hasAttacked)
        {
            behaviorObject.SetActive(false);
        }
    }

    // Enemy is no longer lit by flashlight. Have it continue moving. Only works when enemy is not attacking or in cooldown.
    public void UnFlash()
    {
        //behaviorObject.SetActive(true);
        if (!isAttacking && !hasAttacked)
        {
            behaviorObject.SetActive(true);
        }
    }

    // Enemy is close enough to player. Enables "isAttacking" to attack and stops moving.
    public void Attack()
    {
        behaviorObject.SetActive(false);
        attackObject.SetActive(true);
        ResetTimer();
        isAttacking = true;
    }

    // Enemy has finished attacking. Stop attacking, enter cooldown.
    public void EnterCooldown()
    {
        attackObject.SetActive(false);
        ResetTimer();
        isAttacking = false;
        hasAttacked = true;
    }

    // Enemy has finished cooldown. Resume normal action.
    public void ExitCooldown()
    {
        hasAttacked = false;
    }

    // Reset timer to zero.
    public void ResetTimer()
    {
        currentTimer = 0f;
    }
}
