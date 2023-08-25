using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AsteroidGrid : MonoBehaviour
{

   /* public TetrisBlockManager BlockManagerReference;
    [Description("What percentage of asteroids should spawn. 0f means no astroids, 1f means the grid is completely full")]
    [Range(0, 100)]
    [SerializeField] private int _asteroidFieldDensity;
    [SerializeField] private int _gridWidth; //the amount of asteroids that will spawn, width-wise
    [SerializeField] private Vector2 _bottomLeftSpawnPos; //the position that the bottom left asteroid will start at.
    [SerializeField] private int _gridDepth; // The number of rows that there will be in the asteroid field
    [Space(10)]
    //[SerializeField] private List<Asteroid> _asteroidPrefabs;
    [SerializeField] private List<AsteroidPattern> _asteroidPatternPrefabs;
    private List<AsteroidPattern> _spawnedAsteroids;
    public bool HasAstroids => _spawnedAsteroids.Count > 0;

    private int _currentSpawnRow;
    public static int ActiveAsteroidCount;
    public static System.Action<Asteroid, AsteroidPattern> OnAsteroidDestroyed;

    private void Start()
    {
        OnAsteroidDestroyed += AsteroidDestroyed;
    }

    private void AsteroidDestroyed(Asteroid asteroid, AsteroidPattern parent)
    {
        if (!parent.ShouldDestroy) return;

        Debug.Log("Destroy the Asteroid Pattern.");

        _spawnedAsteroids.Remove(parent);
        Destroy(parent.gameObject);

        if(_spawnedAsteroids.Count == 0)
        {
            WaveManager.OnAllAsteroidsDestroyed?.Invoke();
        }
    }

    private bool CanSpawnAstroid => Random.Range(0, 100) <= 100;

    public void GenerateGrid()
    {
        _spawnedAsteroids = new();

        for (int y = 0; y < _gridDepth; y++)
        {
            for (int x = 0; x < _gridWidth; x++)
            {
                if (CanSpawnAstroid)
                {
                    if (x == 1 && y == 0) continue;
                    AsteroidPattern selectedPrefab = GetRandomAsteroidPrefab();

                    Vector2 posDelta;

                    if (y % 2 == 0)
                    {
                        posDelta = new Vector2(x, y) * TetrisBlockManager.Instance.DistanceBetweenGridSpots;
                        if (y != 0) posDelta.y -= TetrisBlockManager.Instance.DistanceBetweenGridSpots / 1.5f;
                    }
                    else
                    {
                        posDelta = new Vector2(x, y) * TetrisBlockManager.Instance.DistanceBetweenGridSpots;
                        posDelta.x -= TetrisBlockManager.Instance.DistanceBetweenGridSpots / 1.5f;
                        posDelta.y -= TetrisBlockManager.Instance.DistanceBetweenGridSpots / 1.5f;
                    }

                    Vector2 thisAsteroidPos = _bottomLeftSpawnPos + posDelta;
                    //Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
                    Quaternion randomRotation = Quaternion.identity;

                    var spawnedAsteroid = Instantiate(selectedPrefab, thisAsteroidPos, randomRotation, transform);
                    *//*spawnedAsteroid.SpawnLocation = thisAsteroidPos;
                    spawnedAsteroid.Activate = false;*//*
                    _spawnedAsteroids.Add(spawnedAsteroid);
                }
            }
        }
    }
    private AsteroidPattern GetRandomAsteroidPrefab() //right now this is simple - you can upgrade it to make some asteroids rarer than others
    {
        int randomIndex = Random.Range(0, _asteroidPatternPrefabs.Count);
        return _asteroidPatternPrefabs[randomIndex];
    }
    public Vector2 GetNearestPointOnGrid(Vector2 pos)
    {
        return _spawnedAsteroids.OrderBy(a => Vector2.Distance(a.transform.position, pos)).First().transform.position;
    }

    public void SpawnAsteroids(int count)
    {
        while (count-- != 0)
        {
            AsteroidPattern asteroid = GetRandomAsteroid();
            //asteroid.Activate = true;
        }
    }

    private AsteroidPattern GetRandomAsteroid()
    {
        AsteroidPattern asteroid;
        do
        {
            asteroid = _spawnedAsteroids[Random.Range(0, _spawnedAsteroids.Count)];
        } while (asteroid.isActiveAndEnabled);
        return asteroid;
    }

    void OnDrawGizmos() //visualize where the grid will go
    {
        if (BlockManagerReference == null)
        {
            Debug.LogError("drag and drop the tetrisBlockManager in the inspector. We can't use it's singleton since this code runs in edit mode");
            return;
        }

        for (int i = 0; i < _gridWidth; i++)
        {
            Gizmos.color = Color.red;
            Vector2 thisAsteroidPos = _bottomLeftSpawnPos + new Vector2(BlockManagerReference.DistanceBetweenGridSpots * i, 0);
            Gizmos.DrawSphere(thisAsteroidPos, .7f);
            Gizmos.DrawLine(thisAsteroidPos, thisAsteroidPos + new Vector2(0, 5));
        }
    }*/

}
