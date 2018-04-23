using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour {

	public string option;
	public DialogueManager manager;

	public void SetText(string newText) {
		GetComponentInChildren<Text> ().text = newText;
	}

	public void SetOption(string newOption) {
		option = newOption;
	}
	
	// Could this be made into an inspector based control instead of hard coded?
	public void ParseOption() {
		string command = option.Split (',') [0];
		string commandModifier = option.Split (',') [1];
		manager.playerTalking = false;
		switch (command)
		{
			case "line":
				manager.lineNum = int.Parse(commandModifier);
				manager.ShowDialogue();
				break;
			case "scene":
                if(commandModifier == "game")
                    GameManager.StartLevel();
                SceneManager.LoadScene(commandModifier);
				break;
			case "Yes" :
				// do something with Yes
				break;
			case "No" :
				// do something with No
				break;
		}
	}
}
