using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class PlayerInput : MonoBehaviour, PlayerControls.IPlayerActions
{
    private PlayerControls playerControls;
    public static UnityAction<Vector2> onMove = delegate { };
    public static UnityAction onJump = delegate { };
    public static UnityAction onCrouch = delegate { };
    public static UnityAction onRagdoll = delegate { };
    public static UnityAction onInteract = delegate { };
    public static UnityAction onPause = delegate { };
    public static UnityAction onHeal = delegate { };
    public static UnityAction onAttack = delegate { };

    void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.Player.SetCallbacks(this);
        }
        playerControls.Player.Enable();
    }

    void OnDisable()
    {
        playerControls.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        onMove(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            onJump();
        }
        if (!context.started) return;
    }
    
    public void OnRun(InputAction.CallbackContext context)
    {

    }
    
    public void OnInteract(InputAction.CallbackContext context)
    {

    }
    
    public void OnRagdoll(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            onRagdoll();
        }
        if(!context.started) return;
    }
    
    public void OnHeal(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            onHeal();
        }
        if(!context.started) return;
    }
    
    public void OnPause(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            onPause();
        }
        if(!context.started) return;
    }
    
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            onCrouch();
        }
    }
    
    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            onAttack();
        }
        if(!context.started) return;
    }
}
