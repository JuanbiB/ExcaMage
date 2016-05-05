using UnityEngine;
using System.Collections;

public class BigEnemy : MonoBehaviour {

	// prefab management
	public GameObject exploded_pieces_prefab;
	GameObject exploded_pieces;

	Animator Big_Boy_Anim;
	Rigidbody2D body;
	BoxCollider2D coll;

	int shrink_speed;
	bool dead;

	Color enemy_color;
	Color original;

	float fade;
	float move_speed;

	GameObject char_ref;
	Vector3 distance;
	public GameObject bullet_ref;
	GameObject player;

	float time;
	public float shooting_rate;

	bool hit = false;
	int health = 2;
	SpriteRenderer sp_render;

	// Use this for initialization
	void Start()
	{
		Big_Boy_Anim = GetComponent<Animator>();
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
		player = GameObject.FindWithTag("Player");

		time = 0.0f;

		name = "BigEnemy";
		sp_render = GetComponent<SpriteRenderer>();

		body.constraints = RigidbodyConstraints2D.FreezeAll;
		original = sp_render.color;
	}

	// Update is called once per frame
	void Update()
	{
		if (this.health <= 0) {
			spikeDeath ();
			BoardCreator.instance.SendMessage ("kill");

		}
			handleShooting();


	}





//	public void getPushed(string mode)
//	{
//		Vector2 direction = player.transform.position - transform.position;
//		float distance = direction.magnitude;
//		float force_size = 10.0f;
//		direction.Normalize();
//
//		GetComponent<Rigidbody2D>().drag = 0;
//		if (mode == "push")
//		{
//			GetComponent<Rigidbody2D>().AddForce(-direction * (force_size / distance) * 60);
//		}
//		else
//		{
//			GetComponent<Rigidbody2D>().AddForce(direction * (force_size / distance) * 60);
//		}
//
//
//	}

IEnumerator throw_boulder(){
		yield return new WaitForSeconds(.3f);

		Instantiate(bullet_ref, transform.position, transform.rotation);
	}

	void spikeDeath()
	{
		Instantiate(exploded_pieces_prefab, transform.position, transform.rotation);
		//GetComponent<SpriteRenderer>().enabled = false;
		Destroy(gameObject);
	}

	void handleShooting()
	{
		time += Time.deltaTime;
		float distance = Vector2.Distance(transform.position, char_ref.transform.position);

		if (time > shooting_rate && dead == false && distance < 6)
		{

			//Vector2 direction = player.transform.position - transform.position;
			//float dist = direction.magnitude;
			//float force_size = 10.0f;

			//direction.Normalize();

			Vector3 shot = this.transform.position;
			shot.x = shot.x + 1;
			shot.y = shot.y + 1;
			Big_Boy_Anim.Play("big_enemy_shooting");
			StartCoroutine(throw_boulder());
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

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Pitfall")
		{
			dead = true;
			if (BoardCreator.instance != null)
				BoardCreator.instance.SendMessage("kill");

			StartCoroutine(fall_death(coll.gameObject.transform.position));
		}
		else if (coll.gameObject.tag == "Spike")
		{
			if (BoardCreator.instance != null)
				BoardCreator.instance.SendMessage("kill");
			spikeDeath();
		}
	}


	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.name == "Boulder_Bullet")
		{
			if (!hit)   // In place to prevent being damaged when you've already been recently damaged.
			{
				StartCoroutine(hit_animation());
			}
		}
	}

	public IEnumerator hit_animation()
	{
		hit = true;
		health--;
		float time = 0.0f;
		while (time < 1.5f)
		{
			Color temp = sp_render.color;
			temp = Color.red;
			temp.a = .5f;
			sp_render.color = temp;
			time += Time.deltaTime;
			yield return null;
		}

		sp_render.color = original;
		hit = false;
	}

}
