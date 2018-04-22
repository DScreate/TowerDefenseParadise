using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour {

    public Tower[] baseTowers;

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



}
