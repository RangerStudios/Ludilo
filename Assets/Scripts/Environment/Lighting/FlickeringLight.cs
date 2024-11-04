using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickeringLight : MonoBehaviour
{
    Light propLight;

    [SerializeField] FlickerType flickerType;

    [Tooltip("Max wait time for light, in seconds")]
    [SerializeField] float maxWait = 1;
    [Tooltip("Max duration for light flicker, in seconds")]
    [SerializeField] float maxFlicker = 0.2f;
    
    float timer;
    bool isOn;
    float delay;
    float defaultIntensity;

    [Space(10)]
    [Header("Flame flicker style")]
    [SerializeField] float maxDisplacement = 0.25f;
    Vector3 targetPosition;
    Vector3 lastPosition;
    Vector3 origin;

    float targetIntensity;
    float lastIntensity;
    bool isFlickering;
    //Section for the Flame style flickering


    void Awake()
    {
        propLight = GetComponent<Light>();
        defaultIntensity = propLight.intensity;

        origin = transform.position;
        lastPosition = origin;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        switch(flickerType)
        {
            case FlickerType.Electrical:
                if(timer > delay)
                {
                    ToggleLight();
                }

            break;
            case FlickerType.Flame:
            if(timer > delay)
            {
                SetFlameIntensity();
                
            }
            propLight.intensity = Mathf.Lerp(lastIntensity, targetIntensity, timer / delay);
                propLight.transform.position = Vector3.Lerp(lastPosition, targetPosition, timer / delay);
            break;
            case FlickerType.Burst:
            break;
        }

        
    }


    void ToggleLight()  
    {

        isOn = !isOn;

        if(isOn)
        {
            propLight.intensity = defaultIntensity;
            delay = Random.Range(0, maxWait);
        }
        else
        {
            propLight.intensity = Random.Range(0.6f, defaultIntensity);
            delay = Random.Range(0f, maxFlicker);
        }

        timer = 0;
    
    }

    void SetFlameIntensity()
    {
        lastIntensity = propLight.intensity;
        targetIntensity = Random.Range(0.5f, 1f);
        timer = 0;
        delay = Random.Range(0, maxWait);

        targetPosition = origin + Random.insideUnitSphere * maxDisplacement;
        lastPosition = propLight.transform.position;
    }

}


public enum FlickerType{
    Electrical,
    Flame,
    Burst
}
