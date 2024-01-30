using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace ECS.Test
{
    public readonly partial struct EnemySpawnPointsAspect : IAspect
    {
        public readonly Entity Entity;
        
        private readonly RefRO<EnemySpawnPoints> _enemyPoints;
        
        private NativeArray<float3> Points => _enemyPoints.ValueRO.Value;
        private float Radius => _enemyPoints.ValueRO.RadiusFromSpawnDot;
        
        public LocalTransform GetEnemyTransform()
        {
            return new LocalTransform
            {
                Position = GetRandomPosition(),
                Rotation = quaternion.identity,
                Scale = 1f
            };
        }
        
        float3 GetRandomPosition()
        {
            float3 point = Points[Random.Range(0, Points.Length)];
            float angle = Random.Range(0, 360f);
            point += (float3)new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad)) * Radius;
            return point;
        }
    }
}