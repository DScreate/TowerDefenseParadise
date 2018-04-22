using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {

    public Tower tower;

    public float turnSpeedConst = 1f;

    public Transform turretBase;

    public Transform target;

    public float nextFireTime;

    public Quaternion targetRotation;

    void Start()
    {

    }

    void Update()
    {
        

        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + (1 / tower.attackSpeed);

            target = FindClosestTarget();

            if (target != null)
            {
                float distance = Vector3.Distance(target.position, transform.position) * 1.6f;

                if(distance <= tower.range)
                {
                    

                    //Vector3 offset = target.position - transform.position;
                    //Vector3 axis = Vector3.forward;

                    //targetRotation = Quaternion.LookRotation(offset, axis);
                    //turretBase.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeedConst * Time.deltaTime);
                }
            }
        }

        if(target != null)
        {
            float angle = Vector3.Angle(transform.position, target.position);

            Debug.Log("angle: " + angle);

            turretBase.rotation = Quaternion.Euler(new Vector3(0, angle));
        }
    }

    private Transform FindClosestTarget()
    {
        Transform closest = null;
        float distance = 0, closestDist = 0;

        foreach(EnemyController enemy in SpawnController.spawnController.enemies)
        {
            distance = Vector3.Distance(enemy.transform.position, transform.position) * 1.6f;

            //Debug.Log("Distance: " + distance);

            if(distance < closestDist || closest == null)
            {
                closest = enemy.transform;
                closestDist = Vector3.Distance(enemy.transform.position, transform.position) * 1.6f;
            }
        }

        return closest;
    }
}
