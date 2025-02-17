using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderExit : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerController>()?.ExitLadder();
        }
    }
}
