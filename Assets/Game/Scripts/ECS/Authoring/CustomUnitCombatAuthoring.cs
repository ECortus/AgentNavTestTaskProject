using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace ECS.Test
{
    public class CustomUnitCombatAuthoring : MonoBehaviour
    {
        public float Damage = 10;
        public float AttackRange = 0.3f;
        public float AggresionRadius = 5f;
    }
    
    public class UnitCombatAuthoringBaker : Baker<CustomUnitCombatAuthoring>
    {
        public override void Bake(CustomUnitCombatAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CustomUnitCombat
            {
                Range = authoring.AttackRange,
                AggressionRadius = authoring.AggresionRadius,
                Cooldown = 0.2f,
                Speed = 0.5f,
                Damage = authoring.Damage
            });
            AddComponent(entity, new CustomUnitFollow { });
        }
    }
}