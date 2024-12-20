using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attacker : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackPointSize = 0.5f;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private LayerMask saveMask;

    private readonly Collider[] colliders = new Collider[3];
    [SerializeField] private int saveNumFound;
    [SerializeField] private int enemyNumFound;

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    void Update()
    {
        saveNumFound = Physics.OverlapSphereNonAlloc(attackPoint.position, attackPointSize, colliders, saveMask);
        enemyNumFound = Physics.OverlapSphereNonAlloc(attackPoint.position, attackPointSize, colliders, enemyMask);

    }

    public void AttackCheck()
    {
        if (enemyNumFound > 0)
        {
            var damageable = colliders[0].GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(1);
            }
        }

        if (saveNumFound > 0)
        {
            Debug.Log("SavePointHit");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackPointSize);
    }


}
