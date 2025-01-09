using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

[AddComponentMenu("")]
[MBTNode("Custom Service/Enemy Sees Service")]
public class EnemySeesService : Service
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

    public override void Task()
    {
        // Find target in radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, rangeMask, QueryTriggerInteraction.Ignore);
        if (colliders.Length > 0)
        {
            // Set target
            target.Value = colliders[0].transform;
            // Set up raycast to target
            var direction = (target.Value.transform.position - self.Value.transform.position).normalized;
            var ray = new Ray(self.Value.transform.position, direction);
            RaycastHit hit;
            // If raycast hits, check facing direction of enemy
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, sightMask))
            {
                float dotProduct = Vector3.Dot(self.Value.transform.forward, direction);
                Debug.Log(dotProduct);
                // If enemy can properly see player, dot product is greater than 0
                if (dotProduct > 0)
                {
                    seenBool.Value = true;
                }
                else
                {
                    seenBool.Value = false;
                }
            }
        }
        else
        {
            seenBool.Value = false;
        }
    }
}
