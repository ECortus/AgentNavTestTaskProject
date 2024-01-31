using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Zenject;

namespace ECS.Test
{
    public class EnemySpawnMono : MonoBehaviour
    {
        public GameObject EnemyPrefab;
        public int MaxCount;
        public float SpawnDelay;
        
        [Space]
        public Transform[] SpawnDots;
        public float RadiusFromSpawnDot;
    }
    
    public class EnemyPrefabBaker : Baker<EnemySpawnMono>
    {
        public override void Bake(EnemySpawnMono authoring)
        {
            var enemyEntity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent(enemyEntity, new EnemySpawnComponent
            {
                EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
                SpawnDelay = authoring.SpawnDelay
            });

            NativeList<float3> bufferDots = new NativeList<float3>(Allocator.TempJob);

            Transform[] dots = authoring.SpawnDots;
            for (int i = 0; i < dots.Length; i++)
            {
                bufferDots.Add(dots[i].position);
            }
            
            AddComponent(enemyEntity, new EnemySpawnPoints
            {
                Value = bufferDots.ToArray(Allocator.TempJob),
                RadiusFromSpawnDot = authoring.RadiusFromSpawnDot
            });
            
            AddComponent(enemyEntity, new EnemyCount
            {
                MaxToSpawn = authoring.MaxCount,
                Alive = 0,
                Killed = 0
            });
            
            AddComponent<EnemySpawnTimer>(enemyEntity);
        }
    }
}