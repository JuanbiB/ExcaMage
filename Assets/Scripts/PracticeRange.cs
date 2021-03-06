﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PracticeRange : MonoBehaviour {

	public GameObject enemy;
	public GameObject flyingEnemy;
    public GameObject rock;

	public void makeEnemy()
    {

        GameObject player = GameObject.Find ("Character");
		Instantiate (enemy, player.transform.position + transform.up, player.transform.rotation);
		player.GetComponent<Character> ().refreshListofEnemies ();

        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

	public void makeFlyingEnemy(){
		GameObject player = GameObject.Find ("Character");
		Instantiate (flyingEnemy, player.transform.position + new Vector3(1, 2, 0), player.transform.rotation);
        player.GetComponent<Character> ().refreshListofEnemies ();

        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    public void makeRock()
    {
        GameObject player = GameObject.Find("Character");
        Instantiate(rock, player.transform.position + new Vector3(2, 0, 0), player.transform.rotation);
        player.GetComponent<Character>().refreshListofEnemies();

        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }


}
