using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

	[SerializeField] private float fillAmount;

	[SerializeField] private float lerpspeed;

	[SerializeField]
	private Image content;

	[SerializeField] private Text AmmoCount;


	GameObject player;
	GameObject boss;
	[SerializeField] private int numhealth;

	private bool inAnim;

	bool hasLoaded;




	// Use this for initialization
	void Start () {
//		boss = GameObject.FindGameObjectWithTag ("boss");
//		player = GameObject.FindGameObjectWithTag ("Player");
//		numhealth = boss.GetComponent<HandgunCat> ().currHealth;
		content = GameObject.Find ("bossHealth").gameObject.GetComponent<Image>();
		content.fillAmount = 0;
		fillAmount = 1;
		lerpspeed = 3;

		hasLoaded = false;

		//mc_ref = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		//HealthFillAnim ();



		
	
	}

//	void HealthFillAnim(){
//		inAnim = true;
//		for (int i = 0; i < 10; i++) {
//			fillAmount = (float) i;
//			handleBar ();
//		}
//	}




	// Update is called once per frame
	void Update () {
		if (boss == null) {
			//print ("CUCK");
			boss = GameObject.FindGameObjectWithTag ("boss");
			player = GameObject.FindGameObjectWithTag ("Player");
			//fillAmount = 0;
			numhealth = boss.GetComponent<HandgunCat> ().currHealth;

		}
		//if (mc_ref.GetComponent<Cutscene>().fighting=
		if (hasLoaded == false) { //if it hasn't loaded yet
			if (content.fillAmount > fillAmount -0.01f) {
				print ("hasloaded = true");
				hasLoaded = true;
				//boss.GetComponent<HandgunCat> ().currHealth = boss.GetComponent<HandgunCat> ().currHealth - 10;
			} else {
				content.fillAmount = Mathf.Lerp (content.fillAmount, fillAmount, Time.deltaTime * lerpspeed);
			}
		}



		if (AmmoCount != null) {
			AmmoCount.text = "Ammo :" + player.GetComponent<Character> ().ammo;
		}
		if (numhealth > boss.GetComponent<HandgunCat> ().currHealth) {
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
