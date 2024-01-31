using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject stopText;

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
        OnGameStart();
    }

    void OnGameStart()
    {
        GameStarted = true;
        
        AgentsCountersMono.Init();
        
        EnemySpawner.Instance.Activate();
        AllySpawner.Instance.Activate();
    }

    public void StopGame()
    {
        OnStopGame();
    }

    void OnStopGame()
    {
        EnemySpawner.Instance.Deactivate();
        AllySpawner.Instance.Deactivate();
        
        stopText.SetActive(true);

        GameStarted = false;
    }
}
