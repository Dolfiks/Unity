using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {
	public float time = 1;
	void Start () {
		Destroy (gameObject, time);
	}
}
