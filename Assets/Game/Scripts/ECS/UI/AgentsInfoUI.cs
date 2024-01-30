using System.Collections;
using System.Collections.Generic;
using ECS.Test;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class AgentsInfoUI : MonoBehaviour
{
     [SerializeField] private Text allyAlive, allyKilled, enemyAlive, enemyKilled;

     private EnemyCount _enemyCount;
     private AllyCount _allyCount;
    
     public void Update()
     {
         var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
         var enemyQuery = manager.CreateEntityQuery(typeof(EnemyCount));
         
         NativeArray<Entity> enemy = enemyQuery.ToEntityArray(Allocator.Temp);
         
         if (enemy.Length == 1)
         {
             var entity = enemy[0];
             _enemyCount = manager.GetComponentData<EnemyCount>(entity);

             enemyAlive.text = _enemyCount.Alive.ToString();
             enemyKilled.text = _enemyCount.Killed.ToString();
         }
         
         var allyQuery = manager.CreateEntityQuery(typeof(AllyCount));
         
         NativeArray<Entity> ally = allyQuery.ToEntityArray(Allocator.Temp);
         
         if (ally.Length == 1)
         {
             var entity = ally[0];
             _allyCount = manager.GetComponentData<AllyCount>(entity);
             
             allyAlive.text = _allyCount.Alive.ToString();
             allyKilled.text = _allyCount.Killed.ToString();
         }
     }
}
