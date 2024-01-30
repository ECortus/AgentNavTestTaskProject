using System.Collections;
using System.Collections.Generic;
using ProjectDawn.Navigation.Hybrid;
using ProjectDawn.Navigation.Sample.Zerg;
using Unity.Entities;
using UnityEngine;

namespace ECS.Test
{
    public class CustomUnitAuthoring : MonoBehaviour
    {
        public CustomUnitId Owner;
        public bool PlayerFriendly;
        
        [Space]
        public float Life = 100;
    }

    public class UnitAuthoringBaker : Baker<CustomUnitAuthoring>
    {
        public override void Bake(CustomUnitAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CustomUnitLife
            {
                Life = authoring.Life,
                MaxLife = authoring.Life
            });
            AddComponent(entity, new CustomUnit
            {
                Owner = authoring.Owner,
                PlayerFriendly = authoring.PlayerFriendly
            });
            AddComponent(entity, new CustomUnitBrain
            {
                State = CustomUnitBrainState.Idle
            });
        }
    }
}

