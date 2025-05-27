using UnityEngine;
public class Spawnable : MonoBehaviour
{
    [SerializeField] private int spawnableID;
    public int SpawnableID => spawnableID;

    [SerializeField] private SpawnableTypes spawnableType;
    
    public SpawnableTypes  SpawnableType => spawnableType;
}