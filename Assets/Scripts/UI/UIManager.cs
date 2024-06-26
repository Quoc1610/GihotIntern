using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIOnlineLobby uiOnlineLobby;
    public UIMainMenu uiMainMenu;
    public static UIManager _instance { get; private set; }


    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
    private void Awake()
    {
        uiOnlineLobby.OnSetUp();
        uiMainMenu.OnSetUp();
        _instance = GameObject.FindAnyObjectByType<UIManager>();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        // Debug.Log(Player_ID.MyPlayerID);
    }

    public void OnClose_Clicked()
    {
        this.gameObject.SetActive(false);
    }
}