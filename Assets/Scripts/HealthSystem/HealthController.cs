using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    public int health;
    public UnityEvent onDeath;

    public void Damage()
    {
        this.health--;
        if (this.health <= 0)
        {
            Kill();
        }
    }

    public void Damage(int damageValue)
    {
        this.health -= damageValue;
        if (this.health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        onDeath.Invoke();
    }
}
