using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public  GameObject menu;
    [SerializeField] AudioClip selectSound;
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
        buttons = menu.GetComponentsInChildren<Button>();
        foreach(Button button in buttons)
        {
            var pointerEvents = button.GetComponent<PointerEventsController>();

            if(pointerEvents == null)
            {
                Debug.LogWarning(button.name + " does not have a PointerEventsController. Cannot connect events.");
                continue;
            }
            //Subscribe all the buttons when they click to the AudioManager.Instance to play a oneshot of the selectSound
            //pointerEvents.PointerEnter
            

        }
        
   }


   public void UpdatePauseMenu(bool gamePaused)
   {
        menu.SetActive(gamePaused);        
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

   public void BackToMenu()
   {
     SceneManager.LoadScene("MainMenu");
   }

   public void Quit()
   {
     GameManager.GameQuit.Invoke();
   }
}
