using UnityEngine;
using System.Collections;

public class DetectMonsterFalling : MonoBehaviour {
	public Rigidbody2D monsterIV;
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("Player")) {
			monsterIV.isKinematic = false;
			Destroy (gameObject);
		}
	}
}
