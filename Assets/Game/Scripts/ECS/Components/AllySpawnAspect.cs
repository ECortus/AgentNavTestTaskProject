using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;

namespace ECS.Test
{
    public readonly partial struct AllySpawnAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<AllySpawnComponent> _allySpawn;
        private readonly RefRW<AllySpawnInputComponent> _allySpawnInput;
        private readonly RefRW<AllyCount> _allyCount;
        private readonly RefRW<AllySpawnTimer> _allySpawnTimer;

        public Entity AllyPrefab => _allySpawn.ValueRO.AllyPrefab;
        public float SpawnDelay => _allySpawn.ValueRO.SpawnDelay;

        public int MaxCount => _allyCount.ValueRO.MaxToSpawn;
        
        public int Alive
        {
            get => _allyCount.ValueRO.Alive;
            set => _allyCount.ValueRW.Alive = value;
        }
        
        public int Killed
        {
            get => _allyCount.ValueRO.Killed;
            set => _allyCount.ValueRW.Killed = value;
        }

        public bool HaveInput
        {
            get => _allySpawnInput.ValueRO.HaveInput;
            set => _allySpawnInput.ValueRW.HaveInput = value;
        }
        
        public float3 SpawnPosition
        {
            get => _allySpawnInput.ValueRO.Position;
            set => _allySpawnInput.ValueRW.Position = value;
        }

        public float AllySpawnTimer
        {
            get => _allySpawnTimer.ValueRO.Value;
            set => _allySpawnTimer.ValueRW.Value = value;
        }

        public bool AllowToSpawnAlly => AllySpawnTimer <= 0f;
    }
}