using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {
	public float speed = 10f;
	private PlayerController player;

	void Awake(){
		enabled = false;
	}

	void Start(){
		player = FindObjectOfType<PlayerController> ();
	}
	
	void Update () {
		transform.position = Vector3.MoveTowards (transform.position, player.transform.position, speed * Time.deltaTime);
	}

	
	public void Destroy(){
		Destroy (gameObject);
	}
}
