using UnityEngine;
using UnityEngine.Events;


public class Interactable : MonoBehaviour
{
   
    public UnityEvent OnInteract;

    protected void Interact()
    {
        OnInteract?.Invoke();
    }

    public virtual void Interact(Interactor interactor)
    {
        Interact();
    }

}
