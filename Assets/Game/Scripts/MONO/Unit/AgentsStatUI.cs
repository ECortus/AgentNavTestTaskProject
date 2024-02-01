using System;
using System.Collections;
using System.Collections.Generic;
using ECS.Test;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AgentsStatUI : MonoBehaviour
{
    [SerializeField] private Text allyAlive, allyKilled, enemyAlive, enemyKilled;
    
    [Inject] private void Awake()
    {
        AgentsStat.OnUpdate += Refresh;
    }

    private void OnDestroy()
    {
        AgentsStat.OnUpdate -= Refresh;
    }

    private void Refresh()
    {
        allyAlive.text = AgentsStat.Instance.AllyAlive.ToString();
        allyKilled.text = AgentsStat.Instance.AllyKilled.ToString();
        enemyAlive.text = AgentsStat.Instance.EnemyAlive.ToString();
        enemyKilled.text = AgentsStat.Instance.EnemyKilled.ToString();
    }
}
