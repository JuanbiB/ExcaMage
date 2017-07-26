using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

	[SerializeField] private float fillAmount;
	[SerializeField] private float lerpspeed;
	[SerializeField] private Image content;
	[SerializeField] private Text AmmoCount;
	[SerializeField] private int numhealth;

	GameObject player;
	GameObject boss;

	private bool inAnim;
	bool hasLoaded;

	// Use this for initialization
	void Start () {
		content = GameObject.Find ("bossHealth").gameObject.GetComponent<Image>();
		content.fillAmount = 0;
		fillAmount = 1;
		lerpspeed = 3;

		hasLoaded = false;
	}
	// Update is called once per frame
	void Update () {
		if (boss == null) {
			boss = GameObject.FindGameObjectWithTag ("boss");
			player = GameObject.FindGameObjectWithTag ("Player");
			numhealth = boss.GetComponent<HandgunCat> ().currHealth;
			GameObject.FindWithTag ("goof").GetComponent<Text>().text = "Ammo :" + player.GetComponent<Character> ().ammo;

		}
		if (hasLoaded == false) { //if it hasn't loaded yet
			if (content.fillAmount > fillAmount -0.01f) {
				hasLoaded = true;
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
	}
}
