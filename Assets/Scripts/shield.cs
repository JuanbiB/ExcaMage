using UnityEngine;
using System.Collections;

public class shield : MonoBehaviour {

	GameObject player;
	float time;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
		time = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		transform.position = player.transform.position;

		if (time > 2) {
			Destroy (gameObject);
		}
	}
}
