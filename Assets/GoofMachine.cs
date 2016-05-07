using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GoofMachine : MonoBehaviour {

    GameObject player;
    bool get_it;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
        get_it = false;
    }
	
	// Update is called once per frame
	void Update () {
	    if (get_it)
            Camera.main.GetComponent<HUD>().getRekt();
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Spike" || other.gameObject.tag == "Pitfall")
        {
            Camera.main.GetComponent<HUD>().getRekt();
            player.GetComponent<Character>().presentation = true;

            GameObject[] lol = GameObject.FindGameObjectsWithTag("KillItSon");

            foreach( GameObject kek in lol)
            {
                kek.SetActive(false);
            }

            get_it = true;
            player.GetComponent<SpriteRenderer>().enabled = false;
            GameObject.Find("Canvas/Death").GetComponent<Image>().enabled = true;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Rock" && other.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > 6)
        {
            Camera.main.GetComponent<HUD>().getRekt();
            player.GetComponent<Character>().presentation = true;

            GameObject[] lol = GameObject.FindGameObjectsWithTag("KillItSon");

            foreach (GameObject kek in lol)
            {
                kek.SetActive(false);
            }

            get_it = true;
            player.GetComponent<SpriteRenderer>().enabled = false;
            GameObject.Find("Canvas/Death").GetComponent<Image>().enabled = true;
        }
    }
}
