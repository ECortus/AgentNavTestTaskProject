using Unity.Entities;
using UnityEngine;
using ProjectDawn.Navigation.Hybrid;

namespace ProjectDawn.Navigation.Sample.Zerg
{
    public class UnitAuthoring : MonoBehaviour
    {
        public PlayerId Owner;
        // public Animator Animator;
        public float MoveAnimationSpeed = 0.4f;
        public float Life = 100;

        Entity m_Entity;

        void Awake()
        {
            Animator animator = GetComponentInChildren<Animator>();
            
            var world = World.DefaultGameObjectInjectionWorld;
            m_Entity = GetComponent<AgentAuthoring>().GetOrCreateEntity();
            world.EntityManager.AddComponentData(m_Entity, new Unit
            {
                Owner = Owner,
            });
            world.EntityManager.AddComponentData(m_Entity, new UnitAnimator
            {
                AttackId = Animator.StringToHash("Attack 1"),
                MoveSpeed = MoveAnimationSpeed,
                MoveId = Animator.StringToHash("Move"),
                MoveSpeedId = Animator.StringToHash("Speed"),
            });
            world.EntityManager.AddComponentData(m_Entity, new UnitBrain
            {
                State = UnitBrainState.Idle,
            });
            world.EntityManager.AddComponentData(m_Entity, new UnitLife
            {
                Life = Life,
                MaxLife = Life,
            });
            
            if (animator)
                world.EntityManager.AddComponentObject(m_Entity, animator);
        }

        void OnDestroy()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            if (world != null)
            {
                world.EntityManager.RemoveComponent<Unit>(m_Entity);
                world.EntityManager.RemoveComponent<UnitAnimator>(m_Entity);
                world.EntityManager.RemoveComponent<UnitBrain>(m_Entity);
                world.EntityManager.RemoveComponent<UnitLife>(m_Entity);
            }
        }
    }
}
