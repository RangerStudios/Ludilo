using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTest : MonoBehaviour
{
    [SerializeField] bool activated = false;
    //[SerializeField] GameObject ball;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
    {
        gameObject.SetActive(false);
    }
    else
    {
        gameObject.SetActive(true);   
    }
    }

    public void BallState()
    {
        activated = !activated;
    }

    
}
