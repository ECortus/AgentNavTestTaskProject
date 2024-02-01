using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerCameraController : MonoBehaviour
{
    [Inject] public static PlayerCameraController Instance { get; private set; }

    [SerializeField] private float speed;
    [SerializeField] private float distance;
    [SerializeField] private float offsetUp;

    [Space] 
    [SerializeField] private Transform target;

    public void SetTarget(Transform trg)
    {
        target = trg;
    }
    
    public Camera Camera { get; private set; }

    private Vector3 Position
    {
        get
        {
            if (target)
            {
                return target.position -
                    Rotation * new Vector3(0, 0, 1) * distance + new Vector3(0f, offsetUp, 0f);
            }
            
            return Vector3.zero;
        }
    }

    public Quaternion Rotation => transform.rotation;

    [Inject] void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Camera = Camera.main;
    }

    void Update()
    {
        if (target)
        {
            var delta = Time.deltaTime;
            transform.position = Vector3.Slerp(transform.position, Position, speed * delta);
        }
    }
}
