using System;
using System.Collections;
using System.Collections.Generic;
using ECS.Test;
using UnityEngine;

public class UnitView : MonoBehaviour
{
    private UnitAuthoring _authoring;

    [SerializeField] private ParticleType spawnEffectType;
    
    void Awake()
    {
        _authoring = GetComponent<UnitAuthoring>();

        _authoring.OnSpawn += OnSpawn;
        _authoring.OnDeath += OnDeath;
    }

    void OnSpawn()
    {
        ParticlePool.Instance.Insert(spawnEffectType,
            transform.position + Vector3.up * 0.25f, Quaternion.Euler(-90, 0, 0));
    }

    void OnDeath()
    {
        ParticlePool.Instance.Insert(ParticleType.UnitDeath, 
            transform.position + Vector3.up * 0.25f, Quaternion.Euler(-90, 0, 0));
    }
}
