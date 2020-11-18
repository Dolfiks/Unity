using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu_HomeScreen : MonoBehaviour {
	public static Menu_HomeScreen instance;
	public GameObject MainMenu;
	public GameObject WorldChoose;
	public GameObject World1;
	public GameObject Loading;


	int WorldReached;

	void Awake(){
		instance = this;

		MainMenu.SetActive (true);
		WorldChoose.SetActive (false);
		World1.SetActive (false);
		Loading.SetActive (false);
	}

	void Start(){
		WorldReached = PlayerPrefs.GetInt ("WorldReached", 1);
	}
	
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit ();
	}

	public void Play(){
		MainMenu.SetActive (false);
		WorldChoose.SetActive (true);
	}

	public void Menu(){
		MainMenu.SetActive (true);
		WorldChoose.SetActive (false);
		World1.SetActive (false);
	}

	public void LoadLevel(string nameScene){
		Loading.SetActive (true);
		SceneManager.LoadScene (nameScene);
	}

	public void ShowWorld1(){
		GlobalValue.worldPlaying = 1;
		World1.SetActive (true);
		WorldChoose.SetActive (false);
	}

}
