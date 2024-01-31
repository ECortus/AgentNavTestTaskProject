using System;
using System.Collections;
using System.Collections.Generic;
using ProjectDawn.Navigation;
using ProjectDawn.Navigation.Hybrid;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public Transform Transform => transform;
    
    private Controls _controls;
    private AgentAuthoring _body;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _controls = new Controls();
        _body = GetComponent<AgentAuthoring>();

        Instance = this;
    }

    void Start()
    {
        _controls.Enable();
    }

    private void OnDestroy()
    {
        _controls.Disable();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        var input = _controls.Player.Movement.ReadValue<Vector2>();
        
        var angle = CameraControllerMono.Instance ? 
            CameraControllerMono.Instance.Rotation.eulerAngles.y : 0;
        
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
