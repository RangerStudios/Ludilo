using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{


    public static Action<bool> OnGamePause;
    public enum GameState{
        Running,
        Paused,
        Cutscene
    }

    bool gameIsPaused;

    public static GameState currentGameState;
    public GameState previousGameState;
    
    //Set to public so it can be called be triggers placed in the map
    //Can be set to additive so the loading is seemless between areas if needed
    public void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    void PauseGame()
    {
        
    }
    void UnpauseGame()
    {

    }

    void QuitGame()
    {
        Application.Quit();
    }
    public void ChangeGameState(GameState newState)
    {
        previousGameState = currentGameState;
        switch(newState)
        {
            case GameState.Paused:
            //Pause the game when the state is changed
                PauseGame();
            //Send a signal to any listeners that care about the game being paused
                
            break;
            case GameState.Cutscene:
            break;
            case GameState.Running:
            //If the game was paused, it is no longer paused
                UnpauseGame();
                
            break;
        }

        currentGameState = newState;
    }


    
}
