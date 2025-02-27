using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Ladder : MonoBehaviour
{

    PlayerController player;
    bool playerPresent;
    [SerializeField] Transform startPosition;
    [SerializeField] Transform endPosition;

    void OnEnable()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;

        PlayerInput.onInteract += MountLadder;
    }

    void OnDisable()
    {
        PlayerInput.onInteract -= MountLadder;
    }

    private void MountLadder()
    {
        if(playerPresent)
        {
            player.OnLadder(startPosition.position, this);
            playerPresent = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player = other.GetComponent<PlayerController>();
            if(player != null && !player.onLadder)
            {
                playerPresent= true;
            }
        }
    }

    public Vector3 GetEndPosition()
    {
        return endPosition.position;
    }


}
