﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    // prefab management
    public GameObject exploded_pieces_prefab;
    GameObject exploded_pieces;

    Rigidbody2D body;
    BoxCollider2D coll;

    int shrink_speed;
    bool dead;

    Color enemy_color;

    float fade;
    float move_speed;

    GameObject char_ref;
    Vector3 distance;
    public GameObject bullet_ref;

    float time;
	public float shooting_rate;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        dead = false;
        coll = GetComponent<BoxCollider2D>();

        // Color management
        enemy_color = gameObject.GetComponent<SpriteRenderer>().color;
        fade = 1;

        shrink_speed = 2;

        move_speed = 2f;

        char_ref = GameObject.FindGameObjectWithTag("Player");


        time = 0.0f;
    }

	// Update is called once per frame
	void Update()
	{
		handleShooting();

	}

    void spikeDeath()
    {
        Instantiate(exploded_pieces_prefab, transform.position, transform.rotation);
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject);
    }

    void handleShooting()
    {
        time += Time.deltaTime;
        float distance = Vector2.Distance(transform.position, char_ref.transform.position);

        if (time > shooting_rate && dead == false && distance < 6)
        {
            Instantiate(bullet_ref, this.transform.position, transform.rotation);
            time = 0.0f;
        }
    }
		

    public IEnumerator fall_death(Vector2 pos)
    {
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        coll.isTrigger = true;
        //dead = true;

        float clock = 0.0f;
        do
        {
            // This basically does 3 things:

            // Moves the enemy towards the middle of the pit.
            this.transform.position = Vector2.MoveTowards(transform.position, pos, 3 * Time.deltaTime);
            // Diminishes size.
            this.transform.localScale -= Vector3.one * Time.deltaTime * shrink_speed;
            // Spins around.
            this.transform.eulerAngles = new Vector3(0, 0, 360 * clock);

            clock += Time.deltaTime;

            yield return null;

        } while (transform.localScale.x > 0);

        Destroy(gameObject);

    }

    public IEnumerator spike_death()
    {
        float time = 0.0f;
        Quaternion qua = Quaternion.Euler(new Vector3(0, 0, -90));
        while (time < 1.5)
        {
            time += Time.deltaTime;
            body.velocity = Vector2.zero;
            // This just turns the enemy 90 degrees. I guess the direciton has to do with where you the spike from, but that's TODO.
            transform.rotation = Quaternion.Slerp(transform.rotation, qua, Time.deltaTime * 5);
            yield return null;
        }
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        enemy_color = gameObject.GetComponent<SpriteRenderer>().color;

        body.isKinematic = true;
        coll.isTrigger = true;
		dead = true;
		
	}
	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Rock" && other.rigidbody.IsAwake()) {
			spikeDeath();
			BoardCreator.instance.SendMessage("kill");
		}
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Pitfall")
        {
            dead = true;
            StartCoroutine(fall_death(coll.gameObject.transform.position));
			BoardCreator.instance.SendMessage("kill");

        }
        else if (coll.gameObject.tag == "Spike")
        {
            spikeDeath();
			BoardCreator.instance.SendMessage("kill");
        }
		else if (coll.gameObject.tag == "Rock")
		{
			print ("rock");
			spikeDeath();
			BoardCreator.instance.SendMessage("kill");
		}
    }
}
