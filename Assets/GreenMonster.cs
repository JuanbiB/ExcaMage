using UnityEngine;
using System.Collections;

public class GreenMonster : MonoBehaviour {

	GameObject player;
	Rigidbody2D rb;
	Vector3 temp;
	public int speed;

	public bool moving_towards;
	public bool appliedForce;

	bool dead;

	float time;
	float time2;
	float time3;
	int min_distance;

	// Exploding death
	public GameObject exploding_pieces;

	public GameObject bullet_ref;

	public float shooting_rate;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("Player");

		temp = transform.position;
		moving_towards = true;
		time = 0.0f;
		time2 = 0.0f;
		time3 = 0.0f;

		name = "GreenMonster";

		appliedForce = false;
		dead = false;

		min_distance = 10;
	}

	public void getPushed(string mode)
	{
		Vector2 direction = player.transform.position - transform.position;
		float distance = direction.magnitude;
		float force_size = 10.0f;
		direction.Normalize();

		appliedForce = true;
		GetComponent<Rigidbody2D>().drag = 0;
		if (mode == "push")
		{
			GetComponent<Rigidbody2D>().AddForce(-direction * (force_size / distance) * 60);
		}
		else
		{
			GetComponent<Rigidbody2D>().AddForce(direction * (force_size / distance) * 60);
		}

	}

	void handleShooting()
	{
		time3 += Time.deltaTime;
		float distance = Vector2.Distance(transform.position, player.transform.position);

		if (time3 > shooting_rate && dead == false && distance < 6)
		{
			Instantiate(bullet_ref, this.transform.position, transform.rotation);
			time3 = 0.0f;
		}
	}

	// Update is called once per frame
	void Update () {
		handleShooting ();

		if (Vector3.Distance (player.transform.position, transform.position) > min_distance)
			return;

		if (dead)
			return;

	

		// From here
		if (appliedForce) {
			time2 += Time.deltaTime;
			if (time2 > .8f) {
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

	void OnCollisionEnter2D(Collision2D other){

		if (other.gameObject.tag == "Player") {
			bounceOffPlayer ();
		}

		if (other.gameObject.tag == "Rock" && other.gameObject.GetComponent<Rigidbody2D> ().velocity.magnitude > 6) {
			Instantiate (exploding_pieces, transform.position, transform.rotation);
			if (BoardCreator.instance != null)
				BoardCreator.instance.SendMessage ("kill");
			Destroy (this.gameObject);	
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Spike") {
			Instantiate(exploding_pieces, transform.position, transform.rotation);
			if (BoardCreator.instance != null)
				BoardCreator.instance.SendMessage("kill");
			Destroy(this.gameObject);
		}

	}
}
