using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private string sessionName;
    public string SessionName => sessionName;
    
    [SerializeField] private GameObject[] units;
    public GameObject GetUnit()
    {
        if (units.Length == 0) return null;
        return units[Random.Range(0, units.Length)];
    }

    public Action OnUnitUpdate { get; set; }
    void UnitUpdate() => OnUnitUpdate?.Invoke();
    
    public int UnitAlive { get; private set; }
    public int UnitKilled { get; private set; }
    public void AddAlive()
    {
        UnitAlive++;
        UnitUpdate();
    }
    public void AddKilled()
    {
        UnitAlive--;
        UnitKilled++;
        UnitUpdate();
    }

    public Action OnDeathEvent { get; set; }

    private void OnDestroy()
    {
        OnDeathEvent?.Invoke();
    }

    void Start()
    {
        UnitAlive = 0;
        UnitKilled = 0;
        
        UnitUpdate();
    }
}
