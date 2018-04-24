using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static int money = 2000;
    public static int lives = 20;
    public static int chapter = 1;
    public static int kills = 0;
    public static GameObject soundHolder;
    
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
            soundHolder = towerDefenseHolder .transform.Find("SoundHolder").gameObject;
            Random.InitState(System.DateTime.Now.Millisecond);
            //Debug.Log("Awake: " + gameObject);
        }
    }

    void Start ()
    {
		
	}

	void Update ()
    {

	}

    public static int GetRandomInt(int min, int max)
    {
        return Random.Range(min, max);
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

        SceneManager.LoadScene("Chap2_Start");
    }
}
