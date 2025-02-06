using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceScale : MonoBehaviour
{

    //in order for this to work, we need to first get the value of everything that is in range of the body

    //This is the list that stores how many rigidbodies are on the scale
    List<Rigidbody> bodiesOnScale = new();

    //We connect the scale to the two platforms to deal with the logic, like so
    //ScalePlatform -> Get the collective mass of all objects on the scale (store as a float value)
    //Left scale, right scale

    float leftScaleMass, rightScaleMass;


    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CalculateScaleForces()
    {
        //We find the acceleration, following a = g(mass2 - mass1) / (mass2 + mass1)
        //If this value is positive, this means the RIGHT scale is going down and the left scale is increasing, so we should always make 1 negative
        float scaleAcceleration = Physics.gravity.y*(rightScaleMass - leftScaleMass) / (rightScaleMass + leftScaleMass);


        //ScalePlatform.GetComponent<Rigidbody>().AddForce(Vector3.down * rightScaleMass * scaleAcceleration);

        //Copy for other platform, to create inverse relationship
        
        //ScalePlatform.GetComponent<Rigidbody>().AddForce(Vector3.down * rightScaleMass * scaleAcceleration);
    }

    
}
