using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace ECS.Test
{
    public struct EnemySpawnComponent : IComponentData
    {
        public Entity EnemyPrefab;
        public float SpawnDelay;
    }

    [ChunkSerializable]
    public struct EnemySpawnPoints : IComponentData
    {
        public NativeArray<float3> Value;
        public float RadiusFromSpawnDot;
    }
    
    public struct EnemySpawnTimer : IComponentData
    {
        public float Value;
    }

    public struct EnemyCount : IComponentData
    {
        public int MaxToSpawn;
        public int Alive;
        public int Killed;
    }
}