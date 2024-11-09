using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToySoldier : MonoBehaviour
{
    bool lookingAtPlayer = false;
    Transform player;
    public GameObject behaviorObject;

    public void OnEnable()
    {
        ToySoldierAttack.OnSeePlayer += LookActive;
        ToySoldierAttack.OnPlayerNotSeen += LookInactive;
    }
    public void OnDisable()
    {
        ToySoldierAttack.OnSeePlayer -= LookActive;
        ToySoldierAttack.OnPlayerNotSeen -= LookInactive;
    }

    public void Update()
    {
        if (lookingAtPlayer)
        {
            transform.LookAt(player);
        }
    }

    public void LookActive(Transform seenPlayer)
    {
        player = seenPlayer;
        behaviorObject.SetActive(false);
        lookingAtPlayer = true;
    }

    public void LookInactive()
    {
        lookingAtPlayer = false;
        behaviorObject.SetActive(true);
    }
}
