using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

[AddComponentMenu("")]
[MBTNode("Custom Service/Enemy Sight Service")]
public class EnemySightService : Service
{
    [Tooltip("Layer mask to determine what is interacted with.")]
    public LayerMask rangeMask = -1;

    [Tooltip("Layer mask for raycast.")]
    public LayerMask sightMask = -1;

    [Tooltip("Radius in which enemy will eject raycast when entered.")]
    public float range = 15f;

    [Tooltip("Transform variable that gets set by this. If a target is set, then it will sed back to blackboard.")]
    public TransformReference targetToSet = new TransformReference(VarRefMode.DisableConstant);
    
    [Tooltip("Transform representing enemy self.")]
    public TransformReference self = new TransformReference();

    public override void Task()
    {
        // Find Target in radius. Send target transform back to blackboard as long as raycast to target is not interrupted by wall.
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, rangeMask, QueryTriggerInteraction.Ignore);
        if (colliders.Length > 0)
        {
            var direction = (colliders[0].transform.position - self.Value.transform.position).normalized;
            var ray = new Ray(self.Value.transform.position, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, sightMask))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    targetToSet.Value = colliders[0].transform;
                    Debug.Log("Set");
                }
                else
                {
                    targetToSet.Value = null;
                    Debug.Log("Cannot See");
                }
            }
        }
        else
        {
            targetToSet.Value = null;
            Debug.Log("Cannot Find");
        }

    }
}
