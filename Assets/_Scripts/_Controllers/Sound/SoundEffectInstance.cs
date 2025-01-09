using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectInstance : MonoBehaviour
{
    public SoundEffectInstance instance;

    [SerializeField] private AudioSource soundFXObject;

    public void Awake()
    {
        instance = this; 
    }

    public void PlaySoundClip(AudioClip audioclip, Transform spawnTransform, float volume)
    {
        //spawn object
        AudioSource audiosource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        //assign clip
        audiosource.clip = audioclip;
        //assign volume
        audiosource.volume = volume;
        //play sound
        audiosource.Play();
        //get length of sound
        float clipsLength = audiosource.clip.length;
        // destroy object
        Destroy(audiosource.gameObject, clipsLength);
    }
}
