using UnityEngine;
using System.Collections;

public class HandgunCat : MonoBehaviour {

	Rigidbody2D body;
	BoxCollider2D coll;

	public int currHealth;
	public int maxHealth;

	public bool DialogueEnabled;


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
	}



	void Start () {
	
		body = GetComponent<Rigidbody2D>();
		body.freezeRotation = true;
		dead = false;
		coll = GetComponent<BoxCollider2D>();

		char_ref = GameObject.FindGameObjectWithTag ("Player");
		char_ref.GetComponent<Character> ().bossfightEnabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (DialogueEnabled == true) {
			//do nothing
		} else {

			handleShooting ();
		}
	
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

