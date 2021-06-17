using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Start/Stop Game, manage spawn of enemies and items and keep track of score
/// </summary>
public class Spawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public List<GameObject> itemPrefabs1;
    public List<GameObject> itemPrefabs2;
    public List<GameObject> itemPrefabs3;
    public List<GameObject> aliveEnemies;
    public List<GameObject> activeItems;
    public bool gameActive = false;

    public float spawnDistance = 10;

    public int startingEnemies = 2;
    public float enemySpawnInterval = 5;
    private float enemySpawnIntervalModified;
    private float enemySpawnTimer;
    private float enemySpeedboost = 0.0f;

    public int startingItems = 1;
    public float itemSpawnInterval = 8;
    private float itemSpawnIntervalModified;
    private float itemSpawnTimer;

    private float gameSurviveTimer = 0;
    public int enemiesKilled = 0;

    ToggleMenu menuToggle;
    void Start()
    {
        menuToggle = GetComponent<ToggleMenu>();
    }

    void Update()
    {
        //Debug start
        if (Input.GetKeyDown(KeyCode.S)) StartGame();

        //If game is going
        if (gameActive)
        {
            //Update Times
            enemySpawnTimer += Time.deltaTime;
            itemSpawnTimer += Time.deltaTime;
            gameSurviveTimer += Time.deltaTime;

            //Limit to a maximum of 200 enemies alive to avoid impacting performance
            if (aliveEnemies.Count < 200)
            {
                //Update enemy spawn modifiers
                enemySpawnIntervalModified = Mathf.Lerp(5f, 0.3f, gameSurviveTimer / 120f);//shorten inerval based on time survived
                enemySpeedboost = Mathf.Lerp(0.0f, 0.4f, gameSurviveTimer / 120f);//increse speed boost based on time survived
                //Spawn enemies
                while (enemySpawnTimer >= enemySpawnIntervalModified)
                {
                    //randomize next enemy spwan intervals
                    enemySpawnTimer -= enemySpawnIntervalModified * Random.Range(1f, 1.2f);

                    GameObject enemyspawned = SpawnEnemy();
                    enemyspawned.GetComponent<Enemy>().speed += enemySpeedboost;
                    if (Random.Range(0.0f, 2.0f) < 0.2f)
                    {
                        enemyspawned = SpawnEnemy();
                        enemyspawned.GetComponent<Enemy>().speed += enemySpeedboost;
                    }
                }
            }

            //Limit to a maximum of 50 items active to avoid impacting performance
            if (activeItems.Count < 50)
            {
                //Spawn gun parts
                while (itemSpawnTimer >= itemSpawnIntervalModified)
                {
                    //next item spwan intervals
                    itemSpawnTimer -= itemSpawnIntervalModified * Random.Range(1f, 1.2f);

                    SpawnItemsGroup();
                }
            }
        }
    }

    public void StartGame()
    {
        //Reset game parametesr
        enemySpawnTimer = 0;
        itemSpawnTimer = 0;
        gameSurviveTimer = 0;
        enemiesKilled = 0;
        enemySpeedboost = 0;
        enemySpawnIntervalModified = enemySpawnInterval;
        itemSpawnIntervalModified = itemSpawnInterval;

        //Start Game
        gameActive = true;
        menuToggle.DisableMenu();

        //Spawn starting enemies
        foreach (var i in Enumerable.Range(0, startingEnemies)) SpawnEnemy();
        //Spawn startig gun parts
        foreach (var i in Enumerable.Range(0, startingItems)) SpawnItemsGroup();
    }

    public void StopGame()
    {
        //Inturrupt Spawner
        gameActive = false;

        //Destroy all enemies
        foreach (GameObject enemy in aliveEnemies.ToArray())
        {
            //Pass in isGameOver as true so that death are not added to score
            enemy.GetComponent<Enemy>().Die(isGameOver: true);
        }
        //Destroy all gun parts
        foreach (GameObject item in activeItems.ToArray())
        {
            item.GetComponent<Item>().Die();
        }

        //Enable Menu
        menuToggle.EnableMenu((int)gameSurviveTimer, enemiesKilled);
    }

    public GameObject SpawnEnemy()
    {
        //Select one random type of element
        //Included as I was initially planning to have multiple types of enemies but actually only one exist
        var enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        var angle = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
        var position = angle * Vector3.forward * spawnDistance * Random.Range(0.9f, 1.1f);

        GameObject enemyspawned = Instantiate(enemy, position, angle * Quaternion.AngleAxis(180, Vector3.up));
        aliveEnemies.Add(enemyspawned);
        return enemyspawned;
    }

    public void SpawnItemsGroup()
    {
        //Spawn 2 barrels
        SpawnItem(itemPrefabs1);
        SpawnItem(itemPrefabs1);

        //Spawn 2 magazines
        SpawnItem(itemPrefabs2);
        SpawnItem(itemPrefabs2);

        //Spawn 1 handle
        SpawnItem(itemPrefabs3);
    }

    private void SpawnItem(List<GameObject> prefabs)
    {
        if (prefabs.Count == 0) return;

        //Select one random item form the list
        var item = prefabs[Random.Range(0, prefabs.Count)];

        //Randmize angle and position
        var angle = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
        var position = angle * Vector3.forward * spawnDistance * Random.Range(0.03f, 0.09f);

        //Spawn item
        activeItems.Add(Instantiate(item, position + Vector3.up, angle));
    }
}
