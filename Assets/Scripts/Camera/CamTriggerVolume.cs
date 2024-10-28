using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements;

//[RequireComponent(typeof(BoxCollider))]
//[RequireComponent(typeof(Rigidbody))]
public class CamTriggerVolume : TriggerVolume
{
    [SerializeField] private CinemachineVirtualCamera cam;


    //BoxCollider box;
    Rigidbody rb;



    protected override void Awake()
    {
        //box = GetComponent<BoxCollider>();
        //rb = GetComponent<Rigidbody>();
        //box.isTrigger = true;
        base.Awake();
        //rb.isKinematic = true;
    }



    public override void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            if (CamSwitcher.ActiveCamera != cam) CamSwitcher.SwitchCamera(cam);
            //cam.Follow = player
            //cam.LookAt = player
        }
    }
}
