using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    public Text senpaiPointsText;
    public Text livesText;
    public Text chapterText;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        senpaiPointsText.text = "$" + GameManager.money;
        livesText.text = "Lives: " + GameManager.lives;
        chapterText.text = "Chapter: " + GameManager.chapter;
    }

    public void OnTowerButtonClick(int index)
    {
        Debug.Log(index);

        if (TowerPlacer.towerPlacer.activeTowerIndex == index)
            TowerPlacer.towerPlacer.activeTowerIndex = -1;
        else
            TowerPlacer.towerPlacer.activeTowerIndex = index;
    }
}
