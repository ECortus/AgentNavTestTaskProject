using System.Collections;
using System.Collections.Generic;
using ProjectDawn.Navigation.Sample.Zerg;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace ECS.Test
{
    // [RequireMatchingQueriesForUpdate]
    // public partial struct CustomUnitDeathSystem : ISystem
    // {
    //     private EntityManager m_Entity;
    //     
    //     public void OnCreate(ref SystemState state)
    //     {
    //         m_Entity = World.DefaultGameObjectInjectionWorld.EntityManager;
    //     }
    //
    //     public void OnUpdate(ref SystemState state)
    //     {
    //         var ecb = new EntityCommandBuffer(Allocator.Temp);
    //
    //         foreach (var (unit, entity) in SystemAPI.Query<CustomUnit>().WithAll<CustomUnitDead>().WithEntityAccess())
    //         {
    //             switch (unit.Owner)
    //             {
    //                 case CustomUnitId.Ally:
    //                     AddAllyKilled();
    //                     break;
    //                 case CustomUnitId.Enemy:
    //                     AddEnemyKilled();
    //                     break;
    //                 default:
    //                     break;
    //             }
    //             
    //             ecb.DestroyEntity(entity);
    //         }
    //         
    //         ecb.Playback(state.EntityManager);
    //         ecb.Dispose();
    //     }
    //
    //     public void OnDestroy(ref SystemState state)
    //     {
    //         
    //     }
    //
    //     void AddAllyKilled()
    //     {
    //         var allyQuery = m_Entity.CreateEntityQuery(typeof(AllyCount));
    //         NativeArray<Entity> ally = allyQuery.ToEntityArray(Allocator.Temp);
    //                     
    //         if (ally.Length == 1)
    //         {
    //             var allyEntity = ally[0];
    //             var allyCount = m_Entity.GetComponentData<AllyCount>(allyEntity);
    //                         
    //             allyCount.Killed++;
    //             allyCount.Alive--;
    //             
    //             m_Entity.SetComponentData(allyEntity, allyCount);
    //         }
    //     }
    //
    //     void AddEnemyKilled()
    //     {
    //         var enemyQuery = m_Entity.CreateEntityQuery(typeof(EnemyCount));
    //         NativeArray<Entity> enemy = enemyQuery.ToEntityArray(Allocator.Temp);
    //                     
    //         if (enemy.Length == 1)
    //         {
    //             var enemyEntity = enemy[0];
    //             var enemyCount = m_Entity.GetComponentData<EnemyCount>(enemyEntity);
    //                         
    //             enemyCount.Killed++;
    //             enemyCount.Alive--;
    //             
    //             m_Entity.SetComponentData(enemyEntity, enemyCount);
    //         }
    //     }
    // }
    
    [RequireMatchingQueriesForUpdate]
    public partial class CustomUnitDeathSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, Transform transform, in CustomUnitDead unitDead) =>
            {
                GameObject.Destroy(transform.gameObject);
                EntityManager.DestroyEntity(entity);
            
            }).WithStructuralChanges().WithoutBurst().Run();
        }
    }
}