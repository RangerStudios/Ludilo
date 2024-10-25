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

        //Code for if the droppable object for disabling the trap touches the trap goes here. 
    }

    public void Snap()
    {
        //Play sound
        //Play animation on parent object
        this.transform.parent.gameObject.SetActive(false);
    }
}
