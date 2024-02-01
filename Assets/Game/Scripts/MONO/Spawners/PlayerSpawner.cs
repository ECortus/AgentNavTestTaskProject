using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private PrefabsCollection _prefabsCollection;
    [SerializeField] private Transform spawnDot;
    
    [Inject] void Awake()
    {
        GameObject prefab = _prefabsCollection.PlayerPrefab;
        Instantiate(prefab, spawnDot.position, Quaternion.identity);
    }
}
