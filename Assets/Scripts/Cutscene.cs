using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour {

	// Use this for initialization
	//Need references to all UI Objects in Hierarchy and turn them off.
	//Set their Tags to UI, put them in the list, remove them one by one.

	public Camera mc_ref;



	bool initDialog;

	bool prefight;
	bool fight;
	bool postfight;

	//Dialogue dialog; maybe get dialog box instead, we can use GetComponent

	TextAsset script1;
	TextAsset script2;
	TextAsset script3;


	GameObject char_ref;
	GameObject boss_ref;

	GameObject [] UIlist;

	public GameObject dialogbox;

	bool showdialogbox;
	bool dialogadded;

	void Start () {
		showdialogbox = false;
		dialogadded = false;


		//works
	
	}

	void CutsceneStart(){
		
		char_ref = GameObject.FindGameObjectWithTag ("Player");
		boss_ref = GameObject.Find ("HandgunCat");
		UIlist = GameObject.FindGameObjectsWithTag ("UI");

		char_ref.GetComponent<Character> ().DialogueEnabled = true;
		boss_ref.GetComponent<HandgunCat> ().DialogueEnabled = true;

		foreach (GameObject go in UIlist) {
			go.gameObject.SetActive (false);
			if (go.gameObject.name == "DialogBox") {
				dialogbox = go;
			}
		}
		prefight = true;
		initDialog = false;
	}


	void CutsceneEnd(){
		foreach (GameObject go in UIlist) {
			print ("we are hitting cutscene end");
			go.gameObject.SetActive (true);
			if (go.gameObject.name == "DialogBox") {
				go.gameObject.SetActive (false);
			}
			if (go.gameObject.name == "Empty Health Bar") {
				go.gameObject.SetActive (true);
				Destroy (go.gameObject.GetComponent<BarScript> ());
				go.gameObject.AddComponent<BarScript> ();

			}
		}
	}
	IEnumerator wait4lerp2end(){
		//print("fuck u unity");
		yield return new WaitForSeconds (3.6f);
		showdialogbox = true;
	}

	IEnumerator hand2player(){
		yield return new WaitForSeconds (3.6f);
		mc_ref.GetComponent<BossCC> ().enabled = false;
		mc_ref.GetComponent<CameraController> ().enabled = true;
		char_ref.GetComponent<Character> ().DialogueEnabled = false;
		boss_ref.GetComponent<HandgunCat> ().DialogueEnabled = false;

	}


	// Update is called once per frame
	void Update () {
	if (Input.GetKeyUp (KeyCode.P)) {
		CutsceneStart ();
	}


		if (prefight) {
			
			if (initDialog == false) {
				//char_ref.GetComponent<Character>().Cutscenemovement(); // this would have to be scripted as to when the character reaches boss room, he moves to middle of room
				//camera moves towards boss
				print ("yeah");
				mc_ref.GetComponent<CameraController> ().enabled = false;
				mc_ref.GetComponent<BossCC> ().findBoss ();
				StartCoroutine (wait4lerp2end ());
				if (showdialogbox == true) { //when we show the dialog box
					dialogbox.SetActive (true); //activates dialog box
					dialogbox.AddComponent<Dialogue> ();//say the dialog
					initDialog = true;
				}
			} else {
				if (dialogbox.GetComponent<Dialogue> ().dialogdone == true) {
					dialogbox.SetActive (false);
					mc_ref.GetComponent<BossCC> ().setTarget (char_ref);
					StartCoroutine (hand2player ());
					CutsceneEnd ();
					prefight = false;
				}
			}
			}
		
		}
}
	




			



