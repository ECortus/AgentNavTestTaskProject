using System.Collections;
using System.Collections.Generic;
using ProjectDawn.Navigation;
using ProjectDawn.Navigation.Sample.Zerg;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;

[BurstCompile]
[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct UnitBrainSystem : ISystem
{
    ComponentLookup<Unit> m_UnitLooukup;
    ComponentLookup<UnitLife> m_LifeLookup;
    ComponentLookup<UnitBrain> m_BrainLookup;
    ComponentLookup<AgentBody> m_BodyLookup;
    ComponentLookup<AgentShape> m_ShapeLookup;
    ComponentLookup<LocalTransform> m_TransformLookupRO;
    ComponentLookup<LocalTransform> m_TransformLookupRW;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        m_UnitLooukup = state.GetComponentLookup<Unit>(true);
        m_LifeLookup = state.GetComponentLookup<UnitLife>();
        m_BrainLookup = state.GetComponentLookup<UnitBrain>();
        m_ShapeLookup = state.GetComponentLookup<AgentShape>(true);
        m_BodyLookup = state.GetComponentLookup<AgentBody>();
        m_TransformLookupRO = state.GetComponentLookup<LocalTransform>(true);
        m_TransformLookupRW = state.GetComponentLookup<LocalTransform>();
        
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        state.RequireForUpdate<AgentSpatialPartitioningSystem.Singleton>();
        state.RequireForUpdate<PlayerTag>();
    }

    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var spatial = SystemAPI.GetSingleton<AgentSpatialPartitioningSystem.Singleton>();
        var buffer = ecb.CreateCommandBuffer(state.WorldUnmanaged);

        m_UnitLooukup.Update(ref state);
        m_LifeLookup.Update(ref state);
        m_BrainLookup.Update(ref state);
        m_ShapeLookup.Update(ref state);
        m_BodyLookup.Update(ref state);
        m_TransformLookupRO.Update(ref state);
        m_TransformLookupRW.Update(ref state);

        new UnitBrainMoveJob
        {
        }.Schedule();

        new UnitBrainAttackJob
        {
            Ecb = buffer,
            LifeLookup = m_LifeLookup,
            ElapsedTime = state.WorldUnmanaged.Time.ElapsedTime
        }.Schedule();

        new UnitBrainFacingJob
        {
            TransformLookup = m_TransformLookupRW,
            DeltaTime = state.WorldUnmanaged.Time.DeltaTime
        }.Schedule();
        
        new UnitBrainFollowJob
        {
            TransformLookup = m_TransformLookupRO,
            ShapeLookup = m_ShapeLookup,
            ElapsedTime = state.WorldUnmanaged.Time.ElapsedTime
        }.ScheduleParallel();

        new UnitBrainIdleJob
        {
            Player = SystemAPI.GetSingletonEntity<PlayerTag>(),
            RangeToPlayer = UnityEngine.Random.Range(3f, 9f),
            UnitLookup = m_UnitLooukup,
            Spatial = spatial
        }.ScheduleParallel();

        new UnitBrainSmartStopJob
        {
            BodyLookup = m_BodyLookup,
            BrainLookup = m_BrainLookup,
            UnitLookup = m_UnitLooukup,
            Spatial = spatial
        }.Schedule();

        new UnitBrainHiveMindStopJob
        {
        }.Schedule();
    }

    [BurstCompile]
    partial struct UnitBrainMoveJob : IJobEntity
    {
        public void Execute(ref UnitBrain brain, in AgentBody body, in Unit unit)
        {
            if (unit.Owner == UnitId.Player) return;
            
            if (brain.State != UnitBrainState.Move)
                return;

            if (!body.IsStopped)
                return;

            brain.State = UnitBrainState.Idle;
        }
    }

    [BurstCompile]
    partial struct UnitBrainHiveMindStopJob : IJobEntity
    {
        public void Execute(ref AgentSmartStop smartStop, in UnitBrain brain, in AgentBody body, in Unit unit)
        {
            if (unit.Owner == UnitId.Player) return;
            
            if (brain.State == UnitBrainState.Attack || brain.State == UnitBrainState.Follow)
            {
                if (smartStop.HiveMindStop.Enabled)
                    smartStop.HiveMindStop.Enabled = false;
            }
            else
            {
                if (!smartStop.HiveMindStop.Enabled)
                    smartStop.HiveMindStop.Enabled = true;
            }
        }
    }

    [BurstCompile]
    partial struct UnitBrainSmartStopJob : IJobEntity
    {
        [ReadOnly]
        public AgentSpatialPartitioningSystem.Singleton Spatial;
        public ComponentLookup<AgentBody> BodyLookup;
        public ComponentLookup<UnitBrain> BrainLookup;
        [ReadOnly]
        public ComponentLookup<Unit> UnitLookup;

        public void Execute(Entity entity, ref UnitSmartStop smartStop, in Unit unit, in LocalTransform transform)
        {
            if (unit.Owner == UnitId.Player) return;
            
            var brain = BrainLookup[entity];

            if (brain.State != UnitBrainState.Move)
                return;

            var body = BodyLookup[entity];

            // This is just a high performance foreach for nearby agents
            // It is basically as: foreach (var nearbyAgent in GetNearbyAgents()) Spatial.Execute(...)
            // For each nearby agent check if they reached destination
            var action = new FindTargetAction
            {
                BodyLookup = BodyLookup,
                BrainLookup = BrainLookup,
                UnitLookup = UnitLookup,
                Entity = entity,
                Unit = unit,
                Body = body,
                SmartStop = smartStop,
                Transform = transform,
            };
            Spatial.QuerySphere(transform.Position, smartStop.Radius, ref action);

            // If any nearby agent reached destination, this agent should stop too
            if (!action.Stop)
                return;

            brain.State = UnitBrainState.Idle;
            BrainLookup[entity] = brain;

            body.Stop();
            BodyLookup[entity] = body;
        }

        [BurstCompile]
        struct FindTargetAction : ISpatialQueryEntity
        {
            [ReadOnly]
            public ComponentLookup<AgentBody> BodyLookup;
            [ReadOnly]
            public ComponentLookup<UnitBrain> BrainLookup;
            [ReadOnly]
            public ComponentLookup<Unit> UnitLookup;

            public Entity Entity;
            public AgentBody Body;
            public Unit Unit;
            public UnitSmartStop SmartStop;
            public LocalTransform Transform;

            // Output if this agent should stop
            public bool Stop;

            public void Execute(Entity entity, AgentBody body, AgentShape shape, LocalTransform transform)
            {
                // Exclude itself
                if (Entity == entity)
                {
                    return;
                }

                // Check if they collide
                float distance = math.distance(Transform.Position, transform.Position);
                if (SmartStop.Radius < distance)
                {
                    return;
                }

                // Check if same or friendly unit
                if (!UnitLookup.TryGetComponent(entity, out Unit unit) 
                    || Unit.Owner != unit.Owner)
                {
                    return;
                }

                // Check if idle
                if (!BrainLookup.TryGetComponent(entity, out UnitBrain brain) 
                    || brain.State != UnitBrainState.Idle)
                {
                    return;
                }

                // Check if they have similar destinations within the radius
                float distance2 = math.distance(Body.Destination, body.Destination);
                if (SmartStop.Radius < distance2)
                {
                    return;
                }

                Stop = true;
            }
        }
    }

    [BurstCompile]
    partial struct UnitBrainIdleJob : IJobEntity
    {
        [ReadOnly] 
        public Entity Player;
        [ReadOnly] 
        public float RangeToPlayer;
        [ReadOnly]
        public AgentSpatialPartitioningSystem.Singleton Spatial;
        [ReadOnly]
        public ComponentLookup<Unit> UnitLookup;

        public void Execute(Entity entity, ref UnitBrain brain, ref UnitFollow follow, 
            ref AgentBody body, in UnitCombat combat, in Unit unit, in LocalTransform transform)
        {
            // if (brain.State != CustomUnitBrainState.Idle)
            //     return;
            
            if (brain.State == UnitBrainState.Attack)
                return;

            var action = new FindTargetAction
            {
                UnitLookup = UnitLookup,
                Entity = entity,
                Unit = unit,
                Transform = transform,
                Distance = float.MaxValue,
            };
            Spatial.QuerySphere(transform.Position, combat.AggressionRadius, ref action);

            if (action.Target == Entity.Null)
            {
                if (unit.PlayerFriendly && follow.Target != Player)
                {
                    follow.Target = Player;
                    follow.Unit = unit;
                    follow.MinDistance = RangeToPlayer;
                    brain.State = UnitBrainState.PlayerFollower;
                }
                
                return;
            }

            follow.Target = action.Target;
            follow.Unit = action.Unit;
            follow.MinDistance = combat.Range;
            brain.State = UnitBrainState.Follow;
        }

        [BurstCompile]
        struct FindTargetAction : ISpatialQueryEntity
        {
            [ReadOnly]
            public ComponentLookup<Unit> UnitLookup;
            public Entity Entity;
            public Unit Unit;
            public LocalTransform Transform;

            public Entity Target;
            public float Distance;

            public void Execute(Entity entity, AgentBody body, AgentShape shape, LocalTransform transform)
            {
                // Find closest target
                float distance = math.distance(Transform.Position, transform.Position);
                if (Distance <= distance)
                {
                    return;
                }

                // Check if enemies
                if (!UnitLookup.TryGetComponent(entity, out Unit unit) || Unit.Owner == unit.Owner || 
                    (unit.Owner == UnitId.Player && Unit.PlayerFriendly) ||
                    (Unit.Owner == UnitId.Player && unit.PlayerFriendly))
                {
                    return;
                }
                
                Distance = distance;
                Target = entity;
            }
        }
    }

    [BurstCompile]
    partial struct UnitBrainFollowJob : IJobEntity
    {
        [ReadOnly]
        public ComponentLookup<LocalTransform> TransformLookup;
        [ReadOnly]
        public ComponentLookup<AgentShape> ShapeLookup;
        public double ElapsedTime;

        public void Execute(ref UnitBrain brain, ref UnitCombat combat, 
            ref AgentBody body, in UnitFollow follow, in AgentShape shape, 
            in LocalTransform transform, in Unit unit)
        {
            if (brain.State != UnitBrainState.Follow
                && brain.State != UnitBrainState.PlayerFollower)
            {
                return;
            }

            // Check if target is alive
            if (!TransformLookup.TryGetComponent(follow.Target, out LocalTransform targetTransform))
            {
                brain.State = UnitBrainState.Idle;
                body.Stop();
                return;
            }
            
            if (!ShapeLookup.TryGetComponent(follow.Target, out AgentShape targetShape))
            {
                brain.State = UnitBrainState.Idle;
                body.Stop();
                return;
            }

            float3 towards = targetTransform.Position - transform.Position;
            float distancesq = math.distancesq(transform.Position, targetTransform.Position);
            float range = shape.Radius + targetShape.Radius + combat.Range;
            
            if (distancesq <= range * range 
                && brain.State != UnitBrainState.PlayerFollower)
            {
                if (combat.IsReady(ElapsedTime))
                {
                    combat.Target = follow.Target;
                    combat.AttackTime = ElapsedTime;
                    brain.State = UnitBrainState.Attack;
                }
                // body.Stop();
                
                return;
            }
            
            if (unit.Owner == UnitId.Player)
            {
                return;
            }

            float3 offset;
            if (brain.State == UnitBrainState.PlayerFollower)
            {
                offset = towards / math.sqrt(distancesq) * (shape.Radius + shape.Radius) * follow.MinDistance;

                if (math.distancesq(targetTransform.Position, transform.Position) >
                    math.distancesq(targetTransform.Position, targetTransform.Position - offset))
                {
                    body.Destination = targetTransform.Position - offset;
                    body.IsStopped = false;
                }
                else
                {
                    body.Stop();
                }
            }
            else
            {
                offset = towards / math.sqrt(distancesq) * (shape.Radius + shape.Radius);
                body.Destination = targetTransform.Position - offset;
                body.IsStopped = false;
            }
        }
    }

    [BurstCompile]
    partial struct UnitBrainAttackJob : IJobEntity
    {
        public EntityCommandBuffer Ecb;
        public ComponentLookup<UnitLife> LifeLookup;
        public double ElapsedTime;

        public void Execute(Entity entity, ref UnitBrain brain, ref UnitCombat combat)
        {
            if (brain.State != UnitBrainState.Attack)
                return;

            if (!combat.IsFinished(ElapsedTime))
                return;

            // Check if target is alive
            if (!LifeLookup.TryGetComponent(combat.Target, out UnitLife targetLife) || targetLife.Life == 0)
            {
                brain.State = UnitBrainState.Idle;
                return;
            }

            // Reduce target life
            targetLife.Life -= combat.Damage;
            if (targetLife.Life <= 0)
            {
                targetLife.Life = 0;
                Ecb.AddComponent<UnitDead>(combat.Target);
            }
            LifeLookup[combat.Target] = targetLife;

            // Set attack on cooldown
            combat.CooldownTime = ElapsedTime;
            brain.State = UnitBrainState.Follow;
        }
    }

    [BurstCompile]
    partial struct UnitBrainFacingJob : IJobEntity
    {
        public ComponentLookup<LocalTransform> TransformLookup;
        public float DeltaTime;

        public void Execute(Entity entity, in UnitBrain brain, in UnitCombat combat, 
            in AgentLocomotion locomotion, in Unit unit)
        {
            if (unit.Owner == UnitId.Player) return;
            
            if (brain.State != UnitBrainState.Attack)
                return;

            if (!TransformLookup.TryGetComponent(entity, out LocalTransform transform))
                return;

            // Check if target is alive
            if (!TransformLookup.TryGetComponent(combat.Target, out LocalTransform targetTransform))
                return;

            float3 facing = math.normalizesafe(targetTransform.Position - transform.Position);
            float angle = math.atan2(facing.x, facing.z);
            transform.Rotation = math.slerp(transform.Rotation, quaternion.RotateY(angle), DeltaTime * locomotion.AngularSpeed);
            TransformLookup[entity] = transform;
        }
    }
}
