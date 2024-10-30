using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

[AddComponentMenu("")]
[MBTNode("Custom Service/Enemy Home In Service")]
public class EnemyHomeInService : Service
{
    public TransformReference target;

    public override void Task()
    {
        target.Value = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
