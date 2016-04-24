using UnityEngine;
using System.Collections;

public class SingleExplodedPiece : MonoBehaviour {

    // Basically want this to get a random direction and rotation, and fly off. 
    Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();

        int rot = Random.Range(0, 360);
        transform.Rotate(0, 0, rot);

        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);

        Vector3 direction = new Vector3(x, y, 0);

        rb.AddForce(direction.normalized * 200);
    }
	
	// Update is called once per frame
	void Update () {

        rb.drag += .01f;

	}
}
