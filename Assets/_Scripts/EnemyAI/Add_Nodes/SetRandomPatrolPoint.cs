using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

// Based on SetPatrolPoint that comes with MBT. Sets a random point within the array instead of a set order.

[MBTNode("Custom Leaves/Set Random Patrol Point")]
[AddComponentMenu("")]
public class SetPatrolPoint : Leaf
{
    public TransformReference variableToSet = new TransformReference(VarRefMode.DisableConstant);
    public Transform[] waypoints;
    private int index = 0;
    public override NodeResult Execute()
    {
    if (waypoints.Length == 0)
        {
            return NodeResult.failure;
        }
        // Random value from 0 to array length - 1.
        index = Random.Range(0, waypoints.Length - 1);
         
        // Set blackboard variable with need waypoint (position)
        variableToSet.Value = waypoints[index];
        return NodeResult.success;
    }
}
