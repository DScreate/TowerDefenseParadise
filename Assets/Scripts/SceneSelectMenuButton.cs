using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSelectMenuButton : MonoBehaviour {

	public void LoadSceneSelect() {
        //GameManager.StartLevel();

        SceneManager.LoadScene("TestDiag");
	}
}
