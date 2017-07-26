using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	GameObject hero;
	GameObject baddie;

	Transform target;
	Transform me;

	// Use this for initialization
	void Start () {
		hero = GameObject.Find ("Character");
		me = hero.transform;
		baddie = GameObject.Find ("Enemy");
		target = baddie.transform;

		Vector3 vec = target.position - me.position;

		float angle = Vector3.Angle (me.up, vec);
		//print (360 - angle);
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 forward = hero.transform.TransformDirection(Vector3.forward);
		Vector3 toOther = baddie.transform.position - hero.transform.position;

		Vector3 vec = target.position - me.position;

		float angle = Vector3.Angle (me.up, vec);

	}
}
