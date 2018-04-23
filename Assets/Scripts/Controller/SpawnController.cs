using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SpawnController : MonoBehaviour {

    public Transform goalTransform;

    public EnemyController enemyController;

    public static int spawnCount = 10000;

    public static float spawnDelay = 1, nextSpawn;

    public List<EnemyController> enemies = new List<EnemyController>();

    public Transform enemyHolder;

    private static SpawnController _spawnController;

    public static SpawnController spawnController
    {
        get
        {
            if (_spawnController == null)
                _spawnController = FindObjectOfType<SpawnController>();

            return _spawnController;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Time.time >= nextSpawn && spawnCount > 0)
        {
            EnemyController enemy = Instantiate(enemyController, enemyHolder);

            nextSpawn = Time.time + spawnDelay;

            enemy.SetPathToGoal(goalTransform.position);
            enemy.goalTransform = goalTransform;

            enemies.Add(enemy);

            spawnCount--;
        }

        if (spawnCount == 0 && enemies.Count == 0)
        {
            spawnCount = -1;
            GameManager.gameManager.NextLevel();
        }
    }

    public void PathEnemiesToGoal()
    {
        foreach(EnemyController enemy in enemies)
        {
            enemy.SetPathToGoal(goalTransform.position);
        }
    }

    public static void StartLevel()
    {
        spawnCount = GameManager.chapter * 50;
        nextSpawn = Time.time + 15f;
    }

    public void OnLevelLose()
    {
        foreach(EnemyController enemy in enemies)
        {
            enemy.Die(false, false, false);
        }

        enemies.Clear();
    }
}
