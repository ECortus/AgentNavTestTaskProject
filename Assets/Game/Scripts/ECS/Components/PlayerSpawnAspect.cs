using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;

namespace ECS.Test
{
    public readonly partial struct PlayerSpawnAspect : IAspect
    {
        public readonly Entity Entity;
        private readonly RefRO<PlayerSpawnComponent> _playerSpawn;

        public Entity PlayerPrefab => _playerSpawn.ValueRO.PlayerPrefab;
        private LocalTransform SpawnDot => _playerSpawn.ValueRO.SpawnDot;

        public LocalTransform GetPlayerTransform()
        {
            return new LocalTransform
            {
                Position = SpawnDot.Position,
                Rotation = quaternion.identity,
                Scale = 1f
            };
        }
    }
}
