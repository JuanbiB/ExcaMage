using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SetDifficulty : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		// level 1 difficulty adjustments
		if (SceneManager.GetActiveScene ().name.Equals ("level1")) {

			if (PlayerPrefs.GetString ("difficulty").Equals ("easy")) {
				foreach (GameObject go in enemies) {

					if (go.name == "Enemy(Clone)") {
						go.GetComponent<Enemy> ().shooting_rate = 4;	
						go.GetComponent<Enemy> ().SetBulletSpeed (50);
					} else if (go.name == "FlyingEnemy(Clone)") {
						if (go.GetComponent<FlyingEnemy> () != null)
							go.GetComponent<FlyingEnemy> ().speed = 0.5f;
					}
				}
			} else if (PlayerPrefs.GetString ("difficulty").Equals ("medium")) {
				foreach (GameObject go in enemies) {

					if (go.name == "Enemy(Clone)") {
						go.GetComponent<Enemy> ().shooting_rate = 3;	
						go.GetComponent<Enemy> ().SetBulletSpeed (75);
					} else if (go.name == "FlyingEnemy(Clone)") {
						if (go.GetComponent<FlyingEnemy> () != null)
							go.GetComponent<FlyingEnemy> ().speed = 0.7f;
					}
				}
			} else if (PlayerPrefs.GetString ("difficulty").Equals ("hard")) {
				foreach (GameObject go in enemies) {

					if (go.name == "Enemy(Clone)") {
						go.GetComponent<Enemy> ().shooting_rate = 2;	
					} else if (go.name == "FlyingEnemy(Clone)") {
						if (go.GetComponent<FlyingEnemy> () != null)
							go.GetComponent<FlyingEnemy> ().speed = 1f;
					}
				}
			}
		}

		// level 2 adjustments. 
		else if (SceneManager.GetActiveScene ().name.Equals ("level2")) {	

			if (PlayerPrefs.GetString ("difficulty").Equals ("easy")) {

				foreach (GameObject go in enemies) {

					if (go.name == "GreenMonster") {
						go.GetComponent<GreenMonster> ().shooting_rate = 4;	
						go.GetComponent<GreenMonster> ().speed = 0.45f;
					} else if (go.name == "BigEnemy") {
						if (go.GetComponent<BigEnemy> () != null) {
							go.GetComponent<BigEnemy> ().shooting_rate = 3;
							go.GetComponent<BigEnemy> ().health = 1;
						}

					}
				}
			}

			else if (PlayerPrefs.GetString ("difficulty").Equals ("medium")) {

				foreach (GameObject go in enemies) {

					if (go.name == "GreenMonster") {
						go.GetComponent<GreenMonster> ().shooting_rate = 3;	
						go.GetComponent<GreenMonster> ().speed = 0.75f;
					} else if (go.name == "BigEnemy") {
						if (go.GetComponent<BigEnemy> () != null)
							go.GetComponent<BigEnemy> ().shooting_rate = 3;
					}
				}
			
			}

			else if (PlayerPrefs.GetString ("difficulty").Equals ("hard")) {

				foreach (GameObject go in enemies) {

					if (go.name == "GreenMonster") {
						go.GetComponent<GreenMonster> ().shooting_rate = 2;	
						go.GetComponent<GreenMonster> ().speed = 1.0f;
					} else if (go.name == "BigEnemy") {
						if (go.GetComponent<BigEnemy> () != null)
							go.GetComponent<BigEnemy> ().shooting_rate = 4;
					}
				}

			}

		} else {
			print ("didn't get a name");
		}

	}
}
