using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour {
	public int level = 1;		
	public GameObject Locked;	
	public GameObject Star1;
	public GameObject Star2;
	public GameObject Star3;

	private string levelName;
	private int stars;
	private int highestLevel;

	
	void Start () {
		levelName = gameObject.name;		
		highestLevel=PlayerPrefs.GetInt ("World" + GlobalValue.worldPlaying + "HighestLevel", 1);	
		stars = PlayerPrefs.GetInt ("World" + GlobalValue.worldPlaying + level+"stars", 0);		
		CheckStars ();		

		if (level > highestLevel) {	
			Locked.SetActive (true);		
			GetComponent<Button> ().interactable = false;		
		} else {
			Locked.SetActive (false);			
		}
	}

	
	public void LoadLevel(){
		GlobalValue.levelPlaying = level;		
		Menu_HomeScreen.instance.LoadLevel (levelName);	
	}

	private void CheckStars(){
		switch (stars) {
		case 1:
			Star1.SetActive (true);
			Star2.SetActive (false);
			Star3.SetActive (false);
			break;
		case 2:
			Star1.SetActive (true);
			Star2.SetActive (true);
			Star3.SetActive (false);
			break;
		case 3:
			Star1.SetActive (true);
			Star2.SetActive (true);
			Star3.SetActive (true);
			break;
		default:
			Star1.SetActive (false);
			Star2.SetActive (false);
			Star3.SetActive (false);
			break;
		}
	}
}
