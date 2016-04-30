using UnityEngine;
using System.Collections;

public class Boulder_Bullet : MonoBehaviour {

	GameObject char_ref;
	Rigidbody2D rb;
	PolygonCollider2D pgC;
	Vector2 distance;

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

	}

	// Update is called once per frame
	void Update () {
		pgC.isTrigger = false;
		// Continual spinning
		//clock += Time.deltaTime;
		//this.transform.eulerAngles = new Vector3(0,0,360*clock * 3);

	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Rock")
		{
			Destroy(this.gameObject);
		}
	}
}
