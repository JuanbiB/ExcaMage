using UnityEngine;
using System.Collections;

public class FlyingEnemy : MonoBehaviour {

	GameObject player;
	Rigidbody2D rb;
	Vector3 temp;
	int speed;

	public bool moving_towards;
	public bool appliedForce;

	bool dead;

	float time;
	float time2;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		speed = 3;

		temp = transform.position;
		moving_towards = true;
		time = 0.0f;
		time2 = 0.0f;

		name = "FlyingMonster";

		appliedForce = false;
		dead = false;
	}
	
	// Update is called once per frame
	void Update () {


		if (dead)
			return;

		// From here
		if (appliedForce) {
			time2 += Time.deltaTime;
			if (time2 > 1.5f) {
				appliedForce = false;
				rb.velocity = Vector3.zero;
				time2 = 0.0f;
			}
			return;
		}
		// To here - it's to handle when the player uses his power on our creature

		chasingPlayer ();

		//From here
		if (!moving_towards)
			time += Time.deltaTime;

		if (time > .8f) {
			time = 0.0f;
			moving_towards = true;
		}
		// To here - we handle when you collide with the player and bounce off.
	}

	void chasingPlayer(){
		Vector3 direction = player.transform.position - transform.position;
		direction.Normalize ();

		temp = transform.position + direction * Time.deltaTime * speed;

		if (moving_towards == true) {
			transform.position = temp;
		}
	}

	void bounceOffPlayer(){
		Vector3 dir = player.transform.position - transform.position;
		dir.Normalize ();
		rb.AddForce (-dir * 250);
		moving_towards = false;
	}

	public IEnumerator spiked()	
	{
		float time = 0.0f;
		Quaternion qua = Quaternion.Euler(new Vector3(0, 0, -90));
		gameObject.GetComponent<SpriteRenderer>().color = Color.red;
		gameObject.GetComponent<BoxCollider2D> ().isTrigger = true;
		dead = true;
		while (time < 1.5)
		{
			time += Time.deltaTime;
			// This just turns the enemy 90 degrees. I guess the direciton has to do with where you the spike from, but that's TODO.
			transform.rotation = Quaternion.Slerp(transform.rotation, qua, Time.deltaTime * 5);
			yield return null;
		}

		Destroy (this.gameObject);
		BoardCreator.instance.SendMessage("kill");
	}
		

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Player") {
			bounceOffPlayer ();
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Spike") {
			StartCoroutine (spiked ());
		}
	}
}
