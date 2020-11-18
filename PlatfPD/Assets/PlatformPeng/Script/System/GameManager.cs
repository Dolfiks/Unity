using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	[Header("Setup Level")]
	[Tooltip("Is this final level of the World? the next World will be unlock")]
	public bool isFinishWorld;

	public int bullets = 50;		//пули игрока
	public int star1 = 100;			//счет  для 1 звезды
	public int star2 = 500;			//2х звезд
	public int star3 = 1000;		//3х звезд
	[Header("")]
		

	public static GameManager instance;		

	public enum GameState
	{
		Menu, Playing, Pause
	}
	[HideInInspector]
	public GameState state;

	private int score = 0;
	private int stars = 0;
	private int heart = 5;

	
	public static int Score{
		get{ return instance.score; }
		set{ instance.score = value; }
	}

	
	public static int Stars{
		get{ return instance.stars; }
		set{ instance.stars = value; }
	}

	
	public static int Hearts{
		get{ return instance.heart; }
		set{ instance.heart = Mathf.Clamp(value,0,7); }
	}

	
	public static int Bullets{
		get{ return instance.bullets; }
		set{ instance.bullets = value; }
	}

	
	public static int Best {
		get{ return PlayerPrefs.GetInt ("World" + GlobalValue.worldPlaying + GlobalValue.levelPlaying+"best", 0); }
		set{ PlayerPrefs.SetInt ("World" + GlobalValue.worldPlaying + GlobalValue.levelPlaying+"best", value); }
	}

	
	public static int BestStars {
		get{ return PlayerPrefs.GetInt ("World" + GlobalValue.worldPlaying + GlobalValue.levelPlaying+"stars", 0); }
		set{ PlayerPrefs.SetInt ("World" + GlobalValue.worldPlaying + GlobalValue.levelPlaying+"stars", value); }
	}

	
	public static int HighestLevel {
		get{ return PlayerPrefs.GetInt ("World" + GlobalValue.worldPlaying + "HighestLevel", 1); }
		set{ PlayerPrefs.SetInt ("World" + GlobalValue.worldPlaying + "HighestLevel", value); }
	}

	
	public static GameState CurrentState{
		get{ return instance.state; }
		set{ instance.state = value; }
	}
		
	private PlayerController player;

	void Awake(){
		instance = this;
	}

	
	void Start () {
		state = GameState.Menu;
		player = FindObjectOfType<PlayerController> ();

	}

	
	void Update () {
		
		if (Input.anyKeyDown && state != GameState.Playing)		
			Play ();
	}

	
	public void Play(){
		state = GameState.Playing;		
		player.Play ();		
	}

	
	public void GameSuccess(){


		state = GameState.Menu;		
		if (GlobalValue.levelPlaying >= HighestLevel)
			HighestLevel = GlobalValue.levelPlaying + 1;	

		
		if (score > Best) {
			Best = score;
		
		
			if (score >= star3 && BestStars < 3)
				BestStars = 3;
			else if (score >= star2 && BestStars < 2)
				BestStars = 2;
			else if (score >= star1 && BestStars < 1)
				BestStars = 1;
		}
		MenuManager.instance.ShowLevelComplete ();		
			

		
	}

	
	public void GameOver(){


		if (state == GameState.Playing) {	
			state = GameState.Menu;		
			StartCoroutine(Restartcheckpoint(2));	
			player.Dead ();		


		}
	}

	IEnumerator Restartcheckpoint(float time){

		yield return new WaitForSeconds (time);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}
}
