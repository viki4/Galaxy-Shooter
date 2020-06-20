using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _asteroidPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _asteroidContainer;
    [SerializeField]
    private GameObject[] powerups;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnAsteroidRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // spawn game object for every 5 seconds
    // create a coroutine of type IEnumerator -- yield Events
    IEnumerator SpawnEnemyRoutine()
    {
        // yield return null;  // going to wait one frame and the next line is called

        //  yield return new WaitForSeconds(5.0f); // going to wait for five seconds and the next line will be called

        // while loop(Infinite loop)
            // Instantiate enemy prefab
            // wait for five seconds

        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.2f, 9.2f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(3.0f);
        }

     

    }

    IEnumerator SpawnPowerupRoutine()
    {
        // every 3 to 7 seconds spawn a powerup

        while(_stopSpawning == false)
        {
           
            Vector3 posToSpawn = new Vector3(Random.Range(-9.2f, 9.2f), 7, 0);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
                
        }
        
    }

    IEnumerator SpawnAsteroidRoutine()
    {
        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.2f, 9.2f), 7, 0);
            yield return new WaitForSeconds(Random.Range(15, 20));
            GameObject newAsteroid = Instantiate(_asteroidPrefab, posToSpawn, Quaternion.identity);
            newAsteroid.transform.parent = _asteroidContainer.transform;

        }
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
