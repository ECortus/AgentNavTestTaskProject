using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[DefaultExecutionOrder(-1000)]
public class GameManager : MonoBehaviour
{
    [Inject] public static GameManager Instance { get; private set; }

    public Action OnGameStart { get;  set; }
    public Action OnGameStop { get; set; }
    public bool GameStarted { get; private set; }

    [field: SerializeField] public Player Player { get; private set; }
    [field: SerializeField] public Player Opponent { get; private set; }
    
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
