using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace ECS.Test
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct EnemySpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
            
            state.RequireForUpdate<EnemySpawnComponent>();
            state.RequireForUpdate<EnemySpawnPoints>();
        }
        
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // state.Enabled = false;
            
            var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            var entity = SystemAPI.GetSingletonEntity<EnemySpawnComponent>();
            var spawnPointsAspect = SystemAPI.GetAspect<EnemySpawnPointsAspect>(entity);
            
            var delta = SystemAPI.Time.DeltaTime;
            new EnemySpawnJob
            {
                SpawnPoint = spawnPointsAspect.GetEnemyTransform(),
                DeltaTime = delta,
                ECB = ecb
            }.Run();
        }
    }
    
    public partial struct EnemySpawnJob : IJobEntity
    {
        public LocalTransform SpawnPoint;
        public float DeltaTime;
        public EntityCommandBuffer ECB;
        
        private void Execute(EnemySpawnAspect aspect)
        {
            aspect.EnemySpawnTimer -= DeltaTime;
            
            if (!aspect.AllowToSpawnEnemy || aspect.Alive >= aspect.MaxCount) return;
                        
            aspect.Alive++;
            aspect.EnemySpawnTimer = aspect.SpawnDelay;
            
            var newEnemy = ECB.Instantiate(aspect.EnemyPrefab);
            var newTransform = SpawnPoint;
            
            ECB.SetComponent(newEnemy, newTransform);
        }
    }
}