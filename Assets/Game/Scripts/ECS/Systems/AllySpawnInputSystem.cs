using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Test
{
    public partial class AllySpawnInputSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<AllySpawnComponent>();
        }

        protected override void OnUpdate()
        {
            var getMouse = Input.GetMouseButton(0);
            Vector3 position = Vector3.zero;

            if (getMouse && CameraSingleton.Instance)
            {
                var input = Input.mousePosition;
                var camera = CameraSingleton.Instance.Camera;

                var ray = camera.ScreenPointToRay(input);
                float maxDistance = 999f;
                
                if(Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance))
                {
                    position = hitInfo.point;
                }
                else
                {
                    getMouse = false;
                    position = Vector3.zero;
                }
            }
            
            new AllyInputJob
            {
                HaveInput = getMouse,
                Position = position
            }.Run();
        }

        protected override void OnDestroy()
        {
            
        }
    }
    
    public partial struct AllyInputJob : IJobEntity
    {
        public bool HaveInput;
        public float3 Position;
        
        private void Execute(AllySpawnAspect aspect)
        {
            aspect.SpawnPosition = Position;
            aspect.HaveInput = HaveInput;
        }
    }
}
