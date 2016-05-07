using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {
	GameObject boss;
	Rigidbody2D rb;
	PolygonCollider2D col;
	[SerializeField] private float speed;

	Vector2 distance;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		speed = 250;
		boss = GameObject.FindGameObjectWithTag ("boss");

		Vector2 mouse = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		mouse = Camera.main.ScreenToWorldPoint(mouse);

		distance = mouse - (Vector2) transform.position;

		float angle = Vector3.Angle (Vector3.up, distance);

		Vector3 facing = transform.InverseTransformPoint(mouse);

		if (facing.x > 0) {
			transform.Rotate (0, 0, -angle);
		}
		else {
			transform.Rotate (0, 0, angle);
		}



		rb.AddForce (distance.normalized * speed);
	}
	
	// Update is called once per frame
	//We need the bullet to fire forward
	void Update () {
	
	}
}
