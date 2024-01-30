using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Test
{
    public class PlayerSpawnMono : MonoBehaviour
    {
        public GameObject PlayerPrefab;
        public Transform SpawnDot;
    }

    public class PlayerPrefabBaker : Baker<PlayerSpawnMono>
    {
        public override void Bake(PlayerSpawnMono authoring)
        {
            var playerEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(playerEntity, new PlayerSpawnComponent
            {
                PlayerPrefab = GetEntity(authoring.PlayerPrefab, TransformUsageFlags.Dynamic),
                SpawnDot = new LocalTransform
                {
                    Position = authoring.SpawnDot.position
                }
            });
        }
    }
}