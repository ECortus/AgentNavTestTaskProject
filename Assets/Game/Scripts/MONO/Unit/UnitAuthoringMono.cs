using System;
using System.Collections;
using System.Collections.Generic;
using ECS.Test;
using ProjectDawn.Navigation.Hybrid;
using ProjectDawn.Navigation.Sample.Zerg;
using Unity.Entities;
using UnityEngine;

public class UnitAuthoringMono : MonoBehaviour
{
    public Action OnSpawn { get; set; }
    public Action OnDeath { get; set; }
    
    public CustomUnitId Owner;
    public bool PlayerFriendly;
    
    [Space]
    public float Life = 100;

    Entity m_Entity;

    private void Awake()
    {
        OnSpawn += AddAliveStat;
        OnDeath += AddKillStat;
    }

    void Start()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        m_Entity = GetComponent<AgentAuthoring>().GetOrCreateEntity();
        world.EntityManager.AddComponentData(m_Entity, new CustomUnit
        {
            Owner = Owner,
            PlayerFriendly = PlayerFriendly
        });
        world.EntityManager.AddComponentData(m_Entity, new CustomUnitBrain
        {
            State = CustomUnitBrainState.Idle
        });
        world.EntityManager.AddComponentData(m_Entity, new CustomUnitLife
        {
            Life = Life,
            MaxLife = Life
        });

        if (Owner == CustomUnitId.Player)
        {
            world.EntityManager.AddComponentData(m_Entity, new PlayerTag { });
        }
        
        OnSpawn?.Invoke();
    }

    void AddAliveStat()
    {
        switch (Owner)
        {
            case CustomUnitId.Ally:
                AgentsStat.Instance.AddAllyAlive();
                break;
            case CustomUnitId.Enemy:
                AgentsStat.Instance.AddEnemyAlive();
                break;
            default:
                break;
        }
    }

    void AddKillStat()
    {
        switch (Owner)
        {
            case CustomUnitId.Ally:
                AgentsStat.Instance.AddAllyKilled();
                break;
            case CustomUnitId.Enemy:
                AgentsStat.Instance.AddEnemyKilled();
                break;
            default:
                break;
        }
    }

    void OnDestroy()
    {
        OnDeath?.Invoke();
        
        var world = World.DefaultGameObjectInjectionWorld;
        if (world != null)
        {
            world.EntityManager.RemoveComponent<CustomUnit>(m_Entity);
            world.EntityManager.RemoveComponent<CustomUnitBrain>(m_Entity);
            world.EntityManager.RemoveComponent<CustomUnitLife>(m_Entity);
        }
    }
}
