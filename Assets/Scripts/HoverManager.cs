using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HoverManager : MonoBehaviour {
		
	public void EasyButton_Enter(){
		GameObject.Find ("Canvas/background_text").GetComponent<Image> ().enabled = true;
		GameObject.Find ("Canvas/easy text").GetComponent<Text> ().enabled = true;
		GameObject.Find ("Canvas/easy image").GetComponent<Image> ().enabled = true;
	}

	public void EasyButton_Exit(){
		GameObject.Find ("Canvas/background_text").GetComponent<Image> ().enabled = false;
		GameObject.Find ("Canvas/easy text").GetComponent<Text> ().enabled = false;
		GameObject.Find ("Canvas/easy image").GetComponent<Image> ().enabled = false;
	}

	public void MediumButton_Enter(){
		GameObject.Find ("Canvas/background_text").GetComponent<Image> ().enabled = true;
		GameObject.Find ("Canvas/medium text").GetComponent<Text> ().enabled = true;
		GameObject.Find ("Canvas/medium image").GetComponent<Image> ().enabled = true;
	}

	public void MediumButton_Exit(){
		GameObject.Find ("Canvas/background_text").GetComponent<Image> ().enabled = false;
		GameObject.Find ("Canvas/medium text").GetComponent<Text> ().enabled = false;
		GameObject.Find ("Canvas/medium image").GetComponent<Image> ().enabled = false;
	}

	public void HardButton_Enter(){
		GameObject.Find ("Canvas/background_text").GetComponent<Image> ().enabled = true;
		GameObject.Find ("Canvas/hard text").GetComponent<Text> ().enabled = true;
		GameObject.Find ("Canvas/hard image").GetComponent<Image> ().enabled = true;

	}

	public void HardButton_Exit(){
		GameObject.Find ("Canvas/background_text").GetComponent<Image> ().enabled = false;
		GameObject.Find ("Canvas/hard text").GetComponent<Text> ().enabled = false;
		GameObject.Find ("Canvas/hard image").GetComponent<Image> ().enabled = false;
	}
}
