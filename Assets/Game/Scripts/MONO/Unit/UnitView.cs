using System;
using System.Collections;
using System.Collections.Generic;
using ECS.Test;
using UnityEngine;

public class UnitView : MonoBehaviour
{
    private UnitAuthoringMono _authoring;
    
    void Awake()
    {
        _authoring = GetComponent<UnitAuthoringMono>();

        _authoring.OnSpawn += OnSpawn;
        // _authoring.OnDeath += OnDeath;
    }

    void OnSpawn()
    {
        ParticleType type = ParticleType.PlayerUnitSpawn;
        switch (_authoring.Owner)
        {
            case CustomUnitId.Ally:
                type = ParticleType.AllyUnitSpawn;
                break;
            case CustomUnitId.Enemy:
                type = ParticleType.EnemyUnitSpawn;
                break;
            default:
                break;
        }
        
        ParticlePool.Instance.Insert(type,
            transform.position + Vector3.up * 0.25f, Quaternion.Euler(-90, 0, 0));
    }

    void OnDeath()
    {
        ParticlePool.Instance.Insert(ParticleType.UnitDeath, 
            transform.position + Vector3.up * 0.25f, Quaternion.Euler(-90, 0, 0));
    }
}
