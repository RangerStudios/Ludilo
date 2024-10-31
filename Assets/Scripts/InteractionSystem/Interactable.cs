using UnityEngine;
using UnityEngine.Events;


public class Interactable : MonoBehaviour
{
   
    public UnityEvent OnInteract;

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

}
