using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    [SerializeField] private Animator falling = null;

    [SerializeField] private bool fallTrigger = false;
    //[SerializeField] private bool 

    [SerializeField] private string BoxPile1 = "fall";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fallTrigger)
            {
                falling.Play(BoxPile1, 0, 0);
                gameObject.SetActive(false);
            }
        }
    }
    
}
