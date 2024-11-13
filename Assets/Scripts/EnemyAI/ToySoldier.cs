using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToySoldier : MonoBehaviour
{
    //Reference to behavior, in case we need to disable it in this script.
    public GameObject behaviorObject;
    //Reference to bullet prefab, so that it can be instantiated when necessary.
    public GameObject bulletPrefab;

    public void DebugShoot()
    {
        Debug.Log("Fire!");
        //Fires bullet prefab.
    }
}
