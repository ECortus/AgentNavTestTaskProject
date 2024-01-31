using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Inject] public static EnemySpawner Instance { get; private set; }
    
    [SerializeField] private GameObject prefab;
    [SerializeField] private float delay;
    [SerializeField] private int maxCount;

    [Space] 
    [SerializeField] private Transform[] dots;

    private int DelayMilliInt => Mathf.RoundToInt(delay * 1000);
    private Transform RandomDot => dots[Random.Range(0, dots.Length)];

    private CancellationTokenSource _spawnerCancellationToken;

    [Inject] private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        Deactivate();
    }

    public async void Activate()
    {
        Deactivate();
        
        _spawnerCancellationToken = new CancellationTokenSource();
        while (_spawnerCancellationToken != null)
        {
            Spawn();

            try
            {
                if (AgentsCountersMono.EnemyAlive < maxCount)
                {
                    await UniTask.Delay(DelayMilliInt, DelayType.DeltaTime, 
                        PlayerLoopTiming.Update, _spawnerCancellationToken.Token);
                }
                else
                {
                    await UniTask.WaitUntil(() => AgentsCountersMono.EnemyAlive < maxCount, 
                        PlayerLoopTiming.Update, _spawnerCancellationToken.Token);
                }
            }
            catch
            {
                _spawnerCancellationToken.Dispose();
                _spawnerCancellationToken = null;
                return;
            }
        }
    }

    private void Spawn()
    {
        Instantiate(prefab, RandomDot.position, Quaternion.identity, transform);
        AgentsCountersMono.AddEnemyAlive();
    }
    
    public void Deactivate()
    {
        _spawnerCancellationToken?.Cancel();
    }
}
