using UnityEngine;
using System.Collections;

public class Boulder_Bullet : MonoBehaviour {

	GameObject char_ref;
	Rigidbody2D rb;
	PolygonCollider2D pgC;
	Vector2 distance;
	GameObject player;


	public float speed;
	float clock;

	// Use this for initialization
	void Start () {
		pgC = GetComponent<PolygonCollider2D> ();
		char_ref = GameObject.FindGameObjectWithTag ("Player");
		rb = GetComponent<Rigidbody2D> ();
		rb.freezeRotation = true;
		distance = char_ref.transform.position - transform.position;
		rb.AddForce (distance.normalized * speed);	
		name = "Boulder_Bullet";
		player = GameObject.FindWithTag("Player");

	}

	// Update is called once per frame
	void Update () {
		pgC.isTrigger = false;
		// Continual spinning
		//clock += Time.deltaTime;
		//this.transform.eulerAngles = new Vector3(0,0,360*clock * 3);

	}

	public void getPushed(string mode)
	{
		Vector2 direction = player.transform.position - transform.position;
		float distance = direction.magnitude;
		float force_size = 10.0f;
		direction.Normalize();

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

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Wall"){
			Destroy(gameObject);
		}
	}
}
