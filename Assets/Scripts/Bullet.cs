using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	GameObject char_ref;
	Vector3 target;
	float speed;
	Rigidbody2D rb;
	float clock;


	// Use this for initialization
	void Start () {

		char_ref = GameObject.FindGameObjectWithTag ("Player");
		rb = GetComponent<Rigidbody2D> ();
		target = char_ref.transform.position;
		Vector2 distance = char_ref.transform.position - transform.position;

		rb.AddForce (distance.normalized * 250);	

	}
	
	// Update is called once per frame
	void Update () {
		clock += Time.deltaTime;

		this.transform.eulerAngles = new Vector3(0,0,360*clock * 3);
		//transform.position = Vector3.MoveTowards (transform.position, target, Time.deltaTime * 3);
	
	}
}
