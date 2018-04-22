using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {

    public Tower tower;

    public float turnSpeedConst = 1f;

    public Transform turretBase;
    public Transform[] firingHarnesses;
    private int currentFiringHarness = 0;

    public Transform target;

    public float nextFireTime;

    public Quaternion targetRotation;

    void Start()
    {
        tower = TowerFactory.CreateTower(tower);

        PoolManager.CheckForPool(tower.bulletPrefab, 500);
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            //nextFireTime = Time.time + (1 / tower.attackSpeed);

            target = FindClosestTarget();

            float angle = RotateTurret();

            if (target != null)
            {
                float distance = Vector3.Distance(target.position, transform.position) * 1.6f;

                if(distance <= tower.range)
                {
                    Debug.DrawLine(firingHarnesses[currentFiringHarness].position, target.position);



                    FireBullet(angle);
                }
            }
        }
        else
        {
            RotateTurret();
        }
    }

    public void FireBullet(float angle)
    {
        /*
        //Debug.Log("Fire");
        //Vector3 eulers = lookRot.eulerAngles;
        Quaternion rot = Quaternion.Euler(0, lookRot, 0);

        BulletController temp = Instantiate(tower.bulletPrefab, (Vector3)position + lookRot * firingHarness.transform.position, rot).GetComponent<BulletController>();
        Rigidbody rigidbody = temp.GetComponent<Rigidbody>();
        temp.damage = tower.damage;

        rigidbody.velocity = rot * (Vector2.up * 10);*/

        Quaternion rot = Quaternion.Euler(0, angle, 90);
        //BulletController temp = Instantiate(tower.bulletPrefab, firingHarnesses[currentFiringHarness++].transform.position, Quaternion.Euler(0, angle, 0)).GetComponentInChildren<BulletController>();
        GameObject tempParent = PoolManager.ReuseObject(tower.bulletPrefab, firingHarnesses[currentFiringHarness++].transform.position, rot);
        BulletController temp = tempParent.GetComponentInChildren<BulletController>();
        temp.parent = tempParent;
        Rigidbody tempRbody = temp.GetComponentInChildren<Rigidbody>();
        temp.rbody = tempRbody;
        tempRbody.velocity = Quaternion.Euler(0, angle - 90, 0) * (Vector3.forward * tower.bulletVelocity);
        temp.tower = this;

        currentFiringHarness = currentFiringHarness % firingHarnesses.Length;

        /*if (firingHarnesses.Length >= currentFiringHarness)
            currentFiringHarness = 0;*/

        if (tower.attackSpeed != 0f)
            nextFireTime = Time.time + (1f / tower.attackSpeed);
    }

    private float RotateTurret()
    {
        if (target != null)
        {
            float angle = 180 - AngleBetweenVector2(new Vector2(transform.position.x, transform.position.z), new Vector2(target.position.x, target.position.z));

            //Debug.Log("angle: " + angle);

            turretBase.rotation = Quaternion.Euler(new Vector3(0, angle));

            return angle;
        }

        return 0;
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

        if (closestDist <= tower.range)
            return closest;
        else
            return null;
    }

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }
}
