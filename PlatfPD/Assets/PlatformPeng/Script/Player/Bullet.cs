using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public GameObject hitFx;
	public GameObject missFx;
	
	void Start () {
	
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.CompareTag ("Monster")) {
			Instantiate (hitFx, other.transform.position, Quaternion.identity);
			Destroy(other.gameObject);
		} else {
			Instantiate (missFx, transform.position, Quaternion.identity);
		}
		Destroy (gameObject);
	}
}
