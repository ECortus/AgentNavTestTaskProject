using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Test
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct PlayerSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerSpawnComponent>();
        }
        
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            
            var playerEntity = SystemAPI.GetSingletonEntity<PlayerSpawnComponent>();
            var playerSpawn = SystemAPI.GetAspect<PlayerSpawnAspect>(playerEntity);

            var ecb = new EntityCommandBuffer(Allocator.Temp);
            Spawn(playerSpawn, ecb);
            
            ecb.Playback(state.EntityManager);
        }

        [BurstCompile]
        void Spawn(PlayerSpawnAspect aspect, EntityCommandBuffer buffer)
        {
            var newPlayer = buffer.Instantiate(aspect.PlayerPrefab);
            var newPlayerTransform = aspect.GetPlayerTransform();
            
            buffer.SetComponent(newPlayer, newPlayerTransform);
        }
    }
}
