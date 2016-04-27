using UnityEngine;
using System.Collections;

public class practiceManager : MonoBehaviour {

    public GameObject xander;
    public GameObject juanbi;

	// Use this for initialization
	void Start () {
        Instantiate(xander, transform.position, transform.rotation);
        Instantiate(juanbi, new Vector3(-10, 7, -3), transform.rotation);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
