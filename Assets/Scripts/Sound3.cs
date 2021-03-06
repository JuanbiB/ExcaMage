﻿using UnityEngine;
using System.Collections;

public class Sound3 : MonoBehaviour {

	AudioSource source;
	AudioClip MainTheme;
	AudioClip fx1;
	AudioClip fx2;
	AudioClip MenuTheme;

	Character player;

	public bool balls;

	void Start () {
		source = gameObject.AddComponent<AudioSource> ();

		MainTheme = Resources.Load ("Sound/boss fight rough") as AudioClip;
		MenuTheme = Resources.Load ("Sound/open menu") as AudioClip;
		fx1 = Resources.Load ("Sound/fx") as AudioClip;
		fx2 = Resources.Load ("Sound/fx 1") as AudioClip;
		balls = false;
		player = GameObject.FindWithTag ("Player").GetComponent<Character> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (balls) {
			if (source.isPlaying == false)
				source.PlayOneShot (MainTheme, .8f);
		}

        if (player.health <= 0)
        {
            source.Stop();
        }
	}
}
