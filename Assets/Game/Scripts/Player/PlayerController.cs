using System;
using System.Collections;
using System.Collections.Generic;
using ProjectDawn.Navigation;
using ProjectDawn.Navigation.Hybrid;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [Inject] public static PlayerController Instance { get; private set; }
    
    private Controls _controls;
    private AgentAuthoring _body;

    [Inject] private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _controls = new Controls();
        _body = GetComponent<AgentAuthoring>();

        Instance = this;
        _controls.Enable();
    }

    void Start()
    {
        PlayerCameraController.Instance.SetTarget(transform);
    }

    private void OnDestroy()
    {
        _controls.Disable();
    }

    void Update()
    {
        if (GameManager.Instance.GameStarted)
        {
            Move();
        }
    }

    void Move()
    {
        var input = _controls.Player.Movement.ReadValue<Vector2>();
        
        var angle = PlayerCameraController.Instance ? 
            PlayerCameraController.Instance.Rotation.eulerAngles.y : 0;
        
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, angle, 0));
        Vector3 direction = matrix.MultiplyPoint3x4(new Vector3(input.x, 0, input.y));
            
        float3 position = (float3)(transform.position + direction);
        SetDestination(position);
    }

    void SetDestination(float3 position)
    {
        _body.SetDestination(position);
    }
}
