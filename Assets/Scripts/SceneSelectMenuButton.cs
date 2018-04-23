using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectMenuButton : MonoBehaviour {

	public void LoadSceneSelect() {
        //GameManager.StartLevel();

        SceneManager.LoadScene("TestDiag");
	}

	public void Quit()
	{
		Application.Quit();
	}
}
