using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DustBunny : MonoBehaviour
{
    public GameObject explodeEffect;
    public GameObject behavior;
    public void ProtoExplode()
    {
        Debug.Log("Exploded into dust!");
        behavior.SetActive(false);
        explodeEffect.SetActive(true);
    }
}
