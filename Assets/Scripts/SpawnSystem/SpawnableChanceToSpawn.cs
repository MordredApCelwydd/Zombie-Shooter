using System;
using UnityEngine;

[Serializable]
public class SpawnableChanceToSpawn
{
    [SerializeField] private GameObject spawnable;
    public GameObject Spawnable => spawnable;
    
    [SerializeField] private double spawnChance;
    public double SpawnChance => spawnChance;
}
