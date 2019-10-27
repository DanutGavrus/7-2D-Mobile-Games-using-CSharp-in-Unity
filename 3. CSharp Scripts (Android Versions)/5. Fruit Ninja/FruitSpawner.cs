using System.Collections;
using UnityEngine;

public class FruitSpawner : MonoBehaviour {
    /* Control the aparition and functionability of the blade. */
    // Public Game components:
    public GameObject fruitPrefab;
	public GameObject secondFruitPrefab;
    public GameObject bombPrefab;
    public GameObject lifePrefab;
    public Transform[] spawnPoints;
	public Transform[] spawnPointsSecondFruit;
    public Transform[] spawnPointsBomb;
    // Private Game Components:
    private Transform spawnPoint;
    private Transform spawnPointSecondFruit;
    private Transform spawnPointBomb;
    private GameObject spawnedFruit;
    private GameObject spawnedSecondFruit;
    private GameObject spawnedBomb;
    private GameObject spawnedLife;
    // private GameObject lifeObject; DECLARED IN THE STATIC METHOD IEnumerator LifeHit
    // Pulic Variables:
    public float minDelay = 0.1f;
    public float maxDelay = 1f;
    public static bool LifeSoundEnded;
    // Private Variables:
    private float delay;
    private float delaySecondFruit;
    private int spawnIndex;
    private int spawnIndexSecondFruit;
    private int spawnIndexBomb;
    
    private void Start ()
    {
        LifeSoundEnded = false;
        StartCoroutine(SpawnFruits());
        StartCoroutine(SpawnSecondFruits());
        StartCoroutine(SpawnBombs());
        StartCoroutine(SpawnLives());
        StartCoroutine(BombHit());
	}
	
	private IEnumerator SpawnFruits()
    {
        while(true) // Make the coroutine run as long as the game
        {
            // Randomly change a delay between fruits
            delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
            // Randomly select a spawn point
            spawnIndex = Random.Range(0, spawnPoints.Length);
            spawnPoint = spawnPoints[spawnIndex];
            // Spawn new fruit, if in 3 second it was not sliced, destroy it(after 2.6 seconds it is just under the screen
            spawnedFruit = Instantiate(fruitPrefab, spawnPoint.position, spawnPoint.rotation);
            Destroy(spawnedFruit, 3f);
        }
    }

    private IEnumerator SpawnSecondFruits()
    {
        while (true) // Make the coroutine run as long as the game
        {
            // At each 5 seconds spawn 2 secondFruit from random left or right
            // Randomly select a spawn point for the first fruit
            spawnIndexSecondFruit = Random.Range(0, spawnPointsSecondFruit.Length);
            spawnPointSecondFruit = spawnPointsSecondFruit[spawnIndexSecondFruit];
            yield return new WaitForSeconds(5f);
            spawnedSecondFruit = Instantiate(secondFruitPrefab, spawnPointSecondFruit);
            Destroy(spawnedSecondFruit, 3f);
            // Randomly select a spawn point for the second fruit
            spawnIndexSecondFruit = spawnPointsSecondFruit.Length - 1 - spawnIndexSecondFruit;
            spawnPointSecondFruit = spawnPointsSecondFruit[spawnIndexSecondFruit];
            delaySecondFruit = Random.Range(0.5f, maxDelay);
            yield return new WaitForSeconds(delaySecondFruit);
            spawnedSecondFruit = Instantiate(secondFruitPrefab, spawnPointSecondFruit);
            Destroy(spawnedSecondFruit, 3f);
        }
    }

    private IEnumerator SpawnBombs()
    {
        while (true)
        {
            // At each 10 seconds spawn a bomb from random up left or up right
            // Randomly select a spawn point
            spawnIndexBomb = Random.Range(0, spawnPointsBomb.Length);
            spawnPointBomb = spawnPointsBomb[spawnIndexBomb];
            yield return new WaitForSeconds(10f);
            spawnedBomb = Instantiate(bombPrefab, spawnPointBomb);
            Destroy(spawnedBomb, 3f);

        }
    }

    private IEnumerator SpawnLives()
    {
        while (true)
        {
            // At each 15 seconds spawn a life from the last bombIndex if we have at least one life lost
            yield return new WaitForSeconds(15f);
            if (GameManager.Missed != 0)
            {
                spawnedLife = Instantiate(lifePrefab, spawnPointBomb);
                Destroy(spawnedLife, 3f);
            }
        }
    }

    private IEnumerator BombHit()
    {
        while(true)
        {
            if (Bomb.BombHit)
            {
                yield return new WaitForSeconds(0.5f);
                Bomb.BombReloadLevel = true;
            }
            else yield return new WaitForSeconds(0.05f);
        }
    }

    public static IEnumerator LifeHit()
    {
        if (Life.LifeHit)
        {
            yield return new WaitForSeconds(1f);
            GameObject lifeObject = GameObject.Find("Blade");
            lifeObject.GetComponent<AudioSource>().enabled = false;
        }
    }
}