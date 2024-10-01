using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    //Base script to handling shifting between menus while the game is paused

    //Continue
    //Options
    //Main Menu
    //Quit



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




    
}
