using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Test
{
    public struct AllySpawnComponent : IComponentData
    {
        public Entity AllyPrefab;
        public float SpawnDelay;
    }

    public struct AllySpawnTimer : IComponentData
    {
        public float Value;
    }
    
    public struct AllySpawnInputComponent : IComponentData
    {
        public bool HaveInput;
        public float3 Position;
    }
    
    public struct AllyCount : IComponentData
    {
        public int MaxToSpawn;
        public int Alive;
        public int Killed;
    }
}