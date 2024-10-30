using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

[AddComponentMenu("")]
[MBTNode("Custom Service/Enemy Seen Service")]
public class EnemySeenService : Service
{
    [Tooltip("Layer mask to determine what is interacted with.")]
    public LayerMask rangeMask = -1;

    [Tooltip("Layer mask for raycast.")]
    public LayerMask sightMask = -1;

    [Tooltip("Radius in which enemy will eject raycast when entered.")]
    public float range = 15f;

    [Tooltip("Bool variable that is true when enemy is seen.")]
    public BoolReference seenBool;
    
    [Tooltip("Transform representing enemy self.")]
    public TransformReference self = new TransformReference();

    [Tooltip("Enemy target.")]
    public TransformReference target;

    /*[Tooltip("Angle at which the player can 'see' the enemy.")]
    public float viewAngle = 60f;*/

    public override void Task()
    {
        target.Value = GameObject.FindGameObjectWithTag("Player").transform;
        var direction = (target.Value.transform.position - self.Value.transform.position).normalized;
        var ray = new Ray(self.Value.transform.position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, sightMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                float dotProduct = Vector3.Dot(hit.transform.forward, direction);
                if (dotProduct < 0)
                {
                    seenBool.Value = true;
                    Debug.Log("Set");
                }
                else
                {
                    seenBool.Value = false;
                    //Debug.Log("Enenmy sees, but not player.");
                }
            }
            else
            {
                seenBool.Value = false;
                //Debug.Log("Enemy Cannot See");
            }
        }
    }
}
