using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	GameObject char_ref;
	Vector3 target;
	float speed;
	Rigidbody2D rb;


	// Use this for initialization
	void Start () {

		char_ref = GameObject.Find ("Character");
		rb = GetComponent<Rigidbody2D> ();
		target = char_ref.transform.position;
		Vector2 distance = char_ref.transform.position - transform.position;

		rb.AddForce (distance.normalized * 200);	
	}
	
	// Update is called once per frame
	void Update () {
		
		//transform.position = Vector3.MoveTowards (transform.position, target, Time.deltaTime * 3);
	
	}
}
