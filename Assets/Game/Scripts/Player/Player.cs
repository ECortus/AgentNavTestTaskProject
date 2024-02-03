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
    [SerializeField] private PlayerUI infoUI;
    
    public string SessionName => sessionName;
    
    [Space]
    [SerializeField] private GameObject[] units;
    public GameObject GetUnit()
    {
        if (units.Length == 0) return null;
        return units[Random.Range(0, units.Length)];
    }

    public int UnitAlive { get; private set; }
    public int UnitKilled { get; private set; }
    public void AddAlive()
    {
        UnitAlive++;
        infoUI.Refresh(this);
    }
    public void AddKilled()
    {
        UnitAlive--;
        UnitKilled++;
        infoUI.Refresh(this);
    }

    [SerializeField] private UnityEvent OnDeathEvent;

    private void OnDestroy()
    {
        OnDeathEvent?.Invoke();
    }

    void Start()
    {
        UnitAlive = 0;
        UnitKilled = 0;
        
        infoUI.Refresh(this);
    }
}
