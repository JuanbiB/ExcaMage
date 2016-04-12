using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pause : MonoBehaviour {
	bool ispaused;

	// Use this for initialization
	void Start () {
		GetComponent<Text> ();
		ispaused = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp("escape") && ispaused==false){
			Time.timeScale=0;
			ispaused = true;
			print ("YEAH");
		}
		if (Input.GetKeyUp ("escape") && ispaused == true) {
			Time.timeScale = 1;
			ispaused = false;

		}

	
	}
}
