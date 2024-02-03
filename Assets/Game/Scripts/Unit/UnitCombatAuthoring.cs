using System.Collections;
using System.Collections.Generic;
using ECS.Test;
using ProjectDawn.Navigation.Hybrid;
using ProjectDawn.Navigation.Sample.Zerg;
using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(UnitAuthoring))]
public class UnitCombatAuthoring : MonoBehaviour
{
    public float Damage = 10;
    public float AttackRange = 0.3f;
    public float AggresionRadius = 5;

    Entity m_Entity;

    void Start()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        m_Entity = GetComponent<AgentAuthoring>().GetOrCreateEntity();
        world.EntityManager.AddComponentData(m_Entity, new UnitCombat
        {
            Range = AttackRange,
            AggressionRadius = AggresionRadius,
            Cooldown = 0.2f,
            Speed = 0.5f,
            Damage = Damage,
        });
        world.EntityManager.AddComponentData(m_Entity, new UnitFollow {} );
    }

    void OnDestroy()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        if (world != null)
        {
            world.EntityManager.RemoveComponent<UnitCombat>(m_Entity);
            world.EntityManager.RemoveComponent<UnitFollow>(m_Entity);
        }
    }
}
