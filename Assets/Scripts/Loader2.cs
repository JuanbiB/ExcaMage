using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Loader2 : MonoBehaviour {

	public void load_scene(int scene){
		SceneManager.LoadScene (scene);
        GameObject.Find("Canvas/Text (8)").GetComponent<Text>().text = "Loading...";
	}

}
