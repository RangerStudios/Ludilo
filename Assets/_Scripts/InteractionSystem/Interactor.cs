using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask interactableMask;
    private PlayerController playerController;
    //bool interacting;

    private readonly Collider[] colliders = new Collider[3];
    [SerializeField] public int numFound;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    void OnEnable()
    {
        PlayerInput.onInteract += Interaction;
    }

    void OnDisable()
    {
        PlayerInput.onInteract -= Interaction;
    }

    private void Update()
    {
        numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, colliders, interactableMask);

        if(numFound > 0)
        {
            //Debug.Log("Hold J to Interact");
        }
    }

    public void Interaction()
    {
        if(numFound > 0 && playerController.canInteract)
        {
            var interactable = colliders[0].GetComponent<Interactable>();

            if (interactable != null)
            {
                interactable.Interact(this);
            } 
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}

