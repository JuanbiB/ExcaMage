using UnityEngine;
using System.Collections;
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


	public List<string>  lines;


	GameObject char_ref; //reference to character object

	Vector3 distance;
	public GameObject bullet_ref; //reference to bullet prefab

	float time; //time since last bullet was fired
	public float shooting_rate; //rate at which bullets will be fired

	[SerializeField] private bool dead;

	// Use this for initialization

	void Awake(){
		this.currHealth = 30;
		this.maxHealth = currHealth;
		this.DialogueEnabled = false;
		//this.dialog.enabled = false;
		lines.Add(script1.text);  
		lines.Add(script2.text);
		lines.Add(script3.text);
	}



	void Start () {

		for (int i = 0; i < lines.Count; i++) {
			print (lines [i]);
		}
		

		dialogBox = GameObject.Find ("DialogBox").gameObject ;
	
		body = GetComponent<Rigidbody2D>();
		body.freezeRotation = true;
		dead = false;
		coll = GetComponent<BoxCollider2D>();

		char_ref = GameObject.FindGameObjectWithTag ("Player");
		char_ref.GetComponent<Character> ().bossfightEnabled = true;

		this.name = "HandgunCat";
		this.tag = "boss";
		Destroy (dialogBox.GetComponent<Dialogue> ());


	}

//	public void Script(){
//		Destroy (dialogBox.GetComponent<Dialogue> ());
//		dialog = dialogBox.gameObject.AddComponent<Dialogue> ();
//
//	}


//	}
	
	// Update is called once per frame
	void Update () {
		if (DialogueEnabled == true) {
			//do nothing
		} else {

			handleShooting ();
		}
	
	}

	//void 

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

