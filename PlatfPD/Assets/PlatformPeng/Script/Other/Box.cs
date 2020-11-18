using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {
	private float delayTime = 1f;
	private bool delay = false;
	void OnTriggerEnter2D(Collider2D other){
		if(!delay)
		if (other.gameObject.CompareTag ("Player")) {
			StartCoroutine (Delay (delayTime));
			delay = true;
		}
	}

	IEnumerator Delay(float time){
		yield return new WaitForSeconds (time);
		delay = false;
	}
}
