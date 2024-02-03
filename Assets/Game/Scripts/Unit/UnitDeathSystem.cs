using System.Collections;
using System.Collections.Generic;
using ProjectDawn.Navigation.Sample.Zerg;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[RequireMatchingQueriesForUpdate]
public partial class UnitDeathSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity, Transform transform, in UnitDead unitDead) =>
        {
            transform.GetComponent<UnitAuthoring>().Death();
            
            GameObject.Destroy(transform.gameObject);
            EntityManager.DestroyEntity(entity);
        
        }).WithStructuralChanges().WithoutBurst().Run();
    }
}