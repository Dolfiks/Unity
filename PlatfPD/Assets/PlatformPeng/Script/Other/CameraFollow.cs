using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	[Tooltip("the X distance of the Player and Camera")]
	public float offsetX = 3f;
	[Tooltip("How smooth the camera follow the player, lower is better")]
	public float smooth = 0.1f;
	[Tooltip("Limited positon up")]
	public float limitedUp = 2f;
	[Tooltip("Limited positon down")]
	public float limitedDown = 0f;
	[Tooltip("Limited positon left")]
	public float limitedLeft = 0f;
	[Tooltip("Limited positon right")]
	public float limitedRight = 100f;

	private Transform player;
	private float playerX;
	private float playerY;
	
	void Start () {
		player = FindObjectOfType<PlayerController> ().transform;		
	}
	
	
	void FixedUpdate () {
		playerX = Mathf.Clamp (player.position.x  + offsetX, limitedLeft, limitedRight);
		playerY= Mathf.Clamp (player.position.y, limitedDown, limitedUp);
		transform.position = Vector3.Lerp (transform.position, new Vector3 (playerX, playerY, transform.position.z), smooth);

	}
}
