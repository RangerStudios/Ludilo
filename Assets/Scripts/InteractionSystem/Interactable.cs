using UnityEngine;
using UnityEngine.Events;


public class Interactable : MonoBehaviour, IPlaySounds
{
   
    public UnityEvent OnInteract;

    public void Awake()
    {
        AudioSource audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
    }

    protected bool Interact()
    {
        OnInteract?.Invoke();
        return true;
    }

    public virtual bool Interact(Interactor interactor)
    {
        Interact();
        return true; 
    }

    public void PlaySoundEffect(AudioClip soundEffect)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(soundEffect);
    }
}
