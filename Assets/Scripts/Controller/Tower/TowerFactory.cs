using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour {

    public TowerController[] baseTowers;

    //private static bool created = false;

    private static TowerFactory _towerFactory;

    public static TowerFactory towerFactory
    {
        get
        {
            if (_towerFactory == null)
                _towerFactory = FindObjectOfType<TowerFactory>();

            return _towerFactory;
        }
    }

    /*private void Awake()
    {
        // (!created)
        //
            DontDestroyOnLoad(gameObject);
            //created = true;
            //Debug.Log("Awake: " + gameObject);
        //}
    }*/

    public static Tower CreateTower(Tower tower)
    {
        Tower ret = Instantiate(tower);

        return ret;
    }

    public TowerController BuildBaseTower(int index, Vector3 position, Quaternion rotation)
    {
        if (GameManager.money < baseTowers[index].tower.buildCost)
            return null;

        TowerController tower = Instantiate(baseTowers[index], position, rotation, transform);

        // Can do stuff here before returning tower to game.

        return tower;
    }

}
