using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

	private string assetText;


	public GameObject char_ref;
	public GameObject bossref;

	public bool talking; //a boolean testing whether dialogue is to occur

	public bool dialogdone;


	private bool inSay;

	bool hasSpoken;

	int counter;

	//[SerializeField] private Image DialogBox; // reference to DialogBox object.
	public Text dialogUI; //reference to Text UI object where dialogue will be occuring


	//text is wrapping properly thanks to Unity UI (ty Unity). What we need now
	//TODO
	//1. Dialogue printing character by character.
	//2. After certain character length, requires user input, deletes previous text, and starts over from the top (Also if dialog end) 
	//(TBH) Should probably make method dialogueWipe()
	//3. Noises after every character is printed

	void Awake(){
		

	}



	// Use this for initialization
	void Start () {
		
		hasSpoken = false;
		counter = 0;
		dialogdone = false;
		inSay = false;
		char_ref = GameObject.FindGameObjectWithTag ("Player");
		bossref = GameObject.Find("HandgunCat"); //from handgunCat
		dialogUI = GameObject.Find("Dialog").gameObject.GetComponent<Text>();


//		assetText = bossref.GetComponent<HandgunCat> ().lines [0];
//
//
//
//	
//
//
//		if (assetText != null) {
//		print("assetText is not Null");
//			Say (assetText);
//		}


    }
	public bool Say (string dialogue) {
		//print (assetText);
		if (talking)
			return false;
		StartCoroutine(Saydialogue(dialogue));
		return true;
	}

	public IEnumerator Saydialogue (string dialogue) {


	for (int i = 0; i <= dialogue.Length; i++) {

			inSay = true;



			dialogUI.text= dialogue.Substring (0, i);
			yield return new WaitForSeconds (0.001f);
		}
			
		inSay = false;

		while (!Input.anyKey) {
			yield return null;

		}
		counter++;
		dialogUI.text = "";

		//If theres no more dialog, break

		print (counter);
		print (bossref.GetComponent<HandgunCat> ().lines2 [bossref.GetComponent<HandgunCat> ().diacounter].Count - 1);
		if (counter > bossref.GetComponent<HandgunCat> ().lines2 [bossref.GetComponent<HandgunCat> ().diacounter].Count - 1) {
			dialogdone = true;
			print ("reaching dialogdone");
			yield break;
		} else {

			string nextDia = bossref.GetComponent<HandgunCat> ().lines2 [bossref.GetComponent<HandgunCat> ().diacounter] [counter];
			print (nextDia);
			print ("nextdia should happen now");
			yield return StartCoroutine (Saydialogue (nextDia));
		}
		}
				


		//yield return new WaitForEndOfFrame();
	
//
//		while (!Input.GetMouseButton(0))
//			
////
//		talking = false;


//public IEnumerator WipeDialogue(){
//	while (inSay) {
//		yield return new WaitForSeconds(0.01f);
//	}
//		bool isPrompt = Input.anyKeyDown;
//		yield return new WaitWhile (() => isPrompt == false);
//
//		dialogUI.text = "";
//	
//}
//	}

//
//	}

	
	// Update is called once per frame
	void Update () {
		if (hasSpoken == true) {
			//break;
		
		}


		if (assetText == null && hasSpoken==false){
			if (bossref == null) {
				bossref = GameObject.Find("HandgunCat");
			}
			//assetText = bossref.GetComponent<HandgunCat> ().lines [counter];
			assetText = bossref.GetComponent<HandgunCat> ().lines2 [bossref.GetComponent<HandgunCat>().diacounter][0];
			Say (assetText);
			hasSpoken = true;
		}
		
		}
			

		}

			




