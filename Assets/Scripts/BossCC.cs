using UnityEngine;
using System.Collections;

public class BossCC : MonoBehaviour {



	public GameObject player;
	public GameObject boss;

	bool islerp;

	GameObject[] bosslist;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (islerp == true) {
			print ("reaching lerpCamera");
			transform.position = Vector3.Lerp (boss.transform.position, transform.position , 0.9799f) + new Vector3 (0, 0, -10);

		}
	
	}

	//only called during cutscene
	public void findBoss(){
		islerp = true;
		GameObject [] bosslist = GameObject.FindGameObjectsWithTag ("boss");
		foreach (GameObject go in bosslist) {
			if (go.name == "HandgunCat" && go.activeInHierarchy) {
				boss = go;
			}
			if (go.name == "Reaper Jones" && go.activeInHierarchy) {
				boss = go;
			}
		}

	}
	public void setTarget(GameObject newTarget){
		boss = newTarget;
	}

	public void setlerptofalse(){
		islerp = false;
	}


}
