using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivatable
{
    public bool canBeActivated { get; set; }
    /*Activate the object*/
    public void Activate();
    public void Deactivate();

    
}
