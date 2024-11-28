using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script handles swapping the behavior of the Soaring Owl based on the Egg Timer.
public class OwlSoaring : MonoBehaviour
{
    public GameObject normalSoaring;
    public GameObject alertedSoaring;
    public void OnEnable()
    {
        EggTimer.EggTurnOn += EnableAlertedSoaring;
        EggTimer.EggTurnOff += EnableNormalSoaring;
    }

    public void OnDisable()
    {
        EggTimer.EggTurnOn -= EnableAlertedSoaring;
        EggTimer.EggTurnOff -= EnableNormalSoaring;
    }

    public void EnableNormalSoaring()
    {
        alertedSoaring.SetActive(false);
        normalSoaring.SetActive(true);
    }

    public void EnableAlertedSoaring(Transform eggTransform)
    {
        alertedSoaring.SetActive(true);
        normalSoaring.SetActive(false);
    }

}
