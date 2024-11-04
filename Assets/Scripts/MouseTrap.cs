using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTrap : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IDamageable>().Damage(1);
            Snap();
        }

        if (other.CompareTag("PickUp"))
        {
            this.transform.parent.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 3f);
            this.gameObject.SetActive(false);
        }
    }

    public void Snap()
    {
        //Play sound
        //Play animation on parent object
        this.transform.gameObject.SetActive(false);
    }
}
