using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/*
 * Credit to http://www.indiana.edu/~gamedev/2015/09/27/creating-a-visual-novel-in-unity/
 * for their great intro tutorial and our jumping off point
 */

public class DialogueParser : MonoBehaviour {

	struct DialogueLine { // The data of each line of dialogue
		public string name;
		public string content;
		public int pose;
		public string position;
		public string[] options;

		public DialogueLine(string Name, string Content, int Pose, string Position) {
			name = Name;
			content = Content;
			pose = Pose;
			position = Position;
			options = new string[0];
		}
	}

	List<DialogueLine> lines; // Storing the lines in a specific order

	// Use this for initialization
	void Start () {
		string file = "Assets/Data/Dialogue/" + SceneManager.GetActiveScene().name + ".txt";
		lines = new List<DialogueLine>();

		LoadDialogue (file);
	}

	void LoadDialogue(string filename) {
		string line;
		StreamReader r = new StreamReader (filename);

		using (r) {
			do {
				line = r.ReadLine();
				if (line != null) {
					string[] lineData = line.Split(';');
					if (lineData[0] == "Player") {
						DialogueLine lineEntry = new DialogueLine(lineData[0], "", 0, "");
						lineEntry.options = new string[lineData.Length-1];
						for (int i = 1; i < lineData.Length; i++) {
							Debug.Log(lineData[i]);
							lineEntry.options[i-1] = lineData[i];
						}
						lines.Add(lineEntry);
					} else {
						DialogueLine lineEntry = new DialogueLine(lineData[0], lineData[1], int.Parse(lineData[2]), lineData[3]);
						lines.Add(lineEntry);
					}
				}
			}
			while (line != null);
			r.Close();
		}
	}

	public string GetPosition(int lineNumber) {
		if (lineNumber < lines.Count) {
			return lines[lineNumber].position;
		}
		return "";
	}

	public string GetName(int lineNumber) {
		if (lineNumber < lines.Count) {
			return lines[lineNumber].name;
		}
		return "";
	}

	public string GetContent(int lineNumber) {
		if (lineNumber < lines.Count) {
			return lines[lineNumber].content;
		}
		return "";
	}

	public int GetPose(int lineNumber) {
		if (lineNumber < lines.Count) {
			return lines[lineNumber].pose;
		}
		return 0;
	}

	public string[] GetOptions(int lineNumber) {
		if (lineNumber < lines.Count) {
			return lines[lineNumber].options;
		}
		return new string[0];
	}
}
