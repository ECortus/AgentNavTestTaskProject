using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class EnemyUnitSpawner : MonoBehaviour
{
    private Player Owner => GameManager.Instance.Opponent;
    
    [SerializeField] private float delay;
    [SerializeField] private int maxCount;

    [Space] 
    [SerializeField] private Transform[] dots;
    [SerializeField] private float maxDistanceFromDot;

    private GameObject prefab => Owner.GetUnit();
    private int DelayMilliInt => Mathf.RoundToInt(delay * 1000);

    private Vector3 RandomPoint
    {
        get
        {
            Vector3 point = dots[Random.Range(0, dots.Length)].position;
            Vector2 direction = Random.insideUnitCircle;
            point += new Vector3(direction.x, 0, direction.y) * Random.Range(0, maxDistanceFromDot);
            return point;
        }
    }

    private CancellationTokenSource _spawnerCancellationToken;

    [Inject] private void Awake()
    {
        GameManager.Instance.OnGameStart += Activate;
        GameManager.Instance.OnGameStop += Deactivate;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStart -= Activate;
        GameManager.Instance.OnGameStop -= Deactivate;
        
        Deactivate();
    }

    async void Activate()
    {
        Deactivate();
        
        _spawnerCancellationToken = new CancellationTokenSource();
        while (_spawnerCancellationToken != null)
        {
            Spawn();

            try
            {
                if (Owner.UnitAlive < maxCount)
                {
                    await UniTask.Delay(DelayMilliInt, DelayType.DeltaTime, 
                        PlayerLoopTiming.Update, _spawnerCancellationToken.Token);
                }
                else
                {
                    await UniTask.WaitUntil(() => Owner.UnitAlive < maxCount, 
                        PlayerLoopTiming.Update, _spawnerCancellationToken.Token);
                }
            }
            catch
            {
                _spawnerCancellationToken?.Dispose();
                _spawnerCancellationToken = null;
                return;
            }
        }
    }

    private void Spawn()
    {
        Instantiate(prefab, RandomPoint, Quaternion.identity, transform);
    }
    
    void Deactivate()
    {
        _spawnerCancellationToken?.Cancel();
        _spawnerCancellationToken = null;
    }
}
