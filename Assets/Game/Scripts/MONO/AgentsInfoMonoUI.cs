using System;
using System.Collections;
using System.Collections.Generic;
using ECS.Test;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AgentsInfoMonoUI : MonoBehaviour
{
    [Inject] public static AgentsInfoMonoUI Instance { get; private set; }
    
    [SerializeField] private Text allyAlive, allyKilled, enemyAlive, enemyKilled;

    [Inject] private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Refresh()
    {
        allyAlive.text = AgentsCountersMono.AllyAlive.ToString();
        allyKilled.text = AgentsCountersMono.AllyKilled.ToString();
        enemyAlive.text = AgentsCountersMono.EnemyAlive.ToString();
        enemyKilled.text = AgentsCountersMono.EnemyKilled.ToString();
    }
}
