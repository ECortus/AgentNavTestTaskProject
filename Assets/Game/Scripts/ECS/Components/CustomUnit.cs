using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace ECS.Test
{
    public enum CustomUnitId
    {
        Player,
        Ally,
        Enemy
    }

    public struct CustomUnit : IComponentData
    {
        public CustomUnitId Owner;
        public bool PlayerFriendly;
    }
    
    public enum CustomUnitBrainState
    {
        Idle,
        Move,
        Follow,
        PlayerFollower,
        Attack,
    }
    
    public struct CustomUnitBrain : IComponentData
    {
        public CustomUnitBrainState State;
    }

    public struct CustomUnitCombat : IComponentData
    {
        public Entity Target;
        public float Damage;
        public float AggressionRadius;
        public float Range;
        public float Speed;
        public float Cooldown;
        public double CooldownTime;
        public double AttackTime;
        public bool IsReady(double time) => time >= CooldownTime + Cooldown;
        public bool IsFinished(double time) => time >= AttackTime + Speed;
    }

    public struct CustomUnitFollow : IComponentData
    {
        public Entity Target;
        public CustomUnit Unit;
        public float MinDistance;
    }

    public struct CustomUnitLife : IComponentData
    {
        public float Life;
        public float MaxLife;
    }

    public struct CustomUnitDead : IComponentData { }

    public struct CustomUnitSmartStop : IComponentData
    {
        public float Radius;
    }
}
