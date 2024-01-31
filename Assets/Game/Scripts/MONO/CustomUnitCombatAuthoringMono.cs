using System.Collections;
using System.Collections.Generic;
using ECS.Test;
using ProjectDawn.Navigation.Hybrid;
using ProjectDawn.Navigation.Sample.Zerg;
using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(CustomUnitAuthoringMono))]
public class CustomUnitCombatAuthoringMono : MonoBehaviour
{
    public float Damage = 10;
    public float AttackRange = 0.3f;
    public float AggresionRadius = 5;

    Entity m_Entity;

    void Awake()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        m_Entity = GetComponent<AgentAuthoring>().GetOrCreateEntity();
        world.EntityManager.AddComponentData(m_Entity, new CustomUnitCombat
        {
            Range = AttackRange,
            AggressionRadius = AggresionRadius,
            Cooldown = 0.2f,
            Speed = 0.5f,
            Damage = Damage,
        });
        world.EntityManager.AddComponentData(m_Entity, new CustomUnitFollow {} );
    }

    void OnDestroy()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        if (world != null)
        {
            world.EntityManager.RemoveComponent<CustomUnitCombat>(m_Entity);
            world.EntityManager.RemoveComponent<CustomUnitFollow>(m_Entity);
        }
    }
}
