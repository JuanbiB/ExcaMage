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
		rb.AddForce (distance.normalized * speed);
	
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
