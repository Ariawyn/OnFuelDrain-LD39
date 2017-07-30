using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	/// <summary>
	/// The max speed.
	/// </summary>
	public float maxSpeed = 0.05f;

	/// <summary>
	/// The max velocity value for any of the x,y velocity values.
	/// </summary>
	public float maxVelocity = 0.1f;

	public float minVelocity = 0.0004f;

	/// <summary>
	/// This value should be very small! Added to speed while
	/// input forward, until it reaches maxSpeed.
	/// </summary>
	public float acceleration = 0.0005f;

	/// <summary>
	/// This is multiplied to the horizontal axis to rotate the ship.
	/// </summary>
	public float turnStrength = 5f;

	/// <summary>
	/// The bullet gameObject
	/// </summary>
	public GameObject bulletGO;

	/// <summary>
	/// The speed bullets will move at. NOT the fire rate.
	/// </summary>
	public float bulletSpeed = 0.5f;

	public Vector3 debugCurrentVelocity;

	float currentSpeed;
	private InputManager inputManager;
	private AudioManager audioManager;

	// For shooting coroutine
	private float secondsBetweenBulletFire = 0.25f;
	private bool isRunningShootingCoroutine = true;
	MovementVars mVars;

	void Awake() {
		inputManager = Object.FindObjectOfType<InputManager> ();
		audioManager = Object.FindObjectOfType<AudioManager>();
		mVars.Reset ();
	}

	// Use this for initialization
	void Start () {
		currentSpeed = 0;

		// Start running the shoot coroutine, which allows for us to wait a few seconds for when the player can fire after he already did
		StartCoroutine(Shoot());
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		debugCurrentVelocity = mVars.moveAmount;
		
		Move ();

		//Shoot ();
	}

//	void Move() {
//		mVars.oldVelocity = mVars.velocity;
//		float turnAxis = this.inputManager.horizontalAxis.GetRawAxisInput() * turnStrength * -1;
//		float vertAxis = this.inputManager.verticalAxis.GetRawAxisInput ();
//
//		if (vertAxis > 0) {
//			if (currentSpeed + acceleration < maxSpeed) {
//				currentSpeed += acceleration;
//			} else if (currentSpeed > maxSpeed) {
//				currentSpeed = maxSpeed;
//			}
//		}
//		else if (vertAxis !=0) {
////			Debug.Log ("Braking!");
//			Decelerate (2); // Something is broken.
//		}
//
//		bool thrusting = (vertAxis > 0);
//
//		Vector3 newVelocity = mVars.oldVelocity;
//
////		if (turnAxis != 0) {
////			newVelocity = transform.up * currentSpeed * Time.deltaTime;
////		} 
//
//		transform.Rotate (new Vector3 (0, 0, turnAxis));
//
//		mVars.faceDir = transform.up;
//		mVars.faceAngle = transform.eulerAngles.z;
//
//
//		if (mVars.velocity == Vector3.zero || thrusting) {
//
//			newVelocity = transform.up * currentSpeed * Time.deltaTime;
//			mVars.oldMovementAngle = transform.eulerAngles.z;
//
//		}
//
//		Debug.Log ("OldVelocity: " + mVars.oldVelocity.ToString("F5"));
//		Debug.Log ("NewVelocity: " + newVelocity.ToString("F5"));
//		Debug.Log ("Velocity: " + mVars.velocity.ToString("F5"));
//
//		mVars.velocity += newVelocity;
//		mVars.velocity.Normalize ();
//			
//		transform.position += (mVars.velocity);
//	}

	void Move() {
		float turnAxis = this.inputManager.horizontalAxis.GetRawAxisInput() * turnStrength * -1;
		float vertAxis = this.inputManager.verticalAxis.GetRawAxisInput ();

		transform.Rotate (new Vector3 (0, 0, turnAxis));

		Vector3 targetAngle = transform.up.normalized;
		Vector3 currentAngle = mVars.moveAmount.normalized;

		bool thrusting = false;
		if (vertAxis > 0) {
			if (currentSpeed + acceleration < maxSpeed) {
				currentSpeed += acceleration;
			} else if (currentSpeed > maxSpeed) {
				currentSpeed = maxSpeed;
			}
			thrusting = true;
		} else if (vertAxis < 0) {
			Decelerate (2);
		}



		Vector3 moveAmountNew = (thrusting)? targetAngle : Vector3.zero ;

		mVars.moveAmount += moveAmountNew;

		if (currentSpeed == 0 || (Mathf.Abs (mVars.moveAmount.x) < 0.01f && Mathf.Abs (mVars.moveAmount.y) < 0.01f))
			mVars.Reset ();

		transform.position = Vector3.MoveTowards (transform.position, mVars.moveAmount, currentSpeed * Time.fixedDeltaTime);

	}

	void Decelerate(float multiplier = 1) {
		currentSpeed -= acceleration * multiplier;
		if (currentSpeed < 0)
			currentSpeed = 0;
	}

	IEnumerator Shoot() {
		while(isRunningShootingCoroutine) {
			if (inputManager.GetKey ("Fire")) {
				this.audioManager.Play("Fire!");
				GameObject bullet = GameObject.Instantiate (bulletGO);
				bullet.transform.position = transform.position;
				bullet.transform.rotation = transform.rotation;
				bullet.GetComponent<Bullet> ().SetBulletSpeed (bulletSpeed);
				yield return new WaitForSeconds(secondsBetweenBulletFire);
			} else {
				yield return null;
			}
		}
		yield return null;
	}

	struct MovementVars{
		public Vector3 oldVelocity;
		public Vector3 moveAmount;
		public float oldMovementAngle;
		public Vector3 faceDir;
		public float faceAngle;

		public void Reset() {
			oldVelocity = Vector3.zero;
			moveAmount = Vector3.zero;
			faceDir = Vector3.zero;
			oldMovementAngle = 0;
			faceAngle = 0;
		}
	}
}
