using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    //Events other scripts can invoke for GameManager access
    public static Action GamePause;
    public static Action GameUnpause;
    public static Action GameQuit;

    public Action<int> AddNumber;
    //Events the Game Manager sends out

    public static Action<RespawnCondition> SpawnPlayer;

    public static Action<bool> OnGamePause;
    public enum GameState{
        Running,
        Paused,
        Cutscene
    }

    bool gameIsPaused = false;

    public static GameState currentGameState;
    public GameState previousGameState;

    public GameObject player;

    void OnEnable()
    {
        //player.GetComponent<PlayerController>().OnDeath += PauseGame;
        //Plsyer.OnDeath += OnPlayerDied;
        GamePause += PauseGame;
        GameUnpause += UnpauseGame;
        GameQuit += QuitGame;
    }
    void OnDisable()
    {
        GamePause -= PauseGame;
        GameUnpause -= UnpauseGame;
        GameQuit -= QuitGame;
    }

    void Start()
    {
        SpawnPlayer(RespawnCondition.GAMESTART);
    }

    
    //Set to public so it can be called be triggers placed in the map
    //Can be set to additive so the loading is seemless between areas if needed
    public void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    void PauseGame()
    {
        gameIsPaused = true;
        OnGamePause?.Invoke(gameIsPaused);
        Time.timeScale = 0.0f;
    }
    void UnpauseGame()
    {
        gameIsPaused = false;
        OnGamePause?.Invoke(gameIsPaused);
        Time.timeScale = 1.0f;
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

    public void PlayerFell()
    {
        SpawnPlayer(RespawnCondition.FALL);
    }

    public void PlayerDied()
    {
        SpawnPlayer(RespawnCondition.DEATH);
    }
    
}
