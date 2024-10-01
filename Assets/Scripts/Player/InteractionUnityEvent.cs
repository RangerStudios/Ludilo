using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionUnityEvent : MonoBehaviour
{
    [SerializeField] UnityEvent interaction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerController player))
        {
            player.OnInteraction += InteractionEvent;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out PlayerController player))
        {
            player.OnInteraction -= InteractionEvent;
        }
    }

    private void InteractionEvent()
    {
        interaction?.Invoke();
    }
}
