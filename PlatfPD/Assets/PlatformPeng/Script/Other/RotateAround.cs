using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {
	public float speed = 0.5f;
	
	
	void Update () {
		transform.Rotate (Vector3.forward, speed);
	}
}
