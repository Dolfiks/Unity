using UnityEngine;
using System.Collections;

public class DestroyFalling : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D other){
		
		if (other.gameObject.CompareTag ("Player")) {
			Debug.Log ("GAMEOVER");
			GameManager.instance.GameOver ();
			other.gameObject.SetActive (false);
		} else
			Destroy (other.gameObject);
	}
}
