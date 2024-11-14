using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToySoldier : MonoBehaviour
{
    //Reference to behavior, in case we need to disable it in this script.
    public GameObject behaviorObject;
    //Reference to bullet prefab, so that it can be instantiated when necessary.
    public GameObject bulletPrefab;
    //Bullet Speed.
    public float bulletSpeed = 7f;

    public void DebugShoot()
    {
        Debug.Log("Fire!");
        GameObject bullet = GameObject.Instantiate(bulletPrefab, this.transform.position + transform.forward, transform.localRotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = bullet.transform.forward * bulletSpeed;
    }
}
