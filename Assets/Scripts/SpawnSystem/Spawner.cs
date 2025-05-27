using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class ParentSpawner : MonoBehaviour
{
    [SerializeField] private List<LocationSpawnInfo> spawnableContainer;
    
    private Dictionary<GameObject, ObjectPool<MonoBehaviour>> _pool;
    [SerializeField] private int poolDefaultSize;
    [SerializeField] private bool arePoolsExpandable;
    
    private List<RandomizedSpawnPoint> _randomizedSpawnPoints;
    
    private Dictionary<LocationSpawnInfo, List<RandomizedSpawnPoint>> sp;
    
    private List<ScriptedSpawnPoint> _scriptedSpawnPoints;
    
    private Coroutine _randomizedSpawnCoroutine;
    private Coroutine _scriptedSpawnCoroutine;
    
    private bool _isAlreadySpawningRandomized;
    private bool _isAlreadySpawningScripted;
    
    private int _currentSpawnableIndex;
    private HashSet<GameObject> _uniqueSpawnablesHashes;
    
    private void Start()
    {
        _pool = new Dictionary<GameObject, ObjectPool<MonoBehaviour>>();
        _uniqueSpawnablesHashes = new HashSet<GameObject>();
        sp = new Dictionary<LocationSpawnInfo, List<RandomizedSpawnPoint>>();
        _isAlreadySpawningRandomized = false;
        _isAlreadySpawningScripted = false;
        
        _randomizedSpawnPoints = new List<RandomizedSpawnPoint>(GetComponentsInChildren<RandomizedSpawnPoint>());
        _scriptedSpawnPoints = new List<ScriptedSpawnPoint>(GetComponentsInChildren<ScriptedSpawnPoint>());
        
        foreach (LocationSpawnInfo info in spawnableContainer)
        {
            sp.Add(info, _randomizedSpawnPoints.Where(spawnPoint => spawnPoint.SpawnType == info.SpawnType).ToList());
        }

        CreatePools();
    }
    
    private void Update()
    {
        //TODO видалити коментар
        //трігер спавну - затичка
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!_isAlreadySpawningRandomized && !_isAlreadySpawningScripted)
            {
                _isAlreadySpawningRandomized = true;
                _isAlreadySpawningScripted = true;
                if (_randomizedSpawnPoints != null)
                {
                    _randomizedSpawnCoroutine = StartCoroutine(RandomizedSpawnerCourotine(SpawnZoneTypes.Zone1));
                    _scriptedSpawnCoroutine = StartCoroutine(ScriptedSpawnerCourotine(SpawnZoneTypes.Zone1));
                }
            }
        }
    }

    private void CreatePools()
    {
        InitializeUniqueSpawnables();
    }

    private void InitializeUniqueSpawnables()
    {
        _uniqueSpawnablesHashes.Clear();
        List<GameObject> spawnablesWithDuplicates = new List<GameObject>();
        foreach (LocationSpawnInfo info in spawnableContainer)
        {
            spawnablesWithDuplicates.AddRange(info.GetSpawnablePrefabsList());
        }
        
        foreach (GameObject obj in spawnablesWithDuplicates)
        {
            if (!_uniqueSpawnablesHashes.Contains(obj))
            {
                _uniqueSpawnablesHashes.Add(obj);
                _pool.Add(obj, new ObjectPool<MonoBehaviour>(obj.GetComponent<Spawnable>(), arePoolsExpandable, poolDefaultSize));
            }
        }
    }
    
    public void TrySpawn(SpawnZoneTypes type)
    {
        if (!_isAlreadySpawningRandomized)
        {
            _isAlreadySpawningRandomized = true;
                //_isAlreadySpawningScripted = true;  && !_isAlreadySpawningScripted
            if (_randomizedSpawnPoints != null)
            {
                _randomizedSpawnCoroutine = StartCoroutine(RandomizedSpawnerCourotine(type));
               // _scriptedSpawnCoroutine = StartCoroutine(ScriptedSpawnerCourotine(type));
            }
        }
    }
    
    private IEnumerator RandomizedSpawnerCourotine(SpawnZoneTypes zone)
    {
        foreach (KeyValuePair<LocationSpawnInfo, List<RandomizedSpawnPoint>> pair in sp)
        {
            List<RandomizedSpawnPoint> rSpawnPoints = pair.Value;
            ObjectPool<MonoBehaviour> outpool;
            foreach (RandomizedSpawnPoint spawnPoint in rSpawnPoints)
            {
                if (spawnPoint.SpawnZone == zone)
                {
                    _pool.TryGetValue(pair.Key.GetSpawnable, out outpool);
                    outpool.GetElement(spawnPoint.transform.position); 
                    yield return null;
                }
            }
        }
        Debug.Log("_isAlreadySpawningRandomized = false;");
        _isAlreadySpawningRandomized = false;
    }
    
    private IEnumerator ScriptedSpawnerCourotine(SpawnZoneTypes zone)
    {
        foreach (ScriptedSpawnPoint spawnPoint in _scriptedSpawnPoints)
        {
            if (spawnPoint.SpawnZone == zone)
            {
                spawnPoint.TrySpawn();
                yield return null;
            }
        }
        
        _isAlreadySpawningScripted = false;
    }
}