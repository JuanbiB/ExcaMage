using UnityEngine;
using System.Collections;

public class hgcCC : MonoBehaviour {

	public GameObject player;
	public GameObject boss;

	bool islerp;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		boss = GameObject.FindGameObjectWithTag ("boss");
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (player.transform.position, boss.transform.position , 0.5f) + new Vector3 (0, 0, -10);
	
	}
}
