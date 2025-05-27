using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedSpawnPoint : SpawnPoint
{
    [SerializeField] private GameObject spawnable;

    public GameObject Spawnable => spawnable;

    public void TrySpawn()
    {
        if (CanSpawn())
        {
            Instantiate(spawnable, transform.position, Quaternion.identity);
        }
    }
}
