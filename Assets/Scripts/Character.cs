using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    Rigidbody2D body;
    Rigidbody2D enemybody;
    GameObject[] go;
	SpriteRenderer sp_render;

	// Prefabs 
    public GameObject magnet_wave_prefab;
	GameObject magnet_wave;
    public GameObject broken_tile;

	// Variables
    float time;
    float counter;
	int allowed_radius;
	int character_speed;
    bool hit;
    bool animation_happening;
	string direction_facing;

    public bool addForce_BigFireball;
    public int by_how_much;


    // Use this for initialization
    void Start () {

		// Player rigidbody management
        body = GetComponent<Rigidbody2D>();
        
        // Put in place to avoid character to spin off when hit.
		body.freezeRotation = true;

        // Sprite renderer, used for colors and transparency.
        sp_render = GetComponent<SpriteRenderer>();

        // All current enemies in scene with "Enemy" tag. Used for Push and Pull.
        go = GameObject.FindGameObjectsWithTag("Enemy");

        // The sprite that represents the "magnet wave". Pure aesthetics.
		Instantiate(magnet_wave_prefab, transform.position, transform.rotation);
		magnet_wave = GameObject.Find ("Magnet-Wave(Clone)");

        // How fast toon moves.
        character_speed = 4;

        // Lock so that you don't push and pull and the same time.
        animation_happening = false;

        //How far your magnet power should be able to reach
        allowed_radius = 6;

		//Used to keep track of push animation
		direction_facing = "right";

	}

	// Update is called once per frame
	void Update () {

		fixConstants ();
		handleInput();
		check_drag();

	}

    void handleInput()
    {
        //Movement
		if (Input.GetKey (KeyCode.W)) {
			transform.position += Vector3.up * Time.deltaTime * character_speed;
			direction_facing = "up";
		}

		if (Input.GetKey (KeyCode.S)) {
			transform.position += -Vector3.up * Time.deltaTime * character_speed;
			direction_facing = "down";
		}

		if (Input.GetKey (KeyCode.D)) {
			transform.position += Vector3.right * Time.deltaTime * character_speed;
			direction_facing = "right";
		}


		if (Input.GetKey (KeyCode.A)) {
			transform.position += -Vector3.right * Time.deltaTime * character_speed;
			direction_facing = "left";
		}

        // Animation
        if (!animation_happening)
        {
            // Pull
            if (Input.GetKeyDown(KeyCode.J))
            {
                // This is the actual adding of force to enemies.
                applyEnemies(1);

                // This is just the magnet animation.
                StartCoroutine(magnet_animation(2));

                if (addForce_BigFireball)
                    addForceBullets("pull");
            }

            // Push
            if (Input.GetKeyDown(KeyCode.K))
            {
	              applyEnemies(2);
	              StartCoroutine(magnet_animation(1));

                if (addForce_BigFireball)
                    addForceBullets("push");
            }
        }

        // Breaking of tile
        // Creates a broken tile to your right!
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(broken_tile, new Vector2(transform.position.x + 1, transform.position.y), transform.rotation);
        }
    }

    void check_drag()
    {
        // This loop increases drag of enemies, so that they slow down and not fly off with a constant velocity. 
        for (int i = 0; i < go.Length; i++)
        {
            if (go[i] != null)
            {

                enemybody = go[i].GetComponent<Rigidbody2D>();
                enemybody.drag += Time.deltaTime * 3;

                // When they reach a certain point the drag will make them stand still, so you want to reset it for the next time you AddForce.
                if (enemybody.IsSleeping() || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
                {
                    enemybody.drag = 0;
                    enemybody.velocity = Vector2.zero;
                }
            }
        }
    }

	void fixConstants(){
		// Put in place to prevent enemies from transmitting velocity to character if they hit him. 
		body.velocity = Vector3.zero;
	}

	void applyEnemies(int porp)
	{
		for (int i = 0; i < go.Length; i++)
		{
			// Checking if the enemy has been destroyed. Don't access if true.
			if (go [i] != null) {
				enemybody = go [i].GetComponent<Rigidbody2D> ();
				Transform target = go [i].transform;

				// Refer: http://answers.unity3d.com/questions/1167656/choosing-speed-and-direction-of-addforce.html#answer-1167720
				Vector2 dir = transform.position - target.position;  // Direction between character and enemy.
				float distance = dir.magnitude; // Distance between two in float form, instead of Vector.
				float force_size = 10.0f;
				dir.Normalize();    // Makes the distance have a magnitude of 1.

				float distance_check = Vector2.Distance(transform.position, target.position);
				if (distance_check < allowed_radius)
				{
					if (porp == 1)
					{
						// Inverse linear force equation.
						enemybody.AddForce(dir * (force_size / distance) * 50);

					}
					else {
						// Variables we're going to need to make the calculations for the push						
						Vector3 angle_dir = target.position - transform.position;
						Vector3 facing = transform.InverseTransformPoint (target.position);
						float angle = Vector3.Angle (transform.up, angle_dir.normalized);
						
						//print (angle);
						//print(Vector3.Distance(transform.position, target.position));
	
						// Find me if you need me to explain these particular mechanics, they're not that tough. 
						if (direction_facing == "right" && facing.x > 0) {
							if (angle >= 45 && angle <= 135) {
								enemybody.AddForce (-dir * (force_size / distance) * 50);
							}

						} else if (direction_facing == "down") {
							if (angle >= 65) {	// used to be 135
								enemybody.AddForce (-dir * (force_size / distance) * 50);
							}
						} 

						else if (direction_facing == "left") {
							if (angle >= 45 && angle <= 135 && facing.x < 0) {
								enemybody.AddForce (-dir * (force_size / distance) * 50);
							}

						} else if (direction_facing == "up") {
							if (angle <= 75) { //used to be 45
								enemybody.AddForce (-dir * (force_size / distance) * 50);
							}
						}

					}
				}
			}
		}
	}

    void addForceBullets(string push_or_pull)
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        Rigidbody2D bullet_rb = enemybody;  // Just a holder variable to initialize it.

        foreach (GameObject potential_bullet in bullets)
        {
            if (potential_bullet.name != "BigFireball")
                continue;

            bullet_rb = potential_bullet.GetComponent<Rigidbody2D>();

            // Taken from applyEnemies method
            Vector2 dir = transform.position - potential_bullet.transform.position; 
            float distance = dir.magnitude; 
            float force_size = 10.0f;
            dir.Normalize();

            if (push_or_pull == "push")
            {
                // Yeah, shitload of repeated code here from applyEnemies... sue me. 
                // Nah but it's stable and works nicely so w/e.

                Vector3 angle_dir = bullet_rb.transform.position - transform.position;
                Vector3 facing = transform.InverseTransformPoint(bullet_rb.transform.position);
                float angle = Vector3.Angle(transform.up, angle_dir.normalized);

                if (direction_facing == "right" && facing.x > 0)
                {
                    if (angle >= 45 && angle <= 135)
                    {
                        bullet_rb.AddForce(-dir * (force_size / distance) * by_how_much);
                    }

                }
                else if (direction_facing == "down")
                {
                    if (angle >= 65)
                    {   // used to be 135
                        bullet_rb.AddForce(-dir * (force_size / distance) * by_how_much);
                    }
                }

                else if (direction_facing == "left")
                {
                    if (angle >= 45 && angle <= 135 && facing.x < 0)
                    {
                        bullet_rb.AddForce(-dir * (force_size / distance) * by_how_much);
                    }

                }
                else if (direction_facing == "up")
                {
                    if (angle <= 75)
                    { //used to be 45
                        bullet_rb.AddForce(-dir * (force_size / distance) * by_how_much);
                    }
                }

               
            }

            else if (push_or_pull == "pull")
            {
                bullet_rb.AddForce(dir * (force_size / distance) * by_how_much);
            }
            
            print("added force");
        }

    }

	// Strictly for the magnet wave animation
	void decreaseOpacity(){
		Color temp = magnet_wave.GetComponent<SpriteRenderer> ().color;
		temp.a -= 0.04f;
		magnet_wave.GetComponent<SpriteRenderer> ().color = temp;
	}

    public IEnumerator magnet_animation(int type)
    {
        animation_happening = true;
		magnet_wave.transform.position = this.transform.position;
		Color original = magnet_wave.GetComponent<SpriteRenderer> ().color;

        // Pull animation
        if (type == 2)
        {
            // Want to start big, then diminish in size, so simulates "pulling".
            magnet_wave.transform.localScale = new Vector3(8, 8, 1);
            while (magnet_wave.transform.localScale.x > .01f)
            {
				decreaseOpacity ();
                magnet_wave.transform.localScale -= Vector3.one * Time.deltaTime * 20;
                yield return null;
            }
        }
        // Push animation
        else
        {
			Vector3 change = Vector3.zero;
			float time = 0.0f;

			magnet_wave.transform.localScale = new Vector3 (1.5f, 1.5f, 0);
			if (direction_facing == "right")
				change = new Vector3 (1, 0, 0);
			else if (direction_facing == "down")
				change = new Vector3 (0, -1, 0);
			else if (direction_facing == "left")
				change = new Vector3 (-1, 0, 0);
			else if (direction_facing == "up")
				change = new Vector3 (0, 1, 0);

            // Starting small, then increasing in size, simulating "pushing".
			while (magnet_wave.GetComponent<SpriteRenderer>().color.a > 0)
            {
				decreaseOpacity ();
				time += Time.deltaTime;
					
				magnet_wave.transform.position += change * Time.deltaTime * 10;
              	magnet_wave.transform.localScale += Vector3.one * Time.deltaTime * 3;
                yield return null;
            }
  		}

        magnet_wave.transform.localScale = new Vector3(0, 0, 1);
		magnet_wave.GetComponent<SpriteRenderer> ().color = original;
        animation_happening = false;
    }

    public IEnumerator hit_animation()
    {
        hit = true;
        float time = 0.0f;
        Color original = sp_render.color;
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

    void OnCollisionEnter2D(Collision2D coll)
    {
		if (coll.gameObject.tag == "Enemy")
        {
            if (!hit)   // In place to prevent being damaged when you've already been recently damaged.
            {
                print("hit");
                StartCoroutine(hit_animation());
            }
        }
    }

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Bullet") {
			if (!hit) {
				StartCoroutine (hit_animation ());
				Destroy (coll.gameObject);
			} else {
				Destroy (coll.gameObject);
			}
		}
	}
}
