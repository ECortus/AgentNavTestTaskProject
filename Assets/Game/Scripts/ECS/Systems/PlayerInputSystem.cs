using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace ECS.Test
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    public partial class PlayerInputSystem : SystemBase
    {
        private Controls _controls;
        private Entity _playerEntity;

        protected override void OnCreate()
        {
            RequireForUpdate<PlayerTag>();
            RequireForUpdate<PlayerInputComponent>();

            _controls = new Controls();
        }

        protected override void OnStartRunning()
        {
            _controls.Enable();

            _playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        }

        protected override void OnUpdate()
        {
            var curMoveInput = _controls.Player.Movement.ReadValue<Vector2>();
            
            SystemAPI.SetSingleton(new PlayerInputComponent{ MoveDirection = curMoveInput });
        }

        protected override void OnStopRunning()
        {
            _controls.Disable();
            
            _playerEntity = Entity.Null;;
        }
        
        protected override void OnDestroy()
        {
            
        }
    }
}