using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

	[SerializeField] private float fillAmount;

	[SerializeField]
	private Image content;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		handleBar ();
	
	}

	private void handleBar(){

		content.fillAmount = fillAmount;
		
	}
}
