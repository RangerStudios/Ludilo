using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class CamTriggerVolume : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private Vector3 boxSize;

    BoxCollider box;
    Rigidbody rb;



    private void Awake()
    {
        box = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();

        box.isTrigger = true;
        box.size = boxSize;

        rb.isKinematic = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, boxSize); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (CamSwitcher.ActiveCamera != cam) CamSwitcher.SwitchCamera(cam);
        }
    }
}
