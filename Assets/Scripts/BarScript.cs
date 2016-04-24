using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

	[SerializeField] private float fillAmount;

	[SerializeField] private float lerpspeed = 10;

	[SerializeField]
	private Image content;

	GameObject player;
	[SerializeField] private int numhealth;





	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		numhealth = player.GetComponent<Character> ().health;
		fillAmount = 1;
		lerpspeed = 4;

		
	
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			//print ("CUCK");
			player = GameObject.FindGameObjectWithTag ("Player");
			fillAmount = 1;
			numhealth = player.GetComponent<Character> ().health;

		}
		if (numhealth > player.GetComponent<Character> ().health) {
			print ("YEAH YEAH YEAH");
			fillAmount = (float) player.GetComponent<Character> ().health / player.GetComponent<Character>().maxhealth;
			numhealth--;
		}
		if (fillAmount < content.fillAmount){
			handleBar ();

		}
	
	}

	private void handleBar(){

		if (fillAmount != content.fillAmount) { //If we have a new fill amount then we know that we need to update the bar
			//Lerps the fill amount so that we get a smooth movement
			content.fillAmount = Mathf.Lerp (content.fillAmount, fillAmount, Time.deltaTime * lerpspeed);
		}
		
	}


private float Map (float value, float inMin, float inMax, float outMin, float outMax){
	return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
		//(80 - 0) * (1 - 0 ) / (100 - 0) + 0
		// 80 * 1 / 100 = 0.8
}
}
