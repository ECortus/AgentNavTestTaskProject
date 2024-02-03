using System;
using System.Collections;
using System.Collections.Generic;
using ECS.Test;
using ProjectDawn.Navigation.Hybrid;
using ProjectDawn.Navigation.Sample.Zerg;
using Unity.Entities;
using UnityEngine;

public class UnitAuthoring : MonoBehaviour
{
    public Action OnSpawn { get; set; }
    public Action OnDeath { get; set; }
    public void Spawn() => OnSpawn?.Invoke();
    public void Death() => OnDeath?.Invoke();
    
    public UnitId Owner;
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
        world.EntityManager.AddComponentData(m_Entity, new Unit
        {
            Owner = Owner,
            PlayerFriendly = PlayerFriendly
        });
        world.EntityManager.AddComponentData(m_Entity, new UnitBrain
        {
            State = UnitBrainState.Idle
        });
        world.EntityManager.AddComponentData(m_Entity, new UnitLife
        {
            Life = Life,
            MaxLife = Life
        });

        if (Owner == UnitId.Player)
        {
            world.EntityManager.AddComponentData(m_Entity, new PlayerTag { });
        }
        
        Spawn();
    }
    
    private Player Player => GameManager.Instance.Player;
    private Player Opponent => GameManager.Instance.Opponent;

    void AddAliveStat()
    {
        switch (Owner)
        {
            case UnitId.Ally:
                Player.AddAlive();
                break;
            case UnitId.Enemy:
                Opponent.AddAlive();
                break;
        }
    }

    void AddKillStat()
    {
        switch (Owner)
        {
            case UnitId.Ally:
                Player.AddKilled();
                break;
            case UnitId.Enemy:
                Opponent.AddKilled();
                break;
        }
    }

    void OnDestroy()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        if (world != null)
        {
            world.EntityManager.RemoveComponent<Unit>(m_Entity);
            world.EntityManager.RemoveComponent<UnitBrain>(m_Entity);
            world.EntityManager.RemoveComponent<UnitLife>(m_Entity);
        }
    }
}
