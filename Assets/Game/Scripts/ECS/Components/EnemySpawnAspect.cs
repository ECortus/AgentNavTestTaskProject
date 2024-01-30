using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace ECS.Test
{
    public readonly partial struct EnemySpawnAspect : IAspect
    {
        public readonly Entity Entity;
        
        private readonly RefRO<EnemySpawnComponent> _enemySpawn;
        private readonly RefRW<EnemyCount> _enemyCount;
        private readonly RefRW<EnemySpawnTimer> _enemySpawnTimer;

        public Entity EnemyPrefab => _enemySpawn.ValueRO.EnemyPrefab;
        
        public int MaxCount => _enemyCount.ValueRO.MaxToSpawn;

        public int Alive
        {
            get => _enemyCount.ValueRO.Alive;
            set => _enemyCount.ValueRW.Alive = value;
        }
        
        public int Killed
        {
            get => _enemyCount.ValueRO.Killed;
            set => _enemyCount.ValueRW.Killed = value;
        }
        
        public float SpawnDelay => _enemySpawn.ValueRO.SpawnDelay;
        
        public float EnemySpawnTimer
        {
            get => _enemySpawnTimer.ValueRO.Value;
            set => _enemySpawnTimer.ValueRW.Value = value;
        }

        public bool AllowToSpawnEnemy => EnemySpawnTimer <= 0f;
    }
}