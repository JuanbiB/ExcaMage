using UnityEngine;
using System.Collections;

public class bigBullet : MonoBehaviour {

	GameObject char_ref;
	Rigidbody2D rb;
	public float speed;

	Vector2 distance;


	// Use this for initialization
	void Start () {

		char_ref = GameObject.FindGameObjectWithTag ("Player");
		rb = GetComponent<Rigidbody2D> ();
		speed = 250;

		distance = char_ref.transform.position - transform.position;
		float angle = Vector3.Angle (Vector3.up, distance);

		Vector3 facing = transform.InverseTransformPoint(char_ref.transform.position);

		if (facing.x > 0) {
			transform.Rotate (0, 0, -angle);
		}
		else {
			transform.Rotate (0, 0, angle);
		}

		rb.AddForce (distance.normalized * speed);
		this.name = "bigBullet";

	}



	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Wall")
		{
			Destroy(this.gameObject);
		}
	}
}

