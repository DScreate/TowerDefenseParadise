using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public int damage;

    public float lifetime = 2f;

    public TowerController tower;

    private float creationTime;

    private void Start()
    {
        creationTime = Time.time;
    }

    private void Update()
    {
        if (Time.time >= creationTime + lifetime || Vector3.Distance(tower.transform.position, transform.position) * 1.6f > tower.tower.range)
            Destroy(transform.parent.gameObject);
    }

    private void OnCollisionStay(Collision other)
    {
        //Debug.Log("Collision " + other.gameObject.name, other.gameObject);

        Transform otherParent = other.transform.parent;

        if (otherParent != null)
        {
            EnemyController enemy = otherParent.GetComponent<EnemyController>();

            if (enemy != null)
            {
                //Debug.Log("Collision Enemy");

                enemy.TakeDamage(damage);

                Destroy(gameObject);
            }
            else
            {
                //Destroy(gameObject);
            }
        }
    }

}
