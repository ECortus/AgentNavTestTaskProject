using System.Collections;
using System.Collections.Generic;
using ProjectDawn.Navigation;
using ProjectDawn.Navigation.Hybrid;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Test
{
    public class PlayerAuthoring : MonoBehaviour
    {
        
    }

    public class PlayerAuthoringBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var playerEntity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent<PlayerTag>(playerEntity);
            AddComponent<PlayerInputComponent>(playerEntity);
        }
    }
}
