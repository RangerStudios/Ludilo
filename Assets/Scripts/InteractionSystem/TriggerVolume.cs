using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(BoxCollider))]
public class TriggerVolume : Interactable
{

    public UnityEvent<Collider> OnEnterTrigger;
    public UnityEvent<Collider> OnExitTrigger;
    protected BoxCollider box;
    [SerializeField] private Vector3 boxSize;

    protected virtual void Awake()
    {
        box = GetComponent<BoxCollider>();
        box.isTrigger = true;
        box.size = boxSize;
    }


    public virtual void OnTriggerEnter(Collider collider)
    {
        OnEnterTrigger?.Invoke(collider);
    }
    public virtual void OnTriggerExit(Collider collider)
    {
        OnExitTrigger?.Invoke(collider);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, boxSize); 
    }

}
