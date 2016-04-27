using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

	[SerializeField] private float fillAmount;

	[SerializeField] private float lerpspeed = 10;

	[SerializeField]
	private Image content;

	[SerializeField] private Text AmmoCount;


	GameObject player;
	GameObject boss;
	[SerializeField] private int numhealth;





	// Use this for initialization
	void Start () {
		boss = GameObject.FindGameObjectWithTag ("HandgunCat");
		player = GameObject.FindGameObjectWithTag ("Player");
		numhealth = boss.GetComponent<HandgunCat> ().currHealth;
		fillAmount = 1;
		lerpspeed = 4;

		
	
	}

	// Update is called once per frame
	void Update () {
		if (boss == null) {
			//print ("CUCK");
			boss = GameObject.FindGameObjectWithTag ("HandgunCat");
			player = GameObject.FindGameObjectWithTag ("Player");
			fillAmount = 1;
			numhealth = boss.GetComponent<HandgunCat> ().currHealth;

		}
		AmmoCount.text = "Ammo :" + player.GetComponent<Character> ().ammo;
		if (numhealth > boss.GetComponent<HandgunCat> ().currHealth) {
			print ("YEAH YEAH YEAH");
			fillAmount = (float) boss.GetComponent<HandgunCat> ().currHealth / boss.GetComponent<HandgunCat>().maxHealth;
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
