using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationTypeSpawnInfo", menuName = "Gameplay/New LocationTypeSpawnInfo")]
public class LocationSpawnInfo : ScriptableObject
{
    [SerializeField]private SpawnPointTypes spawnType;
    public SpawnPointTypes SpawnType => spawnType;
    
    [SerializeField]private SpawnableTypes spawnableType;
    public SpawnableTypes SpawnableType => spawnableType;
    
    [Tooltip("To be used with the default spawn algorithm (Vose's) \nit is recommended for all the values to have the sum of 1")]
    [SerializeField] private List<SpawnableChanceToSpawn> spawnableList;
    
    private WeightedSpawnAlgo _spawnAlgo;

    public GameObject GetSpawnable => spawnableList[_spawnAlgo.PickValue()].Spawnable;
    public int SpawnableCount => spawnableList.Count;
    
    private void OnEnable()
    {
        _spawnAlgo = new WeightedSpawnAlgo(spawnableList.Select(enemy => enemy.SpawnChance).ToList());
    }

    public List<GameObject> GetSpawnablePrefabsList()
    {
        List<GameObject> prefabs = new List<GameObject>();
        foreach (SpawnableChanceToSpawn spawnable in spawnableList)
        {
            prefabs.Add(spawnable.Spawnable);
        }

        Debug.Log(spawnableList.Count);
        Debug.Log(prefabs.Count);
        return prefabs;
    }
}


