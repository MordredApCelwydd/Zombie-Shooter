using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedSpawnPoint : SpawnPoint
{
    [SerializeField] private SpawnPointTypes spawnType;
    public SpawnPointTypes SpawnType => spawnType;
}
