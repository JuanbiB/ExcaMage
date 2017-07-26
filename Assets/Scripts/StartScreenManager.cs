using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour {

	public Text emtitle;
	[SerializeField] private Button startbutton = null;
    [SerializeField] private Button practiceButton = null;
    [SerializeField] private Button howToPlayButton = null;
    public AudioSource startsound;
    AsyncOperation async;

	public static StartScreenManager instance = null;
    public Texture2D cursor;

    void Awake()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    // Use this for initialization
    void Start () {
		instance = this;
		emtitle.GetComponent<Image> ();
		startbutton.GetComponent<Button> ();
        practiceButton.GetComponent<Button>();
        howToPlayButton.GetComponent<Button>();

        startbutton.onClick.AddListener (() => gameStart (0));
		howToPlayButton.onClick.AddListener(() => gameStart(2));
        practiceButton.onClick.AddListener(() => gameStart(-1));

        startsound.GetComponent<AudioSource> ();
	
	}

	void gameStart(int level){
		emtitle.gameObject.SetActive (false);
		startbutton.gameObject.SetActive (false);
        practiceButton.gameObject.SetActive(false);
        howToPlayButton.gameObject.SetActive(false);

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
			async = SceneManager.LoadSceneAsync ("difficulty");
		} else if (level == 1) {
			async = SceneManager.LoadSceneAsync ("level2");
		}
		else if (level == 2){
			async = SceneManager.LoadSceneAsync ("How To Play");
		}
        else
        {
            async = SceneManager.LoadSceneAsync("practice range");
        }
	
		while (!async.isDone) {
            emtitle.GetComponent<Text>().text = "Loading...";
            emtitle.gameObject.SetActive(true);
            yield return(0);
		}
	}
}
