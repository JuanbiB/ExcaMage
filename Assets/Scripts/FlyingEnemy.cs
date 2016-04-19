using UnityEngine;
using System.Collections;

public class FlyingEnemy : MonoBehaviour {

	GameObject player;
	Rigidbody2D rb;
	Vector3 temp;
	int speed;
	bool moving_towards;
	float time;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		speed = 3;

		temp = transform.position;
		moving_towards = true;
		time = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 direction = player.transform.position - transform.position;
		direction.Normalize ();

		temp = transform.position + direction * Time.deltaTime * speed;

		if (moving_towards == true) {
			rb.MovePosition (temp);
		} else {
			time += Time.deltaTime;
		}

		if (time > .8f) {
			time = 0.0f;
			moving_towards = true;
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Player") {
			Vector3 dir = player.transform.position - transform.position;
			dir.Normalize ();
			rb.AddForce (-dir * 250);
			moving_towards = false;
		}
	}
}
