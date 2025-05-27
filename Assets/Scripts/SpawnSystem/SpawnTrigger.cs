using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] private SpawnZoneTypes spawnZone = SpawnZoneTypes.Zone1;
    
    public SpawnZoneTypes SpawnZone => spawnZone;

    [SerializeField] private ParentSpawner spawner;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("1");
        if (other.GetComponentInParent<PlayerSingleton>())
        {
            Debug.Log("2");
            if (spawner != null)
            {
                Debug.Log("3");
                spawner.TrySpawn(spawnZone); // Call the method in the Spawner
                gameObject.SetActive(false);
            }
        }
    }
}
