using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private SpawnZoneTypes spawnZone = SpawnZoneTypes.Zone1;
    
    public SpawnZoneTypes SpawnZone => spawnZone;
    
    [SerializeField] private bool isEndless = true;
    
    [SerializeField] private int spawnsRemaining = 0;

    public bool CanSpawn()
    {
        if (!isEndless && (spawnsRemaining <= 0))
        {
            return false;
        }
        
        spawnsRemaining--;
        return true;
    }
}
