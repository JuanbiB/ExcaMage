using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;


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

	public GameObject big_bullet_ref; //reference to big bullet prefab

	float time; //time since last bullet was fired
	float time2;
	public float shooting_rate; //rate at which bullets will be fired

	[SerializeField] private bool dead;

	int waveCounter;
   
	public int diacounter;

    bool switch_dash;
    bool stay_still;

    Animator my_animator;

    // Use this for initialization

    void Awake(){
		this.currHealth = 30;
		this.maxHealth = currHealth;
		this.DialogueEnabled = false;
		 diacounter = 0;

		lines.Add(script1.text);  
		lines.Add(script2.text);
		lines.Add(script3.text);
		waveCounter = 0;

	}

	void initLines( List<List<string>> lines2add){
		var info = new DirectoryInfo ("Assets/Resources/TextAssets");
		var folders = info.GetDirectories ();
		int i = 0; 
		foreach (DirectoryInfo folder in folders) {//now going to iterate through each folder
			//we need to add to each list within the list of lists if it's index is the same
			if (folder.Name == "hgcdialog" + i) { //if folder is in the right
				//We need to enter the folder
				var files = folder.GetFiles ("*.txt"); //returns the list of files in that folder
				int j = 0;

				List<string> templist = new List<string> ();

				foreach (FileInfo file in files) {
					string contents = File.ReadAllText (file.FullName); 
					templist.Insert (j, contents);
					j++;
				
				}
				lines2add.Add(templist);
			}
			i++;
		}
		for (int q = 0; q < lines2add.Count; q++) {
			for (int r = 0; r < lines2add [q].Count; r++) {
			}
		}
	}
		
	void Start () {
		lines2 = new List<List<string>>();
		initLines(lines2);
		
		time2 = 0.0f;
	
		body = GetComponent<Rigidbody2D>();
		body.freezeRotation = true;
		dead = false;
		coll = GetComponent<BoxCollider2D>();

		char_ref = GameObject.FindGameObjectWithTag ("Player");
		char_ref.GetComponent<Character> ().bossfightEnabled = true;

		this.name = "HandgunCat";
		this.tag = "boss";
        switch_dash = true;
        stay_still = true;

        my_animator = GetComponent<Animator>();
	}


	// Update is called once per frame
	void Update () {
        if (DialogueEnabled == true) {
            //do nothing
        } else {
			if (transform.position.x == 15 && waveCounter > 5) {
				waveCounter = 0;
			} else {
				waveCounter++;
				dashState ();
			}
		}

		if (currHealth <= 0) {
			SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
		}
	}
	void dashState(){
		time2 += Time.deltaTime;

		if (time2 > 1) {

            if (!stay_still)
            {
                my_animator.Play("idle");
                time2 = 0.0f;
 
                int random = Random.Range(180, 250);

                if (switch_dash == true)
                {
                    my_animator.Play("handguncat_left_dash");
                    body.AddForce(new Vector2(-1, 0) * random);
                    switch_dash = !switch_dash;
                    stay_still = true;
                }

                else
                {
                    my_animator.Play("handguncat_right_dash");
                    body.AddForce(new Vector2(1, 0) * random);
                    switch_dash = !switch_dash;
                    stay_still = true;
                }
            }
            else
            {
                my_animator.Play("cat_shoot");
                body.velocity = Vector2.zero;
				randWave ();
                if (time2 > 4.5f)
                    stay_still = false;
            }
			
		}
	}

	void randWave(){
		int randint = Random.Range (0, 2);
		if (randint == 0) {
			fastBull ();
		}
		if (randint == 1) {
			handleShooting ();
		}
	}

	void fastBull(){
		time += Time.deltaTime;
		float distance = Vector2.Distance (transform.position, char_ref.transform.position);

		if (time > shooting_rate && dead == false) {
			GameObject big_bullet = Instantiate (big_bullet_ref, this.transform.position - new Vector3(0,1,0), char_ref.transform.rotation) as GameObject;
			time = 0.0f;
		}
	}


	void handleShooting(){
		time += Time.deltaTime;
		float distance = Vector2.Distance(transform.position, char_ref.transform.position);

		if (time > shooting_rate && dead == false)
		{
			GameObject bullet = Instantiate(bullet_ref, this.transform.position - new Vector3(0, 1, 0), char_ref.transform.rotation) as GameObject;
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

