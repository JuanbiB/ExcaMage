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

	public IEnumerator spiked()	
	{
		float time = 0.0f;
		Quaternion qua = Quaternion.Euler(new Vector3(0, 0, -90));
		while (time < 1.5)
		{
			time += Time.deltaTime;
			// This just turns the enemy 90 degrees. I guess the direciton has to do with where you the spike from, but that's TODO.
			transform.rotation = Quaternion.Slerp(transform.rotation, qua, Time.deltaTime * 5);
			yield return null;
		}
		gameObject.GetComponent<SpriteRenderer>().color = Color.red;
		BoardCreator.instance.SendMessage("kill");
		Destroy (this);
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Player") {
			Vector3 dir = player.transform.position - transform.position;
			dir.Normalize ();
			rb.AddForce (-dir * 250);
			moving_towards = false;
		}
		if (other.gameObject.tag == "Spike") {
			spiked ();
		}
	}
}
