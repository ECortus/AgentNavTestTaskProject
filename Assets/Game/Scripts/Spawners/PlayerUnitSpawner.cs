using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class PlayerUnitSpawner : MonoBehaviour
{
    private Player Owner => GameManager.Instance.Player;
    
    [SerializeField] private float delay;
    [SerializeField] private int maxCount;

    private GameObject prefab => Owner.GetUnit();
    private int DelayMilliInt => Mathf.RoundToInt(delay * 1000);

    private bool _getMouseInput;
    private Vector3 position;
    
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

    void Update()
    {
        _getMouseInput = Input.GetMouseButton(0);

        if (_getMouseInput && PlayerCameraController.Instance)
        {
            var input = Input.mousePosition;
            var camera = PlayerCameraController.Instance.Camera;

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

    async void Activate()
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
            else
            {
                await UniTask.Yield();
            }
        }
    }

    private void Spawn(Vector3 pos)
    {
        Instantiate(prefab, pos, Quaternion.identity, transform);
    }
    
    void Deactivate()
    {
        _spawnerCancellationToken?.Cancel();
        _spawnerCancellationToken = null;
    }
}
