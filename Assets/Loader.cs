using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Loader : MonoBehaviour {

	public void load_scene(int scene){
		SceneManager.LoadScene (scene);
	}

}
