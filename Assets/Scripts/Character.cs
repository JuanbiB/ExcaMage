using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Character : MonoBehaviour
{

    Rigidbody2D body;
    Rigidbody2D enemybody;
    List<GameObject> go;
    SpriteRenderer sp_render;

	public bool DialogueEnabled;

	public GameObject bullet_ref;

	public bool bossfightEnabled;


    // Prefabs
    public GameObject magnet_wave_prefab;
    GameObject magnet_wave;
    public GameObject push_wave_prefab;
    GameObject push_wave;

    GameObject pushArea;

    public GameObject broken_tile;

    public int health = 5;
	public int maxhealth;

	public int ammo;
	bool fired; //have they fired

    // Variables
    float time;
    float counter;
    int allowed_radius;
    int character_speed;
    bool hit;
    bool animation_happening;
    string direction_facing;
    bool interrupt_animation;

    public bool addForce_BigFireball;
    public int by_how_much;

    // This is for movement
    Vector3 temp;

    // Animation!
    Animator pull_anim_controller;
    Animator push_anim_controller;
    Animator push_wave_controller;
    Animator my_animator;

    //Color stuff
    Color original;

    // Use this please
    public static Character instance = null;

    public string mode;
    // for pushing and pulling

    public Texture2D cursor;

    void Awake()
    {
        this.health = 7;
		this.maxhealth = health;
		this.ammo = 0;
		this.bossfightEnabled = false;
		this.DialogueEnabled = false;
    }


    // Use this for initialization
    void Start()
    {

        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);

        // Player rigidbody management
        body = GetComponent<Rigidbody2D>();


        // Put in place to avoid character to spin off when hit.
        body.freezeRotation = true;

        // Sprite renderer, used for colors and transparency.
        sp_render = GetComponent<SpriteRenderer>();

        // All current enemies in scene with "Enemy" tag. Used for Push and Pull.
        go = new List<GameObject>();
        go.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
		go.AddRange (GameObject.FindGameObjectsWithTag ("Rock"));
        go.AddRange(GameObject.FindGameObjectsWithTag("goof"));


        // The sprite that represents the "magnet wave". Pure aesthetics.
        Instantiate(magnet_wave_prefab, transform.position, transform.rotation);
        magnet_wave = GameObject.Find("Magnet-Wave(Clone)");

        // The sprite that represents the wi-fi esque push
        push_wave = (GameObject)Instantiate(push_wave_prefab, transform.position, transform.rotation);
        push_wave.transform.localScale = new Vector3(20, 20, 0);

        // How fast toon moves.
        character_speed = 7;

        // Lock so that you don't push and pull and the same time.
        animation_happening = false;

        //How far your magnet power should be able to reach
        allowed_radius = 8;

        //Used to keep track of push animation
        direction_facing = "right";

        // Just initializing movement variable
        temp = transform.position;

        // Getting the UI's animators
        push_anim_controller = GameObject.Find("Canvas/Push Cooldown").GetComponent<Animator>();
        pull_anim_controller = GameObject.Find("Canvas/Pull Cooldown").GetComponent<Animator>();


        // Getting the push animator
        push_wave_controller = push_wave.GetComponent<Animator>();
        push_wave.GetComponent<SpriteRenderer>().enabled = false;

        // Getting the player's animator
        my_animator = GetComponent<Animator>();

        //Quick fix
        transform.position += new Vector3(0, 0, -3);

        original = sp_render.color;
        //this.gameObject.tag = "Character";
        interrupt_animation = false;

        // Making an instance for non-monobehaviors
        instance = this;

        // For the new area stuff!
         pushArea = GameObject.Find("Push-Find");

        mode = "none";

    }

	public void refreshListofEnemies(){
		go.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        go.AddRange(GameObject.FindGameObjectsWithTag("Rock"));
    }

    void handleCursor()
    {

    }

    // Update is called once per frame
    void Update()
    {
		if (DialogueEnabled == true) {
			//do Nothing
		} else {
			fixConstants ();
			if (health > 0 && bossfightEnabled == false) {
				handleInput ();
			}
			check_drag ();
			if (health > 0 && bossfightEnabled == true) {
				handleInput (); // we need to replace input script so that it only allows you to move left + right.
				fire ();
			}
		}
    }

	void fire(){
		if (Input.GetKeyUp(KeyCode.L) && this.ammo >0){
			print ("Bullet fired");
			fired = true;

			GameObject bullet = Instantiate(bullet_ref, (transform.position+1.0f*transform.forward) ,Quaternion.identity) as GameObject;
			ammo--;
			//bullet.GetComponent<PurpleBullet>().
			//bullet.tag = "Ammo";
			//bullet.name = "Ammo"; //for some reason this doesn't work

			}
		}


    void handleInput()
    {

        if (my_animator.GetCurrentAnimatorStateInfo(0).IsName("player_idle"))
        {
            interrupt_animation = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!push_anim_controller.GetCurrentAnimatorStateInfo(0).IsName("push_anim"))
            {
                mode = "push";
                StartCoroutine(magnet_animation(1));
                push_anim_controller.Play("push_anim");
                my_animator.Play("player_push_animation");
            }

        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!pull_anim_controller.GetCurrentAnimatorStateInfo(0).IsName("pull_anim"))
            {
                mode = "pull";
                StartCoroutine(magnet_animation(2));
                pull_anim_controller.Play("pull_anim");
                my_animator.Play("player_pull");
            }

        }

            //Movement
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            temp = transform.position + new Vector3(-1, -1, 0) * Time.deltaTime * character_speed;
            direction_facing = "down-left";
        }

        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
        {
            temp = transform.position + new Vector3(-1, 1, 0) * Time.deltaTime * character_speed;
            direction_facing = "up-left";
        }

        else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
        {
            temp = transform.position + new Vector3(1, -1, 0) * Time.deltaTime * character_speed;
            direction_facing = "down-right";
        }

        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            temp = transform.position + new Vector3(1, 1, 0) * Time.deltaTime * character_speed;
            direction_facing = "up-right";
        }

        else if (Input.GetKey(KeyCode.W))
        {
            temp = transform.position + Vector3.up * Time.deltaTime * character_speed;
            direction_facing = "up";

            if (interrupt_animation == false)
                my_animator.Play("player_up");
        }

        else if (Input.GetKey(KeyCode.D))
        {
            temp = transform.position + Vector3.right * Time.deltaTime * character_speed;
            direction_facing = "right";

            if (interrupt_animation == false)
                my_animator.Play("player_right");

        }

        else if (Input.GetKey(KeyCode.S))
        {
            temp = transform.position + -Vector3.up * Time.deltaTime * character_speed;
            direction_facing = "down";
        }

        else if (Input.GetKey(KeyCode.A))
        {
            temp = transform.position + -Vector3.right * Time.deltaTime * character_speed;
            direction_facing = "left";

            if (interrupt_animation == false)
                my_animator.Play("player_left");
        }

        body.MovePosition(temp);


    }


    void revertColors()
    {
        foreach (GameObject g in go)
        {
            if (g != null)
                g.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void check_drag()
    {
        // This loop increases drag of enemies, so that they slow down and not fly off with a constant velocity.
        for (int i = 0; i < go.Count; i++)
        {
            if (go[i] != null && go[i].gameObject.tag != "PurpBullet" && go[i].GetComponent<Rigidbody2D>() != null)
            {
                enemybody = go[i].GetComponent<Rigidbody2D>();
                enemybody.drag += Time.deltaTime * 3;

                // When they reach a certain point the drag will make them stand still, so you want to reset it for the next time you AddForce.
                if (enemybody.IsSleeping() ||
                    (Input.GetKeyUp(KeyCode.J) && !pull_anim_controller.GetCurrentAnimatorStateInfo(0).IsName("pull_anim"))
                    || (Input.GetKeyUp(KeyCode.K) && !push_anim_controller.GetCurrentAnimatorStateInfo(0).IsName("push_anim")))
                {
                    enemybody.drag = 0;
                }
            }
        }
    }

    void fixConstants()
    {
        // Put in place to prevent enemies from transmitting velocity to character if they hit him.
        body.velocity = Vector3.zero;
        push_wave.transform.position = transform.position;

    }


    // Strictly for the magnet wave animation
    void decreaseOpacity()
    {
        Color temp = magnet_wave.GetComponent<SpriteRenderer>().color;
        temp.a -= 0.04f;
        magnet_wave.GetComponent<SpriteRenderer>().color = temp;
    }

    public IEnumerator magnet_animation(int type)
    {
        animation_happening = true;
        magnet_wave.transform.position = transform.position;

        Color original = magnet_wave.GetComponent<SpriteRenderer>().color;
        interrupt_animation = true;
        // Pull animation
        if (type == 2)
        {
            // Want to start big, then diminish in size, so simulates "pulling".
            magnet_wave.transform.localScale = new Vector3(8, 8, 1);
            while (magnet_wave.transform.localScale.x > .01f)
            {
                decreaseOpacity();
                magnet_wave.transform.localScale -= Vector3.one * Time.deltaTime * 20;
                yield return null;
            }

            //In place to fix the animations

        }
        // Push animation
        else
        {
            Vector3 change = Vector3.zero;
            float time = 0.0f;

            magnet_wave.transform.localScale = new Vector3(3, 3, 0);

            Vector2 mouse = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            mouse = Camera.main.ScreenToWorldPoint(mouse);

            mouse = transform.InverseTransformPoint(mouse);
            change = mouse.normalized;


            // Starting small, then increasing in size, simulating "pushing".
            while (magnet_wave.GetComponent<SpriteRenderer>().color.a > 0)
            {
                decreaseOpacity();
                time += Time.deltaTime;

                magnet_wave.transform.position += change * Time.deltaTime * 15;
                magnet_wave.transform.localScale += Vector3.one * Time.deltaTime * 2;
                yield return null;
            }
        }

        magnet_wave.transform.localScale = new Vector3(0, 0, 1);
        magnet_wave.GetComponent<SpriteRenderer>().color = original;
        animation_happening = false;


        push_wave.transform.eulerAngles = Vector3.zero;
        push_wave.GetComponent<SpriteRenderer>().enabled = false;
        push_wave_controller.Play("idle");
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

    public IEnumerator invunerable(float time)
    {
        hit = true;
        float start = 0.0f;
        while (start < time)
        {
            time += Time.deltaTime;
            yield return null;
        }
        hit = false;
    }

	void addAmmo(Collider2D coll){
		this.ammo++;
		Destroy (coll.gameObject);
		print (this.ammo);
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            if (!hit)   // In place to prevent being damaged when you've already been recently damaged.
            {
                StartCoroutine(hit_animation());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Bullet")
        {
            if (!hit)
            {
                StartCoroutine(hit_animation());
                Destroy(coll.gameObject);
            }
            else {
                Destroy(coll.gameObject);
            }
        }

		if (coll.gameObject.tag == "PurpBullet") {
			if (ammo > 0) {
				print ("BONES");
			}
			 if (!hit) {

				//Need to create an instance that the bullet will not hurt if it is being absorbed
				if (animation_happening == true) {
					addAmmo (coll);

				} else {

					StartCoroutine (hit_animation ());
					Destroy (coll.gameObject);
				}
			} else {
				if (animation_happening == true) {
					addAmmo (coll);
				}
				Destroy (coll.gameObject);
			}
		}

        if (coll.gameObject.tag == "Finish")
        {
            Vector3 curr = this.transform.position;
            this.transform.position = new Vector3(curr.x + 30, curr.y, curr.z);
        }

		if (coll.gameObject.tag == "ChangeLevel")
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
		}
    }
}
	
