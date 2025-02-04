using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MBT;

[AddComponentMenu("")]
[MBTNode("Custom Leaves/Move Tethered NavAgent")]
public class MoveTetheredNavmeshAgent : Leaf
{
    public TransformReference destination;
    public TransformReference tetherPoint;
    public FloatReference tetherRange;
    public NavMeshAgent agent;
    public float stopDistance = 2f;
    [Tooltip("How often target position should be updated")]
    public float updateInterval = 1f;
    private float time = 0;

    public override void OnEnter()
    {
        time = 0;
        agent.isStopped = false;
        agent.SetDestination(ClampPosition(destination.Value.position));
    }
        
    public override NodeResult Execute()
    {
        time += Time.deltaTime;
        // Update destination every given interval
        if (time > updateInterval)
        {
            // Reset time and update destination
            time = 0;
            agent.SetDestination(ClampPosition(destination.Value.position));
        }
        // Check if path is ready
        if (agent.pathPending)
        {
            return NodeResult.running;
        }
        // Check if agent is very close to destination
        if (agent.remainingDistance < stopDistance)
        {
            return NodeResult.success;
        }
        // Check if there is any path (if not pending, it should be set)
        if (agent.hasPath)
        {
            return NodeResult.running;
        }
        // By default return failure
        return NodeResult.failure;
    }

    public override void OnExit()
    {
        agent.isStopped = true;
        // agent.ResetPath();
    }

    public override bool IsValid()
    {
        return !(destination.isInvalid || agent == null);
    }

    public Vector3 ClampPosition(Vector3 position)
    {
        Vector3 newPosition = Vector3.zero;
        newPosition.x = Mathf.Clamp(position.x, tetherPoint.Value.position.x - tetherRange.Value, tetherPoint.Value.position.x + tetherRange.Value);
        newPosition.z = Mathf.Clamp(position.x, tetherPoint.Value.position.z - tetherRange.Value, tetherPoint.Value.position.z + tetherRange.Value);
        return newPosition;
    }
}
