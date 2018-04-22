using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour {

	public virtual void OnObjectReuse()
    {
        transform.localScale = Vector3.one;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public virtual void Destroy()
    {
        gameObject.SetActive(false);

        //PoolManager.poolManager.EnqueueObject(gameObject);
    }
}
