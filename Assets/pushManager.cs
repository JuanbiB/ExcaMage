using UnityEngine;
using System.Collections;

public class pushManager : MonoBehaviour {

    Character player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y, -3);
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.gameObject.tag == "Enemy") {
			if (other.name == "Enemy")
				other.gameObject.GetComponent<Enemy> ().getPushed (player.GetComponent<Character> ().mode);

			if (other.name == "FlyingMonster")
				other.gameObject.GetComponent<FlyingEnemy> ().getPushed (player.GetComponent<Character> ().mode);
//			if (other.name == "BigEnemy")
//				other.gameObject.GetComponent<BigEnemy>().getPushed(player.GetComponent<Character>().mode);
		} else if (other.gameObject.tag == "Rock") {
			if (other.name == "Rock")
				other.gameObject.GetComponent<Rock> ().getPushed (player.GetComponent<Character> ().mode);
			if (other.name == "Boulder_Bullet")
				other.gameObject.GetComponent<Boulder_Bullet> ().getPushed (player.GetComponent<Character> ().mode);
		}

        
    }
}
