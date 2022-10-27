using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    public GameObject[] obstaclePrefabs, environmentPrefabs, collectablePrefabs;
    public GameObject obstacleClone, environmentClone, collectableClone;
    private Vector3 _obstacleSpawnPos = new Vector3(0, 2, 0);
    private Vector3 _environmentSpawnPos = new Vector3(13, -10, 0);
    private Vector3 _collectableSpawnPos = new Vector3(0, 1, 0);
    private readonly List<GameObject> _spawnedObstacles = new List<GameObject>();
    private readonly List<GameObject> _spawnedEnvironments = new List<GameObject>();
    private readonly List<GameObject> _spawnedCollectables = new List<GameObject>();
    private const float RepeatRate = 2, StartDelay = 5f;
    public float gameSpeed = 10;
    public float difficultyMultiplier = 1;
    private bool _isPlayerDead;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        InvokeRepeating(nameof(SpawnObstacle), StartDelay, RepeatRate);
        InvokeRepeating(nameof(SpawnEnvironments), StartDelay / 5, RepeatRate);
        InvokeRepeating(nameof(SpawnCollectables), StartDelay, RepeatRate / 2);
    }

    // Update is called once per frame
    public void Update()
    {
        _isPlayerDead = ColorChange.Instance.deathStatus;
        if (!ColorChange.Instance.deathStatus)
        {
            foreach (var o in _spawnedObstacles)
            {
                o.transform.Translate(Vector3.back * (Time.deltaTime * gameSpeed));
                if (o.transform.position.z < -0.5)
                {
                    Destroy(o);
                    _spawnedObstacles.Remove(o);
                }
            }

            foreach (var e in _spawnedEnvironments)
            {
                e.transform.Translate(Vector3.back * (Time.deltaTime * gameSpeed));
                if (e.transform.position.z < -2)
                {
                    Destroy(e);
                    _spawnedEnvironments.Remove(e);
                }
            }

            foreach (var c in _spawnedCollectables)
            {
                c.transform.Translate(Vector3.up * (Time.deltaTime * gameSpeed));
                if (c.transform.position.z < -2)
                {
                    Destroy(c);
                    _spawnedCollectables.Remove(c);
                }
            }
        }
    }

    private void SpawnObstacle()
    {
        var willBeSpawnObstacle = Random.Range(0, 4);
        if (willBeSpawnObstacle == 3)
        {
            _obstacleSpawnPos.y = 0.25f;
            _obstacleSpawnPos.z = _spawnedObstacles.Last().transform.position.z + Random.Range(20, 40);
        }
        else
        {
            if (obstacleClone)
            {
                _obstacleSpawnPos.z = _spawnedObstacles.Last().transform.position.z + Random.Range(20, 40);
                _obstacleSpawnPos.y = 2;
            }
            else
            {
                _obstacleSpawnPos.y = 2;
                _obstacleSpawnPos.z = Random.Range(30, 60);
            }
            // foreach (var obstacle in _spawnedObstacles.Where(obstacle => _obstacleSpawnPos.z- obstacle.transform.position.z <= 15))
            // {
            //     _obstacleSpawnPos.z += 15;
            // }
        }

        if (_isPlayerDead) return;
        obstacleClone = Instantiate(obstaclePrefabs[willBeSpawnObstacle], _obstacleSpawnPos,
            obstaclePrefabs[willBeSpawnObstacle].transform.rotation);
        var spawnedObstacle = obstacleClone;
        _spawnedObstacles.Add(spawnedObstacle);
    }

    private void SpawnEnvironments()
    {
        var environmentRandX = Random.Range(0, 2);
        if (environmentRandX == 1)
        {
            _environmentSpawnPos.x = 16;
        }
        else
        {
            _environmentSpawnPos.x = -16;
        }

        if (environmentClone)
        {
            _environmentSpawnPos.z = Random.Range(40, 50) + (environmentClone.transform.position.z + 20);
        }
        else
        {
            _environmentSpawnPos.z = Random.Range(40, 50);
        }

        var willBeSpawnEnvironment = Random.Range(0, 10);
        if (_isPlayerDead) return;
        environmentClone = Instantiate(environmentPrefabs[willBeSpawnEnvironment], _environmentSpawnPos,
            environmentPrefabs[willBeSpawnEnvironment].transform.rotation);
        GameObject spawnedEnvironment = environmentClone;
        _spawnedEnvironments.Add(spawnedEnvironment);
    }

    private void SpawnCollectables()
    {
        var collectableRandX = Random.Range(0, 3);
        switch (collectableRandX)
        {
            case 0:
                _collectableSpawnPos.x = 2;
                break;
            case 1:
                _collectableSpawnPos.x = 0;
                break;
            case 2:
                _collectableSpawnPos.x = -2;
                break;
        }

        if (collectableClone)
        {
            _collectableSpawnPos.z = Random.Range(40, 50) + (collectableClone.transform.position.z + 5);
        }
        else
        {
            _collectableSpawnPos.z = Random.Range(40, 50);
        }

        var willBeSpawnCollectable = Random.Range(0, 10);
        if (_isPlayerDead) return;
        collectableClone = Instantiate(collectablePrefabs[willBeSpawnCollectable], _collectableSpawnPos,
            collectablePrefabs[willBeSpawnCollectable].transform.rotation);
        var spawnedCollectable = collectableClone;
        _spawnedCollectables.Add(spawnedCollectable);
    }
}