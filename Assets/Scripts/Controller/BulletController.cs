using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public int damage;

    public float lifetime = 2f;

    private float creationTime;

    private void Start()
    {
        creationTime = Time.time;
    }

    private void Update()
    {
        if (Time.time >= creationTime + lifetime)
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

                Destroy(gameObject);
            }
            else
            {
                //Destroy(gameObject);
            }
        }
    }

}
