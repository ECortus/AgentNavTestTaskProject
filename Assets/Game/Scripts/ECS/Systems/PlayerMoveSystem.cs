using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Test
{
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial class PlayerMoveSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<PlayerInputComponent>();
            RequireForUpdate<PlayerTag>();
        }

        protected override void OnUpdate()
        {
            var delta = SystemAPI.Time.DeltaTime;
            var angle = CameraSingleton.Instance ? CameraSingleton.Instance.Rotation.eulerAngles.y : 0;

            new PlayerMoveJob
            {
                DeltaTime = delta,
                Rotation = angle
            }.ScheduleParallel();
        }

        protected override void OnDestroy()
        {
            
        }
    }

    public partial struct PlayerMoveJob : IJobEntity
    {
        public float DeltaTime;
        public float Rotation;

        private void Execute(PlayerTagAspect aspect, PlayerInputComponent input)
        {
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, Rotation, 0));
            Vector3 direction = matrix.MultiplyPoint3x4(new Vector3(input.MoveDirection.x, 0, input.MoveDirection.y));
            
            // transform.Position.xz += new float2(direction.x, direction.z) * 5 * DeltaTime;
            float3 position = aspect.Position.xyz + (float3)direction;
            aspect.SetDestination(position);
        }
    }
}