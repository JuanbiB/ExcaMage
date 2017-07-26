using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DifficultySet : MonoBehaviour {

	public void SetEasy(){
		DeactivateUI ();
		PlayerPrefs.SetString ("difficulty", "easy");
		SceneManager.LoadScene("level1");
	}

	public void SetMedium(){
		DeactivateUI ();
		PlayerPrefs.SetString ("difficulty", "medium");
		SceneManager.LoadScene("level1");
	}

	public void SetHard(){
		DeactivateUI ();
		PlayerPrefs.SetString ("difficulty", "hard");
		SceneManager.LoadScene("level1");
	}

	void DeactivateUI(){
		GameObject.Find ("Canvas/Easy Button").SetActive (false);
		GameObject.Find ("Canvas/Hard Button").SetActive (false);
		GameObject.Find ("Canvas/Medium Button ").SetActive (false);

		GameObject.Find ("Canvas/background_text").SetActive (false);
		GameObject.Find ("Canvas/easy text").SetActive (false);
		GameObject.Find ("Canvas/medium text").SetActive (false);
		GameObject.Find ("Canvas/hard text").SetActive (false);

		GameObject.Find ("Canvas/hard image").SetActive (false);
		GameObject.Find ("Canvas/medium image").SetActive (false);
		GameObject.Find ("Canvas/easy image").SetActive (false);

		GameObject.Find ("Canvas/Choose difficulty").GetComponent<Text> ().text = "Loading...";
	}


}
