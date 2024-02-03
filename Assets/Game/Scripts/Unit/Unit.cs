using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public enum UnitId
{
    Player,
    Ally,
    Enemy
}

public struct Unit : IComponentData
{
    public UnitId Owner;
    public bool PlayerFriendly;
}

public enum UnitBrainState
{
    Idle,
    Move,
    Follow,
    PlayerFollower,
    Attack,
}

public struct UnitBrain : IComponentData
{
    public UnitBrainState State;
}

public struct UnitCombat : IComponentData
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

public struct UnitFollow : IComponentData
{
    public Entity Target;
    public Unit Unit;
    public float MinDistance;
}

public struct UnitLife : IComponentData
{
    public float Life;
    public float MaxLife;
}

public struct UnitDead : IComponentData { }

public struct UnitSmartStop : IComponentData
{
    public float Radius;
}