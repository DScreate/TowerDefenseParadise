using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision Trigger");
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Collision Trigger");

        Debug.Log(other);

        EnemyController enemy = other.transform.parent.gameObject.GetComponent<EnemyController>();

        Debug.Log(enemy);

        if (enemy != null)
        {
            enemy.Die();

            Debug.Log(enemy);
        }
    }
}
