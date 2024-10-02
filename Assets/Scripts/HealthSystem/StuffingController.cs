using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffingController : MonoBehaviour
{
    public int stuffingCount;
    public GameObject stuffing;

    public void SpawnStuffing(int s)
    {
        for (int i = 0; i < s; i++)
        {
            Vector3 randomForce = Vector3.zero;
            int r = Random.Range(0, 4);
            if (r == 0)
            {
                randomForce = Vector3.forward;
            }
            else if (r == 1)
            {
                randomForce = Vector3.back;
            }
            else if (r == 2)
            {
                randomForce = Vector3.left;
            }
            else if (r == 3)
            {
                randomForce = Vector3.right;
            }
            var instance = Instantiate(stuffing, transform.position + (randomForce * 1.1f), Quaternion.identity);
            instance.GetComponent<Rigidbody>().velocity = (randomForce * 5f) + (Vector3.up * 2f);
        }
    }
}
