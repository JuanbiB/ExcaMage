using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour {

	public Text emtitle;
	[SerializeField] private Button startbutton = null;
	public AudioSource startsound;

	// Use this for initialization
	void Start () {
		emtitle.GetComponent<Image> ();
		startbutton.GetComponent<Button> ();
		startbutton.onClick.AddListener (() => gameStart ());
		startsound.GetComponent<AudioSource> ();
	
	}

	void gameStart(){
		emtitle.gameObject.SetActive (false);
		startbutton.gameObject.SetActive (false);
		startsound.Play ();
		StartCoroutine (LoadGame ());
	}
	IEnumerator LoadGame(){
		yield return new WaitForSeconds (4);
		SceneManager.LoadScene ("main");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
