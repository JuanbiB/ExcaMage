using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

	private string assetText;


	public GameObject char_ref;
	public GameObject bossref;

	public bool talking; //a boolean testing whether dialogue is to occur

	public bool dialogdone;

	AudioSource source;
	AudioClip speech1;
	AudioClip speech2;
	AudioClip speech3;

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
	// Use this for initialization
	void Start () {
		
		hasSpoken = false;
		counter = 0;
		dialogdone = false;
		inSay = false;
		char_ref = GameObject.FindGameObjectWithTag ("Player");
		bossref = GameObject.Find("HandgunCat"); //from handgunCat
		dialogUI = GameObject.Find("Dialog").gameObject.GetComponent<Text>();

		speech1 = Resources.Load ("Sound/speech 1") as AudioClip;
		speech2 = Resources.Load ("Sound/speech 2") as AudioClip;
		speech3 = Resources.Load ("Sound/speech 3") as AudioClip;

		source = gameObject.AddComponent<AudioSource> ();
    }

	public bool Say (string dialogue) {
		if (talking)
			return false;
		StartCoroutine(Saydialogue(dialogue));
		return true;
	}

	public IEnumerator Saydialogue (string dialogue) {

		int rand = Random.Range (1, 3);

		if (rand == 1) {
			source.Stop ();
			source.PlayOneShot (speech1, .5f);
		} else if (rand == 2) {
			source.Stop ();
			source.PlayOneShot (speech2, .5f);
		} else {
			source.Stop ();
			source.PlayOneShot (speech3, .5f);
		}

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
		if (counter > bossref.GetComponent<HandgunCat> ().lines2 [bossref.GetComponent<HandgunCat> ().diacounter].Count - 1) {
			dialogdone = true;
			yield break;
		} else {

			string nextDia = bossref.GetComponent<HandgunCat> ().lines2 [bossref.GetComponent<HandgunCat> ().diacounter] [counter];
			yield return StartCoroutine (Saydialogue (nextDia));
		}
	}
	// Update is called once per frame
	void Update () {
		if (assetText == null && hasSpoken==false){
			if (bossref == null) {
				bossref = GameObject.Find("HandgunCat");
			}
			assetText = bossref.GetComponent<HandgunCat> ().lines2 [bossref.GetComponent<HandgunCat>().diacounter][0];
			Say (assetText);
			hasSpoken = true;
		}
	}
}	




