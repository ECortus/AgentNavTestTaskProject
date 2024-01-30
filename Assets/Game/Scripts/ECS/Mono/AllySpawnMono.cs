using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Test
{
    public class AllySpawnMono : MonoBehaviour
    {
        public GameObject AllyPrefab;
        
        [Space]
        public float SpawnDelay;
        public int MaxCount;
    }

    public class AllySpawnBaker : Baker<AllySpawnMono>
    {
        public override void Bake(AllySpawnMono authoring)
        {
            var allyEntity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent(allyEntity, new AllySpawnComponent
            {
                AllyPrefab = GetEntity(authoring.AllyPrefab, TransformUsageFlags.Dynamic),
                SpawnDelay = authoring.SpawnDelay
            });
            AddComponent(allyEntity, new AllySpawnTimer
            {
                Value = 0
            });
            AddComponent(allyEntity, new AllySpawnInputComponent
            {
                HaveInput = false,
                Position = new float3(0, 0, 0)
            });
            AddComponent(allyEntity, new AllyCount
            {
                MaxToSpawn = authoring.MaxCount,
                Alive = 0,
                Killed = 0
            });
        }
    }
}