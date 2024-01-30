using System.Collections;
using System.Collections.Generic;
using ECS.Test;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace ECS.Test
{
    [BurstCompile]
    public partial class CameraControllerSystem : SystemBase
    {
        [BurstCompile]
        protected override void OnCreate()
        {
            RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var camera = CameraSingleton.Instance;
            if (!camera) return;

            var player = SystemAPI.GetSingletonEntity<PlayerTag>();
            var aspect = SystemAPI.GetAspect<PlayerTagAspect>(player);

            var delta = SystemAPI.Time.DeltaTime * camera.Speed;
            Vector3 position = (Vector3)aspect.Position -
                camera.Rotation * new Vector3(0, 0, 1) * camera.Distance + new Vector3(0f, camera.OffsetUp, 0f);

            camera.transform.position = Vector3.Slerp(camera.transform.position, position, delta);
        }
    }

    public struct CameraAngle : IComponentData
    {
        public float Value;
    }
}