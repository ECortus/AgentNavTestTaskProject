using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] public static GameManager Instance { get; private set; }

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
        AgentsCountersMono.Init();
        
        EnemySpawner.Instance.Activate();
        AllySpawner.Instance.Activate();
    }
}
