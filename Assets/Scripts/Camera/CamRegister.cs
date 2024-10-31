using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamRegister : MonoBehaviour
{
    private void OnEnable()
    {
        CamSwitcher.Register(GetComponent<CinemachineVirtualCamera>());
    }

    private void OnDisable()
    {
        CamSwitcher.UnRegister(GetComponent<CinemachineVirtualCamera>());
    }
}
