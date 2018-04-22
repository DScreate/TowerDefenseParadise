using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SpawnController : MonoBehaviour {

    public Transform goalTransform;

    public EnemyController enemyController;

    public float spawnDelay, nextSpawn;

    public List<EnemyController> enemies = new List<EnemyController>();

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
        if (Time.time >= nextSpawn)
        {
            EnemyController enemy = Instantiate(enemyController);

            nextSpawn = Time.time + spawnDelay;

            enemy.SetPathToGoal(goalTransform.position);
            enemy.goalTransform = goalTransform;

            enemies.Add(enemy);
        }
    }

    public void PathEnemiesToGoal()
    {
        foreach(EnemyController enemy in enemies)
        {
            enemy.SetPathToGoal(goalTransform.position);
        }
    }
}
