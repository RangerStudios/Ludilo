using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{

    void Start()
    {
        //GameObject.Find("GameManager");
    }
    public void PauseActivate()
    {
        GameManager.Instance.GamePause.Invoke();
    }
}
