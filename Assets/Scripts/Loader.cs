using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Loader : MonoBehaviour {

    void Start()
    {
        
    }

	public void load_scene(int scene){
        if (SceneManager.GetActiveScene().name == "Start Menu")
        {
            GameObject.Find("Canvas/background/LogoText").GetComponent<Text>().text = "Loading...";
        }
        SceneManager.LoadScene (scene);
	}

}
