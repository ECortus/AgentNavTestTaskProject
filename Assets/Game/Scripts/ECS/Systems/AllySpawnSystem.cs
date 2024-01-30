using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Test
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct AllySpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<AllySpawnComponent>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            
            var delta = SystemAPI.Time.DeltaTime;
            new AllySpawnJob
            {
                DeltaTime = delta,
                ECB = ecb
            }.Run();
        }
        
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            
        }
    }

    public partial struct AllySpawnJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer ECB;
        
        private void Execute(AllySpawnAspect aspect)
        {
            if (!aspect.HaveInput)
            {
                aspect.AllySpawnTimer = 0;
                return;
            }
            
            aspect.AllySpawnTimer -= DeltaTime;
            
            if (!aspect.AllowToSpawnAlly || aspect.Alive >= aspect.MaxCount) return;
                        
            aspect.Alive++;
            aspect.AllySpawnTimer = aspect.SpawnDelay;

            var newAlly = ECB.Instantiate(aspect.AllyPrefab);
            var newTransform = new LocalTransform
            {
                Position = aspect.SpawnPosition,
                Rotation = quaternion.identity,
                Scale = 1f
            };
            
            ECB.SetComponent(newAlly, newTransform);
        }
    }
}