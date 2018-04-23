using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour {

    public TowerController[] baseTowerControllers;
    public Dictionary<string, Tower> baseTowers; 

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

    private void Awake()
    {
        baseTowers = new Dictionary<string, Tower>();

        foreach(TowerController towerContr in baseTowerControllers)
        {
            Debug.Log(towerContr.tower.name);
            baseTowers.Add(towerContr.tower.name, Instantiate(towerContr.tower));
        }
    }

    public static Tower CreateTower(Tower tower)
    {
        return towerFactory.baseTowers[tower.name];
    }

    public TowerController BuildBaseTower(int index, Vector3 position, Quaternion rotation)
    {
        if (GameManager.money < baseTowerControllers[index].tower.buildCost)
            return null;

        TowerController tower = Instantiate(baseTowerControllers[index], position, rotation, transform);

        // Can do stuff here before returning tower to game.

        return tower;
    }

}
