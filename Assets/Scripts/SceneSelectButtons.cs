using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class SceneSelectButtons : MonoBehaviour {

	public void LoadScene() {
		// look at button name
		string buttonName = name;
		// get numbers from button name
		string buttonNumber = Regex.Replace(buttonName, "[^0-9]", "");
		int levelNumber = int.Parse(buttonNumber);

		Debug.Log (levelNumber);
		// load scene + numbers
		SceneManager.LoadScene("Scene" + levelNumber);
	}
}
