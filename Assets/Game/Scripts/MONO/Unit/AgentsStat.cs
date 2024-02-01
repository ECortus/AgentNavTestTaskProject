using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AgentsStat : MonoBehaviour
{
    public static Action OnUpdate { get; set; }
    
    [Inject] public static AgentsStat Instance { get; private set; }
    [Inject] void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        GameManager.OnGameStart += Init;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStart -= Init;
    }

    public int EnemyAlive { get; private set; }
    public void AddEnemyAlive()
    {
        EnemyAlive++;
        
        OnUpdate?.Invoke();
    }
    
    public int EnemyKilled { get; private set; }
    public void AddEnemyKilled()
    {
        EnemyAlive--;
        EnemyKilled++;
        
        OnUpdate?.Invoke();
    }
    
    public int AllyAlive { get; private set; }
    public void AddAllyAlive()
    {
        AllyAlive++;
        
        OnUpdate?.Invoke();
    }
    
    public int AllyKilled { get; private set; }
    public void AddAllyKilled()
    {
        AllyAlive--;
        AllyKilled++;
        
        OnUpdate?.Invoke();
    }
    
    void Init()
    {
        EnemyAlive = 0;
        EnemyKilled = 0;
        AllyAlive = 0;
        AllyKilled = 0;
        
        OnUpdate?.Invoke();
    }
}
