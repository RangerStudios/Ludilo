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

    private readonly Collider[] colliders = new Collider[1];
    [SerializeField] public int numFound;
    public Transform interactablePos;
    [SerializeField] float interactableDistanceFloat;


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
        
        
    }

    public void Interaction()
    {
        if(numFound > 0 && playerController.canInteract)
        {
            var interactable = colliders[0].GetComponent<Interactable>();
            Transform interactablePos = interactable.transform;
            float interactDist = Vector3.Distance(interactionPoint.position, interactablePos.position);

            if (interactable != null && interactDist < interactableDistanceFloat)
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

