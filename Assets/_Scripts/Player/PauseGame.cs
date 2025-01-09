using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    void OnEnable()
    {
        PlayerInput.onPause += PauseActivate;
    }

    void OnDisable()
    {
        PlayerInput.onPause -= PauseActivate;
    }

    void Start()
    {
        //GameObject.Find("GameManager");
    }
    public void PauseActivate()
    {
        GameManager.GamePause?.Invoke();
    }
}
