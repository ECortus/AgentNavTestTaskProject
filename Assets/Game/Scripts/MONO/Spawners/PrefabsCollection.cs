using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Prefab Collection")]
public class PrefabsCollection : ScriptableObject
{
    public GameObject PlayerPrefab;
    public GameObject AllyPrefab;
    public GameObject EnemyPrefab;
}
