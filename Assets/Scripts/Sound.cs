using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {

	AudioSource source;
	AudioClip MainTheme;
	AudioClip fx1;
	AudioClip fx2;
	AudioClip MenuTheme;

	void Start () {
		source = gameObject.AddComponent<AudioSource> ();

		MainTheme = Resources.Load ("Sound/main theme") as AudioClip;
		MenuTheme = Resources.Load ("Sound/open menu") as AudioClip;
		fx1 = Resources.Load ("Sound/fx") as AudioClip;
		fx2 = Resources.Load ("Sound/fx 1") as AudioClip;


		source.PlayOneShot (MainTheme, .5f);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
