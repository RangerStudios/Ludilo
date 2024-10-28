using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(BoxCollider))]
public class TriggerVolume : Interactable
{

    public UnityAction<Collider> OnEnterTrigger;
    public UnityAction<Collider> OnExitTrigger;
    BoxCollider box;

    void Awake()
    {
        box = GetComponent<BoxCollider>();
        box.isTrigger = true;
    }


    public virtual void OnTriggerEnter(Collider collider)
    {
        OnEnterTrigger?.Invoke(collider);
    }
    public virtual void OnTriggerExit(Collider collider)
    {
        OnExitTrigger?.Invoke(collider);
    }

}
