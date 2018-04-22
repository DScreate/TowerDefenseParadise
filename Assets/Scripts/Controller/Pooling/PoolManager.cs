using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    private static readonly int defaultPoolSize = 10;
    private static Dictionary<string, Queue<ObjectInstance>> poolDictionary = new Dictionary<string, Queue<ObjectInstance>>();
    private static Dictionary<string, GameObject> poolHolders = new Dictionary<string, GameObject>();
    private static PoolManager _poolManager;

    public static PoolManager poolManager
    {
        get
        {
            if (_poolManager == null)
                _poolManager = FindObjectOfType<PoolManager>();

            return _poolManager;
        }
    }

    public static void CreatePool(GameObject prefab, int poolSize, GameObject parent)
    {
        string poolKey = prefab.name.Replace("(Clone)", "");
        if (poolSize < 1)
            poolSize = defaultPoolSize;

        if (!poolDictionary.ContainsKey(poolKey)) {
            poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

            //Debug.Log("pool created key = " + poolKey);

            /*GameObject poolHolder = null;
            if (parent != null)
            {
                //poolHolder = new GameObject(prefab.name + "PoolHolder");
                //poolHolder.transform.parent = transform;
            }*/

            for (int i = 0; i < poolSize; i++)
            {
                /*ObjectInstance newObj = new ObjectInstance(Instantiate(prefab) as GameObject);
                poolDictionary[poolKey].Enqueue(newObj);
                if (parent != null)
                {
                    newObj.SetParent(parent);
                }*/
                CreatePoolObject(prefab, parent);
            }
        }
    }

    public static bool HasPool(GameObject prefab)
    {
        string poolKey = prefab.name.Replace("(Clone)", "");

        return poolDictionary.ContainsKey(poolKey);
    }

    public static GameObject GetPoolHolder(GameObject prefab)
    {
        string poolKey = prefab.name.Replace("(Clone)", "") + "PoolHolder";
        if (!poolHolders.ContainsKey(poolKey))
        {
            Debug.Log(poolKey + " not found");
            return null;
        }

        return poolHolders[poolKey];
    }

    public static void CheckForPool(GameObject prefab, int defaultSize)
    {
        if (!HasPool(prefab))
        {
            string name = prefab.name + "PoolHolder";
            GameObject poolHolder = new GameObject(name);
            poolHolder.transform.parent = poolManager.transform;
            poolHolders.Add(name, poolHolder.gameObject);
            CreatePool(prefab, defaultSize, poolHolder);
        }
    }

    /*public GameObject ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return ReuseObject(prefab, position, rotation, true);
    }*/

    public static void CreatePoolObject(GameObject prefab, GameObject parent)
    {
        string poolKey = prefab.name.Replace("(Clone)", "");
        ObjectInstance newObj = new ObjectInstance(Instantiate(prefab) as GameObject);
        poolDictionary[poolKey].Enqueue(newObj);
        if (parent != null)
        {
            newObj.SetParent(parent.transform);
        }
    }

    public static GameObject ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation, bool enqueue = false, bool active = true)
    {
        string poolKey = prefab.name.Replace("(Clone)", "");

        if (poolDictionary.ContainsKey(poolKey))
        {
            if (poolDictionary[poolKey].Count > 0)
            {
                ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
                if(enqueue)
                    poolDictionary[poolKey].Enqueue(objectToReuse);
                objectToReuse.Reuse(position, rotation, active);

                //Debug.Log(objectToReuse.gameObject);
                return objectToReuse.gameObject;
            }
            else
            {
                CreatePoolObject(prefab, poolHolders[poolKey + "PoolHolder"]);
                ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
                if (enqueue)
                    poolDictionary[poolKey].Enqueue(objectToReuse);
                objectToReuse.Reuse(position, rotation, active);

                return objectToReuse.gameObject;
            }
        }

        Debug.Log("Reuse Object is returning null!");
        return null;
    }

    public static void EnqueueObject(GameObject prefab)
    {
        if (prefab != null)
        {
            string poolKey = prefab.name.Replace("(Clone)", "");

            if (poolDictionary.ContainsKey(poolKey))
            {
                ObjectInstance newObj = new ObjectInstance(prefab);
                Queue<ObjectInstance> pool = poolDictionary[poolKey];

                if (!pool.Contains(newObj))
                {
                    //Debug.Log(newObj.gameObject.name + " added to pool");
                    pool.Enqueue(newObj);
                }
                //else
                    //Debug.Log(newObj.gameObject.name + " already in pool!");
                //gameObject.SetActive(false);
            }
            else
            {
                string holderName = poolKey + "PoolHolders";
                GameObject poolHolder = new GameObject(holderName);
                poolHolders.Add(holderName, poolHolder);
                CreatePool(prefab, defaultPoolSize, poolHolder);
            }
        }
    }

    public class ObjectInstance  : IEquatable<ObjectInstance> {

        public GameObject gameObject;

        private Transform _transform;
        private PoolObject poolObjectScript = null; 

        public ObjectInstance(GameObject objectInstance)
        {
            gameObject = objectInstance;
            _transform = gameObject.transform;
            gameObject.SetActive(false);

            if(gameObject.GetComponent<PoolObject>() != null)
            {
                poolObjectScript = gameObject.GetComponent<PoolObject>();
            }
        }

        public void Reuse(Vector3 position, Quaternion rotation, bool active)
        {
            gameObject.SetActive(active);
            _transform.position = position;
            _transform.rotation = rotation;

            if(poolObjectScript != null)
            {
                poolObjectScript.OnObjectReuse();
            }
        }

        public void SetParent(Transform parent)
        {
            //_transform.parent = parent;
            _transform.SetParent(parent, false);
        }

        public bool Equals(ObjectInstance other)
        {
            if (gameObject == null || other == null || other.gameObject == null)
                return false;

            return gameObject.Equals(other.gameObject);
        }
    }

    
}
