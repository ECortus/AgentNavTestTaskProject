using System.Collections;
using System.Collections.Generic;
using ProjectDawn.Navigation;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Test
{
    public struct PlayerTag : IComponentData
    {
        public float3 DefaultPosition;
    }
    
    public struct PlayerInputComponent : IComponentData
    {
        public float2 MoveDirection;
    }
}