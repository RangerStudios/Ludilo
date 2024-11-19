using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class DustBunny : MonoBehaviour, IDamageable
{
    //Reference to explode effect object. Enables it when the bunny explodes.
    public GameObject explodeEffect;
    //Reference to behavior object. Disables when explosion happens.
    public GameObject behavior;
    //Enemy Health component. Used to deal damage to the enemy when they take it.
    public HealthController EnemyHealth;

    //Signals to the health controller that damage has been taken.
    public void Damage(int damageValue)
    {
        EnemyHealth.Damage(damageValue);
    }

    //Called when the health controller says the enemy has died. Primarily here to disable enemy when they die, but can be used on a by-type basis.
    public void Die()
    {
        Debug.Log("Enemy Dies");
        this.gameObject.SetActive(false);
    }

    //Has the enemy activate it's explosion and disable it's behavior.
    public void ProtoExplode()
    {
        Debug.Log("Exploded into dust!");
        behavior.SetActive(false);
        explodeEffect.SetActive(true);
    }

    
}
