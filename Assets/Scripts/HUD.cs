﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour {

	public List <Image> heartSprites;

	public Image HeartUI;

	public Text deathText;

	public Button retry;

	int numhearts;

	GameObject player;

	//public HorizontalLayoutGroup hggroup;
	public RectTransform ParentPanel;


	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player");
		numhearts = 0;
		addHearts ();

		deathText.gameObject.SetActive (false);
		//HeartUI = (Image) GameObject.FindGameObjectWithTag("Heart");

		//chr = player.GetComponent<Character> ();
	}

	//To be used at instantiation of game
	void addHearts(){
			for (int i = 0; i < player.GetComponent<Character>().health; i++) {
			Image heart_to_add = Instantiate (HeartUI);//instantiate heart
			heart_to_add.gameObject.SetActive(true);
			heart_to_add.gameObject.name = "Heart" + numhearts;
			heartSprites.Add (heart_to_add);
			heart_to_add.transform.SetParent(ParentPanel,false);
			heart_to_add.transform.localScale = new Vector3 (1, 1, 1);
			numhearts++;

		}
		
	}

    public void add_life()
    {
        for (int i = numhearts; i < player.GetComponent<Character>().health; i++)
        {
            Image heart_to_add = Instantiate(HeartUI);//instantiate heart
            heart_to_add.gameObject.SetActive(true);
            heart_to_add.gameObject.name = "Heart" + numhearts;
            heartSprites.Add(heart_to_add);
            heart_to_add.transform.SetParent(ParentPanel, false);
            heart_to_add.transform.localScale = new Vector3(1, 1, 1);
            numhearts++;
        }
    }
	
	// Update is called once per frame
	void Update () {

        add_life();

		if (player == null) {
			//print ("CUCK");
			player = GameObject.FindGameObjectWithTag ("Player");
			addHearts ();
			print (heartSprites.Count);
		}

        if (player.GetComponent<Character>().health < numhearts) {
			Image heart_to_destroy = heartSprites [heartSprites.Count - 1];
			Destroy (heart_to_destroy.gameObject);
			heartSprites.RemoveAt (heartSprites.Count - 1);
			numhearts--;
			//heart_to_destroy.SetActive (false);
			
		}
		if (player.GetComponent<Character>().health <= 0) {
			Time.timeScale = 0;
			deathText.gameObject.SetActive (true);
            player.GetComponent<SpriteRenderer>().enabled = false;
            GameObject.Find("Canvas/Death").GetComponent<Image>().enabled = true;
            if (Input.anyKey) {
               // int scene = SceneManager.GetActiveScene().buildIndex;
                //SceneManager.LoadScene(scene, LoadSceneMode.Single);
            }
			retry.gameObject.SetActive (true);
		}

	}

    public void getRekt()
    {
        //Time.timeScale = 0;
        deathText.gameObject.SetActive(true);
        deathText.text = "You cannot kill a God.";
        if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.C)) 
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}
