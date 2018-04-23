using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static int money = 2000;
    public static int lives = 20;
    public static int chapter = 1;
    public static int kills = 0;

    private static bool created = false;

    private static GameObject towerDefenseHolder;

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

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(gameObject);
            created = true;
            towerDefenseHolder = transform.Find("TowerDefenseHolder").gameObject;
            //Debug.Log("Awake: " + gameObject);
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

    public static void LoseLife()
    {
        lives--;

        if(lives <= 0)
        {
            LoseLevel();
        }
    }

    public static void StartLevel()
    {
        SpawnController.StartLevel();

        towerDefenseHolder.SetActive(true);
    }

    public static void LoseLevel()
    {
        lives = 20;

        SpawnController.spawnController.OnLevelLose();

        towerDefenseHolder.SetActive(false);

        SceneManager.LoadScene("LoseScene");
    }

    public void NextLevel()
    {
        StartCoroutine(UpdateLevel());

        //SpawnController.StartLevel();
    }

    IEnumerator UpdateLevel()
    {
        yield return new WaitForSeconds(3);

        chapter++;

        lives = 20;

        towerDefenseHolder.SetActive(false);

        SceneManager.LoadScene("Chap2");
    }
}
