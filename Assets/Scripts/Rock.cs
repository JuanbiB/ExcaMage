using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour
{
	Rigidbody2D body;
	BoxCollider2D coll;

	void Start()
	{
		body = GetComponent<Rigidbody2D>();
		body.freezeRotation = true;
		coll = GetComponent<BoxCollider2D>();


	}

	// Update is called once per frame
	void Update()
	{
        
	}


	void OnTriggerEnter2D(Collider2D coll)
	{

	}
}
