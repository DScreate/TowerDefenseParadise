using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public Camera cam;

    public NavMeshAgent agent;

    public Transform goalTransform;

    public int health = 100;
    public int creditValue = 100;

    NavMeshPath path;

    float nextPathUpdate;

    void Start ()
    {
        path = new NavMeshPath();
    }



	void Update ()
    {
        if (Time.time >= nextPathUpdate && goalTransform != null)
        {
            SetPathToGoal(goalTransform.position);

            nextPathUpdate = Time.time + 1f;
        }

        path = agent.path;

        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);

        CheckPath(agent.path);
    }

    public bool CheckPath(NavMeshPath path)
    {
        bool ret = path.status != NavMeshPathStatus.PathPartial;

        if (!ret)
        {
            Debug.Log("Destination Impossible");
        }

        return ret;
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        SpawnController.spawnController.enemies.Remove(this);

        GameManager.AddMoney(creditValue);

        Destroy(gameObject);
    }

    public void SetPathToGoal(Vector3 goal)
    {
        agent.SetDestination(goal);
    }
}
