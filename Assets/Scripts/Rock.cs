using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour
{
	Rigidbody2D body;
	BoxCollider2D coll;
    GameObject player;

    public GameObject pieces;

	void Start()
	{
		body = GetComponent<Rigidbody2D>();
		body.freezeRotation = true;
		coll = GetComponent<BoxCollider2D>();
        player = GameObject.FindWithTag("Player");

        name = "Rock";
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
		if (coll.gameObject.tag == "Wall" && body.velocity.magnitude > 7){
            Instantiate(pieces, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}
}
