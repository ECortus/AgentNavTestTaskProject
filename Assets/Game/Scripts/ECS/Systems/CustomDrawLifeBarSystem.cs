using System.Collections;
using System.Collections.Generic;
using ECS.Test;
using ProjectDawn.Navigation;
using ProjectDawn.Navigation.Sample.Zerg;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Test
{
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class CustomDrawLifeBarSystem : SystemBase
    {
        LifeBar m_LifeBar;

        protected override void OnCreate()
        {
            m_LifeBar = GameObject.FindObjectOfType<LifeBar>(true);
        }

        protected override void OnUpdate()
        {
            if (m_LifeBar == null)
            {
                return;
            }
            
            m_LifeBar.UpdateProperties();
            
            Entities.ForEach((in CustomUnitLife life, in AgentShape shape, in LocalTransform transform) =>
            {
                m_LifeBar.Draw(transform.Position, life.Life / life.MaxLife, (int)(shape.Radius / 0.2f),
                    shape.Radius, shape.Height * 1.75f);
                
            }).WithoutBurst().Run();
        }
    }
}