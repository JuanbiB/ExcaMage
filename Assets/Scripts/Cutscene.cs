using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour {

	// Use this for initialization
	//Need references to all UI Objects in Hierarchy and turn them off.
	//Set their Tags to UI, put them in the list, remove them one by one.

	public Camera mc_ref;
	bool initDialog;
	bool prefight;
	bool fighting;
	bool postfight;

	//Dialogue dialog; maybe get dialog box instead, we can use GetComponent
	TextAsset script1;
	TextAsset script2;
	TextAsset script3;


	GameObject char_ref;
	GameObject boss_ref;

	GameObject [] UIlist;

	public GameObject dialogbox;
	public GameObject Directions;

	bool showdialogbox;
	bool dialogadded;

	bool laughanim = false;
	bool flipanim = false;

	void Start () {

		//fighting = true;
		showdialogbox = false;
		dialogadded = false;

		char_ref = GameObject.FindGameObjectWithTag ("Player");
		boss_ref = GameObject.Find ("HandgunCat");
		UIlist = GameObject.FindGameObjectsWithTag ("UI");

		//works
	
	}
	void hideUIelements(){
		foreach (GameObject go in UIlist) {
			if (go.gameObject.name == "HeartPanel" || go.gameObject.name == "Push Cooldown" || go.gameObject.name == "Pull Cooldown" || go.gameObject.name=="Empty Health Bar" 
				|| go.gameObject.name =="Text (1)" || go.gameObject.name=="Text" || go.gameObject.name=="tip") {
			//Do nothing
			}
			else{
				go.gameObject.SetActive(false);
			}
		}
		mc_ref.gameObject.AddComponent<hgcCC> ();
		mc_ref.GetComponent<hgcCC> ().enabled = true;
	}


	//Hides all UI and starts 'Prefight' scenario
	void CutsceneStart(){
		char_ref = GameObject.FindGameObjectWithTag ("Player");
		boss_ref = GameObject.Find ("HandgunCat");

		/**This is a workaround because I didn't know else how to hide UIlist on start and hide at end, kinda buggy but it works**/
		char_ref.GetComponent<Character> ().DialogueEnabled = true;
		boss_ref.GetComponent<HandgunCat> ().DialogueEnabled = true;

		foreach (GameObject go in UIlist) {
			go.gameObject.SetActive (false);
			if (go.gameObject.name == "DialogBox") {
				dialogbox = go;
			}
			if (go.gameObject.name == "Directions") {
				Directions = go;
			}
		}
		prefight = true;
		initDialog = false;
	}

	void CutsceneEnd(){
		foreach (GameObject go in UIlist) {
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
		yield return new WaitForSeconds (3.6f);
		showdialogbox = true;
	}

	IEnumerator hand2player(){
		Directions.SetActive (true);
		yield return new WaitForSeconds (8.0f);
		char_ref.GetComponent<Character> ().DialogueEnabled = false;
		boss_ref.GetComponent<HandgunCat> ().DialogueEnabled = false;
		Directions.SetActive (false);

	}

	void runDia(){
		dialogbox.SetActive (true); //activates dialog box
		dialogbox.AddComponent<Dialogue> ();//say the dialog
		initDialog = true;
	}


	// Update is called once per frame
	void Update () {
	//We want this to only happen once (I'm not exactly sure how we do this)
		if (SceneManager.GetActiveScene().name=="boss" && initDialog==false && fighting!=true) {
		CutsceneStart ();
		}
		//before fight happens
		if (prefight) {
			//If dialog has not been initialized, we need to initialize it
			if (initDialog == false) {
				mc_ref.GetComponent<CameraController> ().enabled = false;
				mc_ref.GetComponent<BossCC> ().findBoss ();
				StartCoroutine (wait4lerp2end ());
				if (showdialogbox == true) { //when we show the dialog box
					runDia();
					
				}
			//Check if dialog is done, if so, then get rid of dialog and hand controls back to player
			} else {
				if (dialogbox.GetComponent<Dialogue> ().dialogdone == true) {
					dialogbox.SetActive (false); //turn off dialogbox
					if (flipanim == false && laughanim == false) { //if he hasn't laughed yet do so
						Destroy (dialogbox.gameObject.GetComponent<Dialogue>());
						Destroy (dialogbox.gameObject.GetComponent<Dialogue>());
						boss_ref.GetComponent<HandgunCat>().diacounter++; 
						runDia();
						flipanim = true;
					}
					else if (flipanim == true && laughanim == false) {
						boss_ref.GetComponent<HandgunCat>().diacounter++;//increment diacounter so it hits dialogue again
						Destroy (dialogbox.gameObject.GetComponent<Dialogue>());
						runDia();
						laughanim = true;
					} else {
						mc_ref.gameObject.AddComponent<hgcCC> ();
						mc_ref.GetComponent<hgcCC> ().enabled = true;
						StartCoroutine (hand2player ()); //Waitforseconds + return original camera script to player
						CutsceneEnd (); //
						prefight = false;
						initDialog = false;
						fighting = true;
				}
			}
			if (fighting == true) {
				GameObject.Find ("SoundManager").GetComponent<Sound3> ().balls = true;
			}

			}
		}
	}
}
	




			



