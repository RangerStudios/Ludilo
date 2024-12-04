using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    //Base script to handling shifting between menus while the game is paused

    //Continue
    //Options
    //Main Menu
    //Quit

    public static Action<string> TryLoadNewScene;


    public void Awake()
    {
        GameManager.GameUnpause?.Invoke();
    }

    void SwitchMenu()
    {
        //Disables the current menu
        //Activates new menu
        //Sets the focus target for new menu for UI control
    }


    public void GoToMainMenu()
    {
        //Popup for "Are you sure?" before heading to main menu

        //Should be an event so the player's game is saved whenever they go back to menu or quit the game
        //Event OnReturnToMenu >> Store most recent checkpoint
        GameManager.Instance.LoadScene("MainMenu");
    }

    public void TryLoadScene(string sceneToLoad)
    {
        GameManager.Instance.LoadScene(sceneToLoad);
    }

    //Code for adding tabs to a given menu as children of the parent menu
    public Transform GetNextMenuTab(Transform currentMenu)
    {
        if(currentMenu == null)
        {
            //If there is not a valid point, default to the starting waypoint
            return transform.GetChild(0);
        }

        if(currentMenu.GetSiblingIndex() < transform.childCount -1)
        {
            //If there are still waypoints in the object, get the next one in series through the siblings
            return transform.GetChild(currentMenu.GetSiblingIndex()+1);  
        }
        else
        {
            //If all else fails, default to the first waypoint in the index
            return transform.GetChild(0);
        }
    }
    public Transform GetPreviousMenuTab(Transform currentMenu)
    {
        if(currentMenu == null)
        {
            //If there is not a valid point, default to the starting waypoint
            return transform.GetChild(0);
        }

        if(currentMenu.GetSiblingIndex() > 0)
        {
            //If there are still waypoints in the object, get the next one in series through the siblings
            return transform.GetChild(currentMenu.GetSiblingIndex()-1);  
        }
        else
        {
            //If all else fails, default to the first waypoint in the index
            return transform.GetChild(transform.childCount-1);
        }
    }

    /*TODO: Implement a proper failsafe for getting out of any menu with Escape*/
    void ForceCloseMenus()
    {
        //Immediately closes the given menu and unpauses the game
    }


    
}
