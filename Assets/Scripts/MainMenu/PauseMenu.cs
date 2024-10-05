using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    Button[] buttons;

   private void OnEnable()
   {
        GameManager.OnGamePause += UpdatePauseMenu;
   }
   private void OnDisable()
   {
        GameManager.OnGamePause -= UpdatePauseMenu;
   }

   void Awake()
   {
        buttons = GetComponentsInChildren<Button>();
        
   }


   public void UpdatePauseMenu(bool gamePaused)
   {
        gameObject.SetActive(gamePaused);        
        Cursor.visible = gamePaused;
        if(gamePaused)
        {
            buttons[0].Select();
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
   }

   public void ResumeGame()
   {
        GameManager.GameUnpause.Invoke();
   }
}
