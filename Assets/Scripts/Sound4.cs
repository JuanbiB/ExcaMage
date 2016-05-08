using UnityEngine;
using System.Collections;

public class Sound4 : MonoBehaviour {

	AudioSource source;
	AudioClip MainTheme;
	AudioClip fx1;
	AudioClip fx2;
	AudioClip MenuTheme;

	Character player;

	void Start () {
		source = gameObject.AddComponent<AudioSource> ();

		MainTheme = Resources.Load ("Sound/you've won") as AudioClip;
	}
	
	// Update is called once per frame
	void Update () {

        if (source.isPlaying == false)
            source.PlayOneShot(MainTheme, .8f);
	}
}
