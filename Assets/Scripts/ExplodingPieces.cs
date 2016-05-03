using UnityEngine;
using System.Collections;

public class ExplodingPieces : MonoBehaviour {

    public GameObject piece_1;
    public GameObject piece_2;
    public GameObject piece_3;
    public GameObject piece_4;
    public GameObject piece_5;
    public GameObject piece_6;

    void Start () {

        Instantiate(piece_1, transform.position, transform.rotation);
        Instantiate(piece_2, transform.position, transform.rotation);
        Instantiate(piece_3, transform.position, transform.rotation);
        Instantiate(piece_4, transform.position, transform.rotation);
        if (piece_5 != null)
            Instantiate(piece_5, transform.position, transform.rotation);

        if (piece_6 != null)
            Instantiate(piece_6, transform.position, transform.rotation);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
