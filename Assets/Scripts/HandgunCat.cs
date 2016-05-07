using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class HandgunCat : MonoBehaviour {

	Rigidbody2D body;
	BoxCollider2D coll;

	public int currHealth;
	public int maxHealth;

	public bool DialogueEnabled;

	public Dialogue dialog;

	public GameObject dialogBox;

	public TextAsset script1;
	public TextAsset script2;
	public TextAsset script3;


	public List <string> lines;
	public List<List<string>>  lines2;


	GameObject char_ref; //reference to character object

	Vector3 distance;
	public GameObject bullet_ref; //reference to bullet prefab

	float time; //time since last bullet was fired
	public float shooting_rate; //rate at which bullets will be fired

	[SerializeField] private bool dead;

	int waveCounter;

	public int diacounter;

	// Use this for initialization

	void Awake(){
		this.currHealth = 30;
		this.maxHealth = currHealth;
		this.DialogueEnabled = false;
		//this.dialog.enabled = false;

		 diacounter = 0;

		lines.Add(script1.text);  
		lines.Add(script2.text);
		lines.Add(script3.text);
		waveCounter = 0;

	}

	//THIS WORKS.
	void initLines( List<List<string>> lines2add){
		var info = new DirectoryInfo ("Assets/Resources/TextAssets");
		var folders = info.GetDirectories ();
		int i = 0; 
		foreach (DirectoryInfo folder in folders) {//now going to iterate through each folder
			//List <string> dialogbranch = new List<string>(
			//we need to add to each list within the list of lists if it's index is the same
			//print(files.Name); //files.Name returns the name of the .txt file
			if (folder.Name == "hgcdialog" + i) { //if folder is in the right
				//We need to enter the folder

				//print (folder.Name); //returning proper folder name
				var files = folder.GetFiles ("*.txt"); //returns the list of files in that folder
				int j = 0;

				List<string> templist = new List<string> ();

				foreach (FileInfo file in files) {
					//print (file.Name); // returning proper file name
					string contents = File.ReadAllText (file.FullName); 
					//print (contents); //printing the proper contents

					//lines2add [i].Insert (j, contents);
					//print ("cuck me");
					templist.Insert (j, contents);
					//print (templist [j]);
					j++;
				
				}
				lines2add.Add(templist);
			}
			i++;
		}
		for (int q = 0; q < lines2add.Count; q++) {
			for (int r = 0; r < lines2add [q].Count; r++) {
				print (lines2add [q] [r]);
			}
		}
	}




		
//		FileInfo[] info = dir.GetFiles("*.txt");
//		print ("Hahahaha");
//
//
//	foreach (FileInfo f in info) 
//	{ 
//			print (f.FullName);
////			int counter = 0; //initialize counter to add to list properly
////			int countcounter = 0;
////			if (f.FullName == "HandgunCatDia" + counter + countcounter) {
////				TextAsset dia2add = (TextAsset) f;
////				lines[counter] = f.text
////				
//		}
//
//
//
//		
//	}

	void Start () {

//		for (int i = 0; i < lines.Count; i++) {
//			print (lines [i]);
//		}
		lines2 = new List<List<string>>();
		initLines(lines2);
		

		//dialogBox = GameObject.Find ("DialogBox").gameObject ;
	
		body = GetComponent<Rigidbody2D>();
		body.freezeRotation = true;
		dead = false;
		coll = GetComponent<BoxCollider2D>();

		char_ref = GameObject.FindGameObjectWithTag ("Player");
		char_ref.GetComponent<Character> ().bossfightEnabled = true;

		this.name = "HandgunCat";
		this.tag = "boss";
	}


	
	// Update is called once per frame
	void Update () {
		if (DialogueEnabled == true) {
			//do nothing
		} else {
//			if (currHealth > maxHealth / 2) {
//				int randomWave = Random.Range (0, 3);
//				warmWave (randomWave);
//			}
			//fastBull();
			handleShooting ();
		}
	
	}

	//selects a random animation and then begins coroutine
//	void warmWave(int waveNum){
//		if (waveNum == 0) {
//			StartCoroutine(fastBull)
//		}
//	}

	void fastBull(){
		time += Time.deltaTime;
		float distance = Vector2.Distance (transform.position, char_ref.transform.position);

		//GameObject bullet = Instantiate(bullet_ref, this.transform.position, char_ref.transform.rotation) as GameObject;
	}


	void handleShooting(){
		time += Time.deltaTime;
		float distance = Vector2.Distance(transform.position, char_ref.transform.position);

		if (time > shooting_rate && dead == false && distance < 6)
		{
			GameObject bullet = Instantiate(bullet_ref, this.transform.position, char_ref.transform.rotation) as GameObject;
			//bullet.transform.rotation = new Quaternion( Vector3.Angle (this.transform.position, char_ref.transform.position));
			//bullet.transform.forward = transform.forward;
			time = 0.0f;
		}

	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Ammo") {
			this.currHealth--;
			Destroy (coll.gameObject);
		}
	}





}

