using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Zenject;

namespace ECS.Test
{
    public struct PlayerSpawnComponent : IComponentData
    {
        public Entity PlayerPrefab;
        public LocalTransform SpawnDot;
    }
}
