using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

// Edit of MoveTowards from the example package, to use a transform instead of a vector3.

[MBTNode("Custom Leaves/Move To Transform")]
[AddComponentMenu("")]
public class MoveToTransform : Leaf
{
    public TransformReference targetPosition;
    public TransformReference transformToMove;
    public float speed = 0.1f;
    public float minDistance = 0f;

    public override NodeResult Execute()
    {
        Transform target = targetPosition.Value;
        Transform obj = transformToMove.Value;
        // Move as long as distance is greater than min. distance
        float dist = Vector3.Distance(target.position, obj.position);
        if (dist > minDistance)
        {
            // Move towards target
            obj.position = Vector3.MoveTowards(
                obj.position, 
                target.position, 
                (speed > dist)? dist : speed 
            );
            return NodeResult.running;
        }
        else
        {
            return NodeResult.success;
        }
    }
}
