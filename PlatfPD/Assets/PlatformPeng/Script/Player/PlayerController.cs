using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	[Header("Set up player value")]
	[Tooltip("speed of player")]
	public float speed = 0.01f;
	[Tooltip("Jump force of player")]
	public float jumpForce = 391f;
	[Tooltip("Gravity of player when is jumping in the air")]
	public float gravityJump = 1.2f;
	[Tooltip("Gravity of player when sliding")]
	public float gravitySlide = 5f;
	[Tooltip("The force of bullet when player throw it")]
	public float throwForce = 300f;
	[Header("JetPack")]
	public GameObject JetPack;
	[Tooltip("Force of jetpack when user hold the Jump button")]
	public float jetPackForce = 50f;
	[Tooltip("The fire fx of Jet Pack")]
	public ParticleSystem JetPackFire;
	[Tooltip("The position of bullet")]
	public Transform throwPoint;
	[Tooltip("Smoke position when player jump, slide")]
	public Transform smokePoint;
	[Tooltip("Place Bullet prefab here")]
	public GameObject Bullet;
	[Tooltip("Place Smoke fx prefab here")]
	public GameObject smokeFx;
	[Tooltip("Place Jump fx prefab here")]
	public GameObject jumpFx;
	[Tooltip("Place the magnet from player here, this object will be set on and off during game")]
	public GameObject Magnet;
	[Tooltip("Time allow the magnet work")]
	public float magnetTimer = 10f;


	[Tooltip("The Box Collider of upper body, this will be disabled when player sliding")]
	public BoxCollider2D boxColl1;
	public BoxCollider2D boxColl2;
	[Tooltip("Check ground point, this must be under player feet ")]
	public Transform checkGround;
	[Tooltip("The layers that are considered is the ground")]
	public LayerMask LayerGround;
	[Header("Animator Controller")]
	[Tooltip("Place the paremeters of Animator in here, useful to custom player")]
	public string walkTrigger = "Walk";
	public string isGroundBool = "isGround";
	public string slideBool = "Slide";
	public string thrownTrigger = "Thrown";
	public string dieTrigger = "Die";

	//private 
	private Animator anim;
	private Rigidbody2D rig;
	private bool play = false;
	private bool die = false;
	private bool isGrounded = true;
	[HideInInspector]
	public bool isUsingJetPack = false;
	private bool isJumpHold = false;
	private float gravityNormal;
	private bool isCannonFiring = false;
	private bool isBoost = false;
	private float timeStuck = 0.1f;

	void Awake(){
		Magnet.SetActive (false);		
	}

	
	void Start () {
		
		rig = GetComponent<Rigidbody2D> ();
		gravityNormal = rig.gravityScale;		
		anim = GetComponent<Animator> ();
		JetPack.SetActive (false);		

		if(GlobalValue.isUsingJetpack){
			isUsingJetPack = true;

			rig.velocity = Vector2.zero;
			JetPack.SetActive (true);
		}
	}
	
	
	void Update () {
		
		if (!die) {		
			if (Input.GetKeyDown (KeyCode.UpArrow)) {		
				Jump();
			}
			if (Input.GetKeyUp (KeyCode.UpArrow)) {
				JumpOff ();
			}
			if (Input.GetKeyDown (KeyCode.Space)) {
				Attack ();
			}
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				Slide (true);
			} 
			if (Input.GetKeyUp (KeyCode.DownArrow)) {
				Slide (false);
			}

			anim.SetFloat("Height", rig.velocity.y);
		}
	}

	void FixedUpdate(){
		if (!die) {		
			if (play && !isCannonFiring) {		
				transform.Translate (new Vector3 (speed, 0, 0));		
			}
			if (isUsingJetPack && isJumpHold) {		
				rig.AddForce (new Vector2 (0, jetPackForce));
			}

			
			if (Physics2D.OverlapCircle (checkGround.transform.position, 0.2f, LayerGround)) {
				anim.SetBool (isGroundBool, true);		
				isGrounded = true;
				isCannonFiring = false;	
			} else {
				anim.SetBool (isGroundBool, false);		
				isGrounded = false;
			}

			if (rig.velocity.y == 0 && !isGrounded && !isUsingJetPack) {
				timeStuck -= Time.fixedDeltaTime;
				if (timeStuck <= 0)
					GameManager.instance.GameOver ();
			} else
				timeStuck = 0.1f;

				
		}
	}

	
	public void CannonFire(){
		isCannonFiring = true;
		anim.SetTrigger (walkTrigger);
	}

	
	public void Play(){
		if (anim != null)
			anim.SetTrigger(walkTrigger);
		play = true;
	}

	
	public void Jump(){
		if (!die) {		
			isJumpHold = true;		
			if (isUsingJetPack) {	
				JetPackFire.emissionRate = 100f;			
				rig.gravityScale = gravityNormal;

			}
			else if (isGrounded) {
				rig.gravityScale = gravityJump;
				rig.velocity = Vector2.zero;
				rig.AddForce (new Vector2 (0, jumpForce));
				Instantiate (jumpFx, smokePoint.position, Quaternion.identity);
			}
		}
	}

	
	public void JumpOff(){
		isJumpHold = false;
		if (isUsingJetPack) {
			JetPackFire.emissionRate = 25f;		
		} else
			rig.gravityScale = gravityNormal;
	}

	
	public void Slide(bool slide){
		if (!die) {
			anim.SetBool (slideBool, slide);
			if (slide) {
				boxColl1.enabled = false;		
				boxColl2.enabled = false;
				StartCoroutine (CreateSmoke (0.1f));	
				rig.gravityScale = gravitySlide;		
			} else {
				boxColl1.enabled = true;		
				boxColl2.enabled = true;
				StopAllCoroutines ();
				if(!isJumpHold)
					rig.gravityScale = gravityNormal;
			}
		}
	}

	IEnumerator CreateSmoke(float time){
		yield return new WaitForSeconds (time);
		if (isGrounded)	
			Instantiate (smokeFx, smokePoint.transform.position, Quaternion.identity);
		StartCoroutine (CreateSmoke (0.1f));	
	}
	
	public void Attack(){
		if (!die && !isCannonFiring) {
			if (GameManager.Bullets > 0) {		
				GameManager.Bullets--;
				anim.SetTrigger (thrownTrigger);		
				GameObject obj = Instantiate (Bullet, throwPoint.position, Quaternion.AngleAxis (30, Vector3.forward)) as GameObject;
				obj.GetComponent<Rigidbody2D> ().AddRelativeForce (new Vector2 (throwForce, 0));
			}
		}
	}

	
	public void Dead(){
		if (!die) {

			die = true;
			anim.SetTrigger (dieTrigger);
			JetPack.SetActive (false);		
			StopAllCoroutines ();
			
			rig.velocity = Vector2.zero;
			rig.gravityScale = 0.5f;

			var boxCo = GetComponents<BoxCollider2D> ();
			foreach (var box in boxCo) {
				box.enabled = false;
			}
			var CirCo = GetComponents<CircleCollider2D> ();
			foreach (var cir in CirCo) {
				cir.enabled = false;
			}

		}
	}

	
	void OnTriggerEnter2D(Collider2D other){
		
		if (other.gameObject.CompareTag ("Fruit")) {
			other.GetComponent<CircleCollider2D> ().enabled = false;
			GameManager.Hearts++;
			other.gameObject.GetComponent<Animator> ().SetTrigger ("Collected");
		}
		else if (other.gameObject.CompareTag ("Bullet")) {
			GameManager.Bullets += 10;
			Destroy (other.gameObject);
		}
		else if (other.gameObject.CompareTag ("Magnet")) {
			Magnet.SetActive (true);
			StartCoroutine (WaitAndDisableMagnet (magnetTimer));
			Destroy (other.gameObject);
		}
		else if (other.gameObject.CompareTag ("Star")) {
			other.GetComponent<CircleCollider2D> ().enabled = false;
			GameManager.Stars++;
			GameManager.Score += 10;
			other.gameObject.GetComponent<Animator> ().SetTrigger ("Collected");
		}

		else if (other.gameObject.CompareTag ("JetPack")) {
			isUsingJetPack = !isUsingJetPack;
			rig.velocity = Vector2.zero;
			JetPack.SetActive (isUsingJetPack);
			Destroy (other.gameObject);
		}
		else if (other.gameObject.CompareTag ("SpeedBoost")) {
			if (!isBoost) {
				isBoost = true;
				speed *= 1.45f;
			} else {
				isBoost = false;
				speed /= 1.45f;
			}

			Destroy (other.gameObject);
		}
	}

	
	IEnumerator WaitAndDisableMagnet(float time){
		yield return new WaitForSeconds (time);
		Magnet.SetActive (false);
	}

	
	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.CompareTag ("Enemy")) {
			GameManager.instance.GameOver ();
		}
	}
}
