using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour {

	public Text emtitle;
	[SerializeField] private Button startbutton = null;
    [SerializeField] private Button practiceButton = null;
	public AudioSource startsound;
    AsyncOperation async;

	public static StartScreenManager instance = null;

	// Use this for initialization
	void Start () {
		instance = this;
		emtitle.GetComponent<Image> ();
		startbutton.GetComponent<Button> ();
        practiceButton.GetComponent<Button>();
        startbutton.onClick.AddListener (() => gameStart (0));
        practiceButton.onClick.AddListener(() => gameStart(-1));

        startsound.GetComponent<AudioSource> ();
	
	}

	void gameStart(int level){
		emtitle.gameObject.SetActive (false);
		startbutton.gameObject.SetActive (false);
        practiceButton.gameObject.SetActive(false);

		startsound.Play ();
		StartCoroutine (LoadGame (level));
	}

	void loadlevel(int level)
	{
		StartCoroutine (LoadGame (level));
	}

	IEnumerator LoadGame(int level) {
        AsyncOperation async;

		if (level == 0) {
			async = SceneManager.LoadSceneAsync ("main");
		} else if (level == 1) {
			async = SceneManager.LoadSceneAsync ("level2");
		}
        else
        {
            async = SceneManager.LoadSceneAsync("practice range");
        }
	
		while (!async.isDone) {
    //		print (async.progress);
            emtitle.GetComponent<Text>().text = "Loading...";
            emtitle.gameObject.SetActive(true);
            yield return(0);
		}
		//yield return new WaitForSeconds (4);
		//SceneManager.LoadScene ("main");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
