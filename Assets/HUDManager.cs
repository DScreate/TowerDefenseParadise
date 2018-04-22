using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    public Text senpaiPointsText;


	void Start ()
    {
		
	}
	
	void Update ()
    {
        senpaiPointsText.text = "$" + GameManager.money;
	}
}
