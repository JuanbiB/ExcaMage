using UnityEngine;
using System.Collections;

public class PurpleBullet : MonoBehaviour {

	GameObject char_ref;
	Rigidbody2D rb;
	public float speed;

	Vector2 distance;

	// Use this for initialization
	void Start () {
		char_ref = GameObject.FindGameObjectWithTag ("Player");
		rb = GetComponent<Rigidbody2D> ();
		speed = 100;

		distance = char_ref.transform.position - transform.position;
		float angle = Vector3.Angle (Vector3.up, distance);

		print (angle);

		Vector3 facing = transform.InverseTransformPoint(char_ref.transform.position);

		if (facing.x > 0) {
			transform.Rotate (0, 0, -angle);
		}
		else {
			transform.Rotate (0, 0, angle);
		}

		rb.AddForce (distance.normalized * speed);
		this.name = "PurpBullet";
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void getPushed(string mode)
	{
		Vector2 direction = char_ref.transform.position - transform.position;
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

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Wall")
		{
			Destroy(this.gameObject);
		}
	}
}
