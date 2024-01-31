using System.Collections;
using System.Collections.Generic;
using Unity.Transforms;
using UnityEngine;
using Zenject;

public class CameraSingleton : MonoBehaviour
{
    [Inject] public static CameraSingleton Instance { get; private set; }

    [SerializeField] private float _speed;
    [SerializeField] private float _distance;
    [SerializeField] private float _offsetUp;

    public float Speed => _speed;
    public float Distance => _distance;
    public float OffsetUp => _offsetUp;
    public Quaternion Rotation => transform.rotation;

    public Camera Camera { get; private set; }

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
}
