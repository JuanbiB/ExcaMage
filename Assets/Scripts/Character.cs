using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Character : MonoBehaviour
{

    Rigidbody2D body;
    Rigidbody2D enemybody;
    List<GameObject> go;
    SpriteRenderer sp_render;

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




    void Awake()
    {
        this.health = 7;
		this.maxhealth = health;
		this.ammo = 0;
		this.bossfightEnabled = false;
    }


    // Use this for initialization
    void Start()
    {

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

    }

	public void refreshListofEnemies(){
		go.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
	}

    // Update is called once per frame
    void Update()
    {
        fixConstants();
		if (health > 0 && bossfightEnabled == false)
        {
            handleInput();
        }
        check_drag();
		if (health > 0 && bossfightEnabled == true) {
			handleInput(); // we need to replace input script so that it only allows you to move left + right.
			fire ();
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

		if (!Input.GetKey(KeyCode.K))
        	body.MovePosition(temp);

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W))
            my_animator.Play("player_idle");

        push_and_pull();


        // Breaking of tile
        // Creates a broken tile to your right!
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(broken_tile, new Vector2(transform.position.x + 1, transform.position.y), transform.rotation);
        }
    }

    void push_and_pull()
    {
        if (Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.K))
            return;

        // Checks the recharge rate animation.
        if (!pull_anim_controller.GetCurrentAnimatorStateInfo(0).IsName("pull_anim"))
        {


            // Pull
			if (Input.GetKeyUp(KeyCode.J))
            {
                // This is the actual adding of force to enemies.
                applyForceToEnemies(1);

                // This is just the magnet animation.
                StartCoroutine(magnet_animation(2));

                // In place to fix animation splitting
                interrupt_animation = true;

                //Triggers the recharge rate animation.
                pull_anim_controller.Play("pull_anim");
                my_animator.Play("player_pull");


                if (addForce_BigFireball)
                    addForceBullets("pull");
            }
        }

        // Put in place to manage the showing of the projected push
        if (Input.GetKey(KeyCode.K))
            cone_check();

        // Push
        if (Input.GetKeyUp(KeyCode.K))
        {
            pushArea.GetComponent<SpriteRenderer>().enabled = false;
            revertColors();

            if (!push_anim_controller.GetCurrentAnimatorStateInfo(0).IsName("push_anim"))
            {   
                applyForceToEnemies(2);
                StartCoroutine(magnet_animation(1));
                push_anim_controller.Play("push_anim");

                if (addForce_BigFireball)
                    addForceBullets("push");
            }
        }
    }

    void revertColors()
    {
        foreach (GameObject g in go)
        {
            if (g != null)
                g.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    // WARNING: Lots of repeated code here, ¯\_(ツ)_/¯ so if you're sensitive to that stuff, look no further.
    // Character script is MY REALM >:)
    void cone_check()
    {
		if (push_anim_controller.GetCurrentAnimatorStateInfo (0).IsName ("push_anim")) {
			pushArea.GetComponent<SpriteRenderer> ().color = Color.red;
		} else {
			pushArea.GetComponent<SpriteRenderer> ().color = Color.white;

		}


        pushArea.transform.position = transform.position;
        pushArea.transform.localScale = new Vector3(10, 9, 0);

        for (int i = 0; i < go.Count; i++)
        {
            // Checking if the enemy has been destroyed. Don't access if true.
            if (go[i] != null)
            {
                enemybody = go[i].GetComponent<Rigidbody2D>();
                Transform target = go[i].transform;

                Vector2 dir = transform.position - target.position;  // Direction between character and enemy.
                float distance = dir.magnitude; // Distance between two in float form, instead of Vector.
                dir.Normalize();    // Makes the distance have a magnitude of 1.

                float distance_check = Vector2.Distance(transform.position, target.position);

                string nameCheck = go[i].gameObject.name;

                Vector3 angle_dir = target.position - transform.position;
                Vector3 facing = transform.InverseTransformPoint(target.position);
                float angle = Vector3.Angle(transform.up, angle_dir.normalized);

                Color changeTo = Color.blue;

                pushArea.GetComponent<SpriteRenderer>().enabled = true;

                // If it's within the allowed radius, then we highlight it. 
                if (distance_check < allowed_radius)
                {
                    if (direction_facing == "right")
                    {
                        pushArea.transform.localEulerAngles = new Vector3 (0, 0, -90);
                        if (angle >= 45 && angle <= 135 && facing.x > 0)
                        {
                            go[i].GetComponent<SpriteRenderer>().color = changeTo;
                        }
                        else
                        {
                            go[i].GetComponent<SpriteRenderer>().color = Color.white;
                        }
                    }

                    else if (direction_facing == "down")
                    {
                        pushArea.transform.localEulerAngles = new Vector3(0, 0, 180);
                        if (angle >= 65)
                        {   // used to be 135
                            go[i].GetComponent<SpriteRenderer>().color = changeTo;
                        }
                        else
                        {
                            go[i].GetComponent<SpriteRenderer>().color = Color.white;
                        }
                    }

                    else if (direction_facing == "left")
                    {
                        pushArea.transform.localEulerAngles = new Vector3(0, 0, 90);
                        if (angle >= 45 && angle <= 135 && facing.x < 0)
                        {
                            go[i].GetComponent<SpriteRenderer>().color = changeTo;
                        }
                        else
                        {
                            go[i].GetComponent<SpriteRenderer>().color = Color.white;
                        }

                    }
                    else if (direction_facing == "up")
                    {
                        pushArea.transform.localEulerAngles = new Vector3(0, 0, 0);
                        if (angle <= 75)
                        {
                            go[i].GetComponent<SpriteRenderer>().color = changeTo;
                        }
                        else
                        {
                            go[i].GetComponent<SpriteRenderer>().color = Color.white;
                        }
                    }

                    else if (direction_facing == "up-right")
                    {
                        pushArea.transform.localEulerAngles = new Vector3(0, 0, -45);
                        if (angle >= 22.5 && angle <= 67.5 && facing.x > 0)
                        {
                            go[i].GetComponent<SpriteRenderer>().color = changeTo;
                        }
                        else
                        {
                            go[i].GetComponent<SpriteRenderer>().color = Color.white;
                        }
                    }

                    else if (direction_facing == "down-right")
                    {
                        pushArea.transform.localEulerAngles = new Vector3(0, 0, -135);
                        if (angle >= 112.5 && angle <= 135.5 && facing.x > 0)
                        {
                            go[i].GetComponent<SpriteRenderer>().color = changeTo;
                        }
                        else
                        {
                            go[i].GetComponent<SpriteRenderer>().color = Color.white;
                        }
                    }

                    else if (direction_facing == "down-left")
                    {
                        pushArea.transform.localEulerAngles = new Vector3(0, 0, 135);
                        if (angle >= 112.5 && angle <= 135.5)
                        {
                            go[i].GetComponent<SpriteRenderer>().color = changeTo;
                        }
                        else
                        {
                            go[i].GetComponent<SpriteRenderer>().color = Color.white;
                        }
                    }

                    else if (direction_facing == "up-left")
                    {
                        pushArea.transform.localEulerAngles = new Vector3(0, 0, 45);
                        if (angle >= 22.5 && angle <= 67.5)
                        {
                            go[i].GetComponent<SpriteRenderer>().color = changeTo;
                        }

                        else
                        {
                            go[i].GetComponent<SpriteRenderer>().color = Color.white;
                        }
                    }

                    else
                    {
                        go[i].GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }
        }
    }

    void check_drag()
    {
        // This loop increases drag of enemies, so that they slow down and not fly off with a constant velocity. 
        for (int i = 0; i < go.Count; i++)
        {
            if (go[i] != null && go[i].gameObject.tag != "PurpBullet")
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

    void applyForceToEnemies(int porp)
    {
        GameObject[] purple_bullets = GameObject.FindGameObjectsWithTag("PurpBullet");
        foreach (GameObject bullet in purple_bullets)
        {
            go.Add(bullet);
        }

        for (int i = 0; i < go.Count; i++)
        {
            // Checking if the enemy has been destroyed. Don't access if true.
            if (go[i] != null)
            {
                enemybody = go[i].GetComponent<Rigidbody2D>();
                Transform target = go[i].transform;

                // Refer: http://answers.unity3d.com/questions/1167656/choosing-speed-and-direction-of-addforce.html#answer-1167720
                Vector2 dir = transform.position - target.position;  // Direction between character and enemy.
                float distance = dir.magnitude; // Distance between two in float form, instead of Vector.
                float force_size = 10.0f;
                dir.Normalize();    // Makes the distance have a magnitude of 1.

                float distance_check = Vector2.Distance(transform.position, target.position);

                string nameCheck = go[i].gameObject.name;

                if (distance_check < allowed_radius)
                {

                    if (porp == 1)
                    {
                        // Inverse linear force equation.
                        if (go[i].gameObject.name == "FlyingMonster")
                        {
                            go[i].gameObject.GetComponent<FlyingEnemy>().appliedForce = true;
                        }
                        enemybody.AddForce(dir * (force_size / distance) * 70);
                    }

                    else {
                        // Variables we're going to need to make the calculations for the push						
                        Vector3 angle_dir = target.position - transform.position;
                        Vector3 facing = transform.InverseTransformPoint(target.position);
                        float angle = Vector3.Angle(transform.up, angle_dir.normalized);

						int speed = 80;

                        // Find me if you need me to explain these particular mechanics, they're not that tough. 
                        if (direction_facing == "right" && facing.x > 0)
                        {
                            if (angle >= 45 && angle <= 135)
                            {
								enemybody.AddForce(-dir * (force_size / distance) * speed);

                                if (nameCheck == "FlyingMonster")
                                    go[i].gameObject.GetComponent<FlyingEnemy>().appliedForce = true;
                            }

                        }
                        else if (direction_facing == "down")
                        {
                            if (angle >= 65)
                            {   // used to be 135


								enemybody.AddForce(-dir * (force_size / distance) * speed);

                                if (nameCheck == "FlyingMonster")
                                    go[i].gameObject.GetComponent<FlyingEnemy>().appliedForce = true;

                            }
                        }

                        else if (direction_facing == "left")
                        {
                            if (angle >= 45 && angle <= 135 && facing.x < 0)
                            {

								enemybody.AddForce(-dir * (force_size / distance) * speed);

                                if (nameCheck == "FlyingMonster")
                                    go[i].gameObject.GetComponent<FlyingEnemy>().appliedForce = true;


                            }

                        }
                        else if (direction_facing == "up")
                        {
                            if (angle <= 75)
                            {

								enemybody.AddForce(-dir * (force_size / distance) * speed);

                                if (nameCheck == "FlyingMonster")
                                    go[i].gameObject.GetComponent<FlyingEnemy>().appliedForce = true;


                            }

                        }

                        else if (direction_facing == "up-right" && facing.x > 0)
                        {
                            if (angle >= 22.5 && angle <= 67.5)
                            {
								enemybody.AddForce(-dir * (force_size / distance) * speed);


                                if (nameCheck == "FlyingMonster")
                                    go[i].gameObject.GetComponent<FlyingEnemy>().appliedForce = true;


                            }
                        }

                        else if (direction_facing == "down-right" && facing.x > 0)
                        {

                            if (angle >= 112.5 && angle <= 135.5)
                            {
								enemybody.AddForce(-dir * (force_size / distance) * speed);

                                if (nameCheck == "FlyingMonster")
                                    go[i].gameObject.GetComponent<FlyingEnemy>().appliedForce = true;


                            }
                        }

                        else if (direction_facing == "down-left")
                        {
                            if (angle >= 112.5 && angle <= 135.5)
                            {
								enemybody.AddForce(-dir * (force_size / distance) * speed);

                                if (nameCheck == "FlyingMonster")
                                    go[i].gameObject.GetComponent<FlyingEnemy>().appliedForce = true;


                            }
                        }

                        else if (direction_facing == "up-left")
                        {
                            if (angle >= 22.5 && angle <= 67.5)
                            {
								enemybody.AddForce(-dir * (force_size / distance) * speed);


                                if (nameCheck == "FlyingMonster")
                                    go[i].gameObject.GetComponent<FlyingEnemy>().appliedForce = true;


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

            print("added force to bullet");
        }

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
            interrupt_animation = false;
        }
        // Push animation
        else
        {
            Vector3 change = Vector3.zero;
            float time = 0.0f;
            //push_wave.GetComponent<SpriteRenderer>().enabled = true;

            magnet_wave.transform.localScale = new Vector3(3, 3, 0);

            if (direction_facing == "right")
            {
                change = new Vector3(1, 0, 0);
                push_wave.transform.Rotate(0, 0, -90);
            }
                
            else if (direction_facing == "down")
            {
                change = new Vector3(0, -1, 0);
                push_wave.transform.Rotate(0, 0, 180);
            }  
            else if (direction_facing == "left")
            {
                change = new Vector3(-1, 0, 0);
                push_wave.transform.Rotate(0, 0, 90);
            }
             
            else if (direction_facing == "up")
            {
                change = new Vector3(0, 1, 0);
                push_wave.transform.Rotate(0, 0, 0);
            }
           
            else if (direction_facing == "up-right")
            {
                change = new Vector3(1, 1, 0);
                push_wave.transform.Rotate(0, 0, -45);
            }
                
            else if (direction_facing == "down-right")
            {
                change = new Vector3(1, -1, 0);
                push_wave.transform.Rotate(0, 0, -135);
            }
               
            else if (direction_facing == "down-left")
            {
                change = new Vector3(-1, -1, 0);
                push_wave.transform.Rotate(0, 0, 135);
            }
                
            else if (direction_facing == "up-left")
            {
                change = new Vector3(-1, 1  , 0);
                push_wave.transform.Rotate(0, 0, 45);
            }

            // Starting small, then increasing in size, simulating "pushing".
            while (magnet_wave.GetComponent<SpriteRenderer>().color.a > 0)
            {
                decreaseOpacity();
                time += Time.deltaTime;

                magnet_wave.transform.position += change * Time.deltaTime * 10;
                magnet_wave.transform.localScale += Vector3.one * Time.deltaTime * 3;
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
            //StartCoroutine ("invunerable", 1f);
        }
    }
}
