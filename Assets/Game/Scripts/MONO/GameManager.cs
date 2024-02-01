using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] public static GameManager Instance { get; private set; }

    public static Action OnGameStart { get;  set; }
    public static Action OnGameStop { get; set; }
    
    public static bool GameStarted { get; private set; }

    [Inject] private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        GameStart();
    }

    void GameStart()
    {
        if (GameStarted) return;
        GameStarted = true;
        
        OnGameStart?.Invoke();
    }

    public void StopGame()
    {
        if (!GameStarted) return;
        GameStarted = false;
        
        OnGameStop?.Invoke();
    }
}
