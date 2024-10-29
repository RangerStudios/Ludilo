using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    public int health;
    public bool canHeal;
    public UnityEvent OnDeath;
    public UnityEvent OnHeal;
    public StuffingController stuffingController;
    

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
        OnDeath.Invoke();
    }

    public void Heal()
    {
        if(canHeal == true)
        {
            this.health += GetComponent<StuffingController>().stuffingCount;
            OnHeal.Invoke();
            canHeal = false;
        }
    }
}
