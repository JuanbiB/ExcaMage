using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

	public TextAsset asset; //to get text from 
	private string assetText;

	public GameObject char_ref;
	public GameObject bossref;

	public bool talking; //a boolean testing whether dialogue is to occur
	[SerializeField] private Text dialogUI; //reference to Text UI object where dialogue will be occuring

	string dialog; //String where we will add to dialogUI. Holds all of the actual dialogue.

	//text is wrapping properly thanks to Unity UI (ty Unity). What we need now
	//TODO
	//1. Dialogue printing character by character.
	//2. After certain character length, requires user input, deletes previous text, and starts over from the top (Also if dialog end) 
	//(TBH) Should probably make method dialogueWipe()
	//3. Noises after every character is printed



	// Use this for initialization
	void Start () {
		assetText = asset.text;
		char_ref = GameObject.FindGameObjectWithTag ("Player");
		bossref = GameObject.FindGameObjectWithTag ("HandgunCat");
	
	}
	public bool Say (string dialogue) {
		if (talking)
			return false;
		StartCoroutine(Saydialogue(dialogue));
		char_ref.GetComponent<Character> ().DialogueEnabled = true;
		bossref.GetComponent<HandgunCat> ().DialogueEnabled = true;
		return true;
	}
	IEnumerator Saydialogue (string dialogue) {
		for (int i = 0; i <= dialogue.Length; i ++) {
			print ("yech");
			dialogUI.text = dialogue.Substring(0, i);

			yield return new WaitForSeconds (0.05f);
		}
		yield return new WaitForEndOfFrame();
	
//
//		while (!Input.GetMouseButton(0))
//			
////
		talking = false;
}
//
//	}

	
	// Update is called once per frame
	void Update () {
		if (talking == true) {
			
		}
		print (assetText);
		Say (assetText);
		
	
	}
}
