using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

public class Amalgam : MonoBehaviour, ILureable
{
    // Reference to the Amalgam's behavior tree blackboard. Assign in Unity Editor.
    public Blackboard amalgamBlackboard;

    // When lured, set the blackboard's lureTarget to the transform of the lure.
    public void OnLure(Transform lurePosition)
    {
        amalgamBlackboard.GetVariable<TransformVariable>("lureTarget").Value = lurePosition;
    }

    // Removes lureTarget reference, so that it is not set.
    public void Unlure()
    {
        amalgamBlackboard.GetVariable<TransformVariable>("lureTarget").Value = null;
    }
}
