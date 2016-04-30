using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;

	void Awake(){
		Time.timeScale = 1;
	}

	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	void Update () 
	{
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
		}
		transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, -10);
	}
}
