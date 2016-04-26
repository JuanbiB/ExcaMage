using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {
	GameObject char_ref;
	Rigidbody2D rb;
	PolygonCollider2D col;
	[SerializeField] private float speed;

	Vector2 dist;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		speed = 100;
		char_ref = GameObject.FindGameObjectWithTag ("Player");
		dist = new Vector3 (0,10,0) - char_ref.transform.position;
		rb.AddForce (dist.normalized * speed);
	
	}
	
	// Update is called once per frame
	//We need the bullet to fire forward
	void Update () {
	
	}
}
