﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour {

	public Text emtitle;
	[SerializeField] private Button startbutton = null;
    [SerializeField] private Button practiceButton = null;
	public AudioSource startsound;

	// Use this for initialization
	void Start () {
		emtitle.GetComponent<Image> ();
		startbutton.GetComponent<Button> ();
        practiceButton.GetComponent<Button>();
        startbutton.onClick.AddListener (() => gameStart ());
		startsound.GetComponent<AudioSource> ();
	
	}

	void gameStart(){
		emtitle.gameObject.SetActive (false);
		startbutton.gameObject.SetActive (false);
        practiceButton.gameObject.SetActive(false);

		startsound.Play ();
		StartCoroutine (LoadGame ());
	}

	IEnumerator LoadGame(){

		AsyncOperation async = SceneManager.LoadSceneAsync ("main");

		while (!async.isDone) {
    		print (async.progress);
            emtitle.GetComponent<Text>().text = "Loading...";
            emtitle.gameObject.SetActive(true);
            yield return(0);
		}
		//yield return new WaitForSeconds (4);
		//SceneManager.LoadScene ("main");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
