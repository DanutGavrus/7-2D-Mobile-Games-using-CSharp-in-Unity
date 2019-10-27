using UnityEngine;

public class SpawnerManager : MonoBehaviour {
    /* Controls the spawn of platforms and monsters. */
    public static int platformCounter;

    public GameObject[] platformPrefabs; // 0 normal, 1 breakable, 2 higher
    public GameObject monsterPrefab;
    public int maxNrOfPlatforms;
    public float levelWidth;
    public float minY;
    public float maxY;
    public int monsterEachNrOfPlatfsMin;
    public int monsterEachNrOfPlatfsMax;

    private Vector3 spawnPosition;
    private int monsterEachNrOfPlatfs;
    private int monsterCounter;
    private bool randomPlatformEachTwo;

    private void Start()
    {
        monsterEachNrOfPlatfs = Random.Range(monsterEachNrOfPlatfsMin, monsterEachNrOfPlatfsMax);
        for (int i = 0; i < maxNrOfPlatforms; i++)
        {
            Spawner();
        }
    }

    private void Update()
    {
        if (platformCounter < maxNrOfPlatforms)
        {
            Spawner();
        }
    }
    
    private void Spawner()
    {
        SpawnPlatform();
        platformCounter++;
        monsterCounter++;
        if (monsterCounter == monsterEachNrOfPlatfs)
        {
            SpawnMonster();
            monsterCounter = 0;
            monsterEachNrOfPlatfs = Random.Range(monsterEachNrOfPlatfsMin, monsterEachNrOfPlatfsMax);
        }
    }

    private void SpawnPlatform()
    {
        GameObject platformGo;
        spawnPosition.y += Random.Range(minY, maxY);
        spawnPosition.x = Random.Range(-levelWidth, levelWidth);
        // Each one in two is a normal platform, other 50% are random between all types(normal inclusive). I wanted more normal platforms
        if (!randomPlatformEachTwo)
        {
            platformGo = Instantiate(platformPrefabs[0], spawnPosition, Quaternion.identity);
            randomPlatformEachTwo = true;
        }
        else 
        {
            platformGo = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)], spawnPosition, Quaternion.identity);
            randomPlatformEachTwo = false;
        }
        platformGo.transform.SetParent(transform);
    }

    private void SpawnMonster()
    {
        spawnPosition.y += Random.Range(minY, maxY);
        spawnPosition.x = Random.Range(-levelWidth, levelWidth);
        Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        spawnPosition.x *= -1f;
        Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        spawnPosition.x *= -1f;
    }
}
