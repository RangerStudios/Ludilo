using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToySoldier : MonoBehaviour, IDamageable
{
    //Reference to behavior, in case we need to disable it in this script.
    public GameObject behaviorObject;
    //Reference to bullet prefab, so that it can be instantiated when necessary.
    public GameObject bulletPrefab;
    //Bullet Speed.
    public float bulletSpeed = 7f;
    //Enemy Health Component. Used to deal damage to the enemy.
    public HealthController EnemyHealth;

    //Signals to the health controller that the enemy has taken damage.
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

    //Has enemy fire a bullet. Called by the Behavior Tree.
    public void Shoot()
    {
        Debug.Log("Fire!");
        GameObject bullet = GameObject.Instantiate(bulletPrefab, this.transform.position + transform.forward, transform.localRotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = bullet.transform.forward * bulletSpeed;
    }
}
