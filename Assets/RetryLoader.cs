using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RetryLoader : MonoBehaviour {

	public void Retry(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}
}
