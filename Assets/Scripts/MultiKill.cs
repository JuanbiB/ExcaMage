using UnityEngine;
using System.Collections;

public class MultiKill : MonoBehaviour {

    float clock;
    float start_time;
    SpriteRenderer sprite;
	GameObject player;
    Character player_script;

    AudioSource source;
    AudioClip sound;

	// Use this for initialization
	void Start () {
        clock = 0.0f;
        start_time = Time.time;
        sprite = GetComponent<SpriteRenderer>();
		player = GameObject.FindWithTag ("Player");
        player_script = player.GetComponent<Character>();

        source = gameObject.AddComponent<AudioSource>();
        sound = Resources.Load("Sound/multi-kill") as AudioClip;

        source.PlayOneShot(sound, 1f);

        GameObject[] double_kills = GameObject.FindGameObjectsWithTag("DoubleKills");

        foreach (GameObject db in double_kills)
        {
            if (db != null)
            {
                db.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            if (player_script.health < player_script.maxhealth)
            {
                player_script.health++;
            }
        }
     

        if (player.GetComponent<Character>().running_life_message == true)
        {
            // do nothing;
        }
        else
        {
            StartCoroutine(player.GetComponent<Character>().life_message(2));
        }
      

    }
	
	// Update is called once per frame
	void Update () {
		transform.position = (Vector2) player.transform.position + new Vector2 (0, 3);

        clock += Time.deltaTime;

        transform.position = new Vector3(transform.position.x, transform.position.y, -9);

        Vector2 target_size = new Vector2(3, 3);

        float distCovered = (Time.time - start_time) * 5f;
        float journeyLength = Vector2.Distance(Vector2.zero, target_size);
        float fracJourney = distCovered / journeyLength;

        transform.localScale = Vector3.Lerp(Vector3.zero, target_size, fracJourney);

        if (clock > 3f)
        {
            Color temp = sprite.color;
            temp.a -= 0.04f;
            sprite.color = temp;
        }

        
		if (sprite.color.a < 0)
		{
			GetComponent<SpriteRenderer>().enabled = false;
		}

		if (clock > 7f)
			Destroy (gameObject);
		

        if (transform.localScale.x == 3)
        {
            transform.eulerAngles = Vector3.zero;
            return;
        }
            
        transform.eulerAngles = new Vector3(0, 0, 360 * clock * 4.5f);
    }

}
