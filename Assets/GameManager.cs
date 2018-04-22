using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static int money = 1000;

    private static GameManager _gameManager;

    public static GameManager gameManager
    {
        get
        {
            if (_gameManager == null)
                _gameManager = FindObjectOfType<GameManager>();

            return _gameManager;
        }
    }

	void Start ()
    {
		
	}

	void Update ()
    {
		
	}

    public static void AddMoney(int amount)
    {
        money += amount;
    }
}
