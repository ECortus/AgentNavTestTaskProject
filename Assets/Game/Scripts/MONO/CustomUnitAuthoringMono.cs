using System.Collections;
using System.Collections.Generic;
using ECS.Test;
using ProjectDawn.Navigation.Hybrid;
using ProjectDawn.Navigation.Sample.Zerg;
using Unity.Entities;
using UnityEngine;

public class CustomUnitAuthoringMono : MonoBehaviour
{
    public CustomUnitId Owner;
    public bool PlayerFriendly;
    
    [Space]
    public float Life = 100;

    Entity m_Entity;

    void Awake()
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
    }

    void OnDestroy()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        if (world != null)
        {
            world.EntityManager.RemoveComponent<CustomUnit>(m_Entity);
            world.EntityManager.RemoveComponent<CustomUnitBrain>(m_Entity);
            world.EntityManager.RemoveComponent<CustomUnitLife>(m_Entity);
        }
    }
}
