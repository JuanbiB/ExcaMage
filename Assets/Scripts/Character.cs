using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    Rigidbody2D body;
    Rigidbody2D enemybody;
    GameObject[] go;
    GameObject magnet_wave;
    SpriteRenderer sp_render;
    public GameObject broken_tile;

    int character_speed;
    float time;
    float counter;
    bool hit;
    bool animation_happening;

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
        magnet_wave = GameObject.Find("Magnet-Wave");

        // How fast toon moves.
        character_speed = 4;

        // Lock so that you don't push and pull and the same time.
        animation_happening = false;

	}

    void applyEnemies(int porp)
    {
        for (int i = 0; i < go.Length; i++)
        {
			// Checking if the enemy has been destroyed. Don't access if true.
			if (go [i] != null) {
				enemybody = go [i].GetComponent<Rigidbody2D> ();

                // Refer: http://answers.unity3d.com/questions/1167656/choosing-speed-and-direction-of-addforce.html#answer-1167720
                Vector2 dir = this.transform.position - go [i].transform.position;  // Direction between character and enemy.
                float distance = dir.magnitude; // Distance between two in float form, instead of Vector.
                float force_size = 10.0f;
                dir.Normalize();    // Makes the distance have a magnitude of 1.

				if (porp == 1) {
                    // Inverse linear force equation.
                    enemybody.AddForce(dir * (force_size / distance) * 50);

                } else {
                    enemybody.AddForce(-dir * (force_size / distance) * 50);
                }
			}
        }
    }
	
	// Update is called once per frame
	void Update () {

        // Put in place to prevent enemies from transmitting velocity to character if they hit him. 
        body.velocity = Vector3.zero;

        // This keeps the magnet sprite always at characters position, and "invisible" so it's ready to be used when we push or pull.
        magnet_wave.transform.position = this.transform.position;

        //Movement
        if (Input.GetKey(KeyCode.W))
            transform.position += Vector3.up * Time.deltaTime * character_speed;

        if (Input.GetKey(KeyCode.S))
            transform.position += -Vector3.up * Time.deltaTime * character_speed;

        if (Input.GetKey(KeyCode.D))
            transform.position += Vector3.right * Time.deltaTime * character_speed;

        if (Input.GetKey(KeyCode.A))
            transform.position += -Vector3.right * Time.deltaTime * character_speed;

        // This loop increases drag of enemies, so that they slow down and not fly off with a constant velocity. 
		for (int i = 0; i < go.Length; i++) {
			if (go [i] != null) {

				enemybody = go [i].GetComponent<Rigidbody2D> ();
				enemybody.drag += Time.deltaTime * 3;

                // When they reach a certain point the drag will make them stand still, so you want to reset it for the next time you AddForce.
				if (enemybody.IsSleeping () || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K)) {
					enemybody.drag = 0;
					enemybody.velocity = Vector2.zero;
				}
			}
		}

        if (!animation_happening)
        {
            // Pull
            if (Input.GetKeyDown(KeyCode.J))
            {
                // This is the actual adding of force to enemies.
                applyEnemies(1);

                // This is just the magnet animation.
                StartCoroutine(magnet_animation(2));
            }

            // Pull
            if (Input.GetKeyDown(KeyCode.K))
            {
                applyEnemies(2);
                StartCoroutine(magnet_animation(1));
            }
        }

        // Creates a broken tile to your right!
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(broken_tile, new Vector2(transform.position.x + 1, transform.position.y), transform.rotation);
        }
	}

    public IEnumerator magnet_animation(int type)
    {
        animation_happening = true;
        // Pull animation
        if (type == 2)
        {
            // Want to start big, then diminish in size, so simulates "pulling".
            magnet_wave.transform.localScale = new Vector3(6, 6, 1);
            while (magnet_wave.transform.localScale.x > .01f)
            {
                magnet_wave.transform.localScale -= Vector3.one * Time.deltaTime * 20;
                yield return null;
            }
        }
        // Push animation
        else
        {
            // Starting small, then increasing in size, simulating "pushing".
            while (magnet_wave.transform.localScale.x < 8)
            {
                magnet_wave.transform.localScale += Vector3.one * Time.deltaTime * 20;
                yield return null;
            }
        }
        magnet_wave.transform.localScale = new Vector3(.01f, .01f, 1);
        animation_happening = false;
    }

    // This just reduces opacity and changes the color to red for 1.5f seconds. Ghost mechanic.
    // TODO: Make it oscillate between low and high transparency, like most games when you get hit?
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
}
