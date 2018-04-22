using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSelectMenuButton : MonoBehaviour {

	public void LoadSceneSelect() {
		SceneManager.LoadScene(1);
	}
}
