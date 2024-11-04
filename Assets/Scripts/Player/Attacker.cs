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
        //Animation here?? Idk
        if (enemyNumFound > 0)
        {
            Debug.Log("EnemyHit");
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
