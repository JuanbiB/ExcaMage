using UnityEngine;
using System.Collections;

public class MultiKill : MonoBehaviour {

    float clock;
    float start_time;
    SpriteRenderer sprite;
	GameObject player;

	// Use this for initialization
	void Start () {
        clock = 0.0f;
        start_time = Time.time;
        sprite = GetComponent<SpriteRenderer>();
		player = GameObject.FindWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = (Vector2) player.transform.position + new Vector2 (0, 3);

        clock += Time.deltaTime;

        Vector3 target_size = new Vector3(3, 3, 0);

        float distCovered = (Time.time - start_time) * 5f;
        float journeyLength = Vector3.Distance(Vector3.zero, target_size);
        float fracJourney = distCovered / journeyLength;

        transform.localScale = Vector3.Lerp(Vector3.zero, target_size, fracJourney);

        if (clock > 2f)
        {
            Color temp = sprite.color;
            temp.a -= 0.04f;
            sprite.color = temp;
        }

        
        if (sprite.color.a < 0)
        {
            Destroy(gameObject);
        }

        if (transform.localScale.x == 3)
        {
            transform.eulerAngles = Vector3.zero;
            return;
        }
            
        transform.eulerAngles = new Vector3(0, 0, 360 * clock * 4.5f);
    }

}
