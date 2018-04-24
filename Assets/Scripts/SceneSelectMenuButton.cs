using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectMenuButton : MonoBehaviour {

	public void LoadSceneSelect() {
        //GameManager.StartLevel();

        SceneManager.LoadScene("Chap1_Start");
	}

	public void Quit()
	{
		Application.Quit();
	}
}
