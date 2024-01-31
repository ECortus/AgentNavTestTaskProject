using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class AllySpawner : MonoBehaviour
{
    [Inject] public static AllySpawner Instance { get; private set; }
    
    [SerializeField] private GameObject prefab;
    [SerializeField] private float delay;
    [SerializeField] private int maxCount;

    private int DelayMilliInt => Mathf.RoundToInt(delay * 1000);

    private bool _getMouseInput;
    private Vector3 position;

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

    void Update()
    {
        _getMouseInput = Input.GetMouseButton(0);

        if (_getMouseInput && CameraControllerMono.Instance)
        {
            var input = Input.mousePosition;
            var camera = CameraControllerMono.Instance.Camera;

            var ray = camera.ScreenPointToRay(input);
            float maxDistance = 999f;
                
            if(Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance))
            {
                position = hitInfo.point;
            }
            else
            {
                _getMouseInput = false;
                position = Vector3.zero;
            }
        }
    }

    public async void Activate()
    {
        Deactivate();
        
        _spawnerCancellationToken = new CancellationTokenSource();
        while (_spawnerCancellationToken != null)
        {
            if (_getMouseInput)
            {
                Spawn(position);

                try
                {
                    if (AgentsCountersMono.AllyAlive < maxCount)
                    {
                        await UniTask.Delay(DelayMilliInt, DelayType.DeltaTime, 
                            PlayerLoopTiming.Update, _spawnerCancellationToken.Token);
                    }
                    else
                    {
                        await UniTask.WaitUntil(() => AgentsCountersMono.AllyAlive < maxCount, 
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
            else
            {
                await UniTask.Yield();
            }
        }
    }

    private void Spawn(Vector3 pos)
    {
        Instantiate(prefab, pos, Quaternion.identity, transform);
        AgentsCountersMono.AddAllyAlive();
    }
    
    public void Deactivate()
    {
        _spawnerCancellationToken?.Cancel();
    }
}
