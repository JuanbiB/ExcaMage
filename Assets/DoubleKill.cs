using UnityEngine;
using System.Collections;

public class DoubleKill : MonoBehaviour
{

    float clock;
    float start_time;
    SpriteRenderer sprite;
    GameObject player;


    void Awake()
    {
        GameObject[] double_kills = GameObject.FindGameObjectsWithTag("DoubleKills");

        foreach (GameObject db in double_kills)
        {
            if (db != null)
            {
                db.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        clock = 0.0f;
        start_time = Time.time;
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");
        name = "Double Kill";

        if (player.GetComponent<Character>().health < player.GetComponent<Character>().maxhealth)
            player.GetComponent<Character>().health++;

        if (player.GetComponent<Character>().running_life_message == true)
        {
            // do nothing;
        }
        else
        {
            StartCoroutine(player.GetComponent<Character>().life_message(1));
        }


    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (Vector2)player.transform.position + new Vector2(0, 3);

        clock += Time.deltaTime;

        transform.position = new Vector3(transform.position.x, transform.position.y, -9);

        Vector2 target_size = new Vector2(2, 2);

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
            Destroy(gameObject);
        }

        if (transform.localScale.x == 2)
        {
            transform.eulerAngles = Vector3.zero;
            return;
        }

        transform.eulerAngles = new Vector3(0, 0, 360 * clock * 4.5f);
    }

}
