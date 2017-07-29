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
	MovementVars mVars;

	void Awake() {
		inputManager = Object.FindObjectOfType<InputManager> ();
		mVars.Reset ();
	}

	// Use this for initialization
	void Start () {
		currentSpeed = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		debugCurrentVelocity = mVars.velocity;
		
		Move ();

		Shoot ();
	}

	void Move() {
		mVars.oldVelocity = mVars.velocity;
		float turnAxis = this.inputManager.horizontalAxis.GetRawAxisInput() * turnStrength * -1;
		float vertAxis = this.inputManager.verticalAxis.GetRawAxisInput ();

		if (vertAxis > 0) {
			if (currentSpeed + acceleration < maxSpeed) {
				currentSpeed += acceleration;
			} else if (currentSpeed > maxSpeed) {
				currentSpeed = maxSpeed;
			}
		}
		else if (vertAxis !=0) {
//			Debug.Log ("Braking!");
			Decelerate (2); // Something is broken.
		}

		bool thrusting = (vertAxis > 0);

		transform.Rotate (new Vector3 (0, 0, turnAxis));

		mVars.faceDir = transform.up;
		mVars.faceAngle = transform.eulerAngles.z;

		Vector3 newVelocity = mVars.velocity;

		if (mVars.velocity == Vector3.zero) {
			newVelocity = transform.up * currentSpeed * Time.deltaTime;
			mVars.oldMovementAngle = transform.eulerAngles.z;
		} else {
			if (thrusting) {
				newVelocity = transform.up * currentSpeed * Time.deltaTime;
				mVars.oldMovementAngle = transform.eulerAngles.z;
			}
		}

		mVars.velocity.x = Mathf.Lerp (mVars.velocity.x, newVelocity.x, 0.3f);
		mVars.velocity.y = Mathf.Lerp (mVars.velocity.y, newVelocity.y, 0.3f);

		// Clamp the vectors.

		if (mVars.velocity.x > maxVelocity) {
			mVars.velocity.x = maxVelocity;
		}
		else if (mVars.velocity.x < -maxVelocity) {
			mVars.velocity.x = -maxVelocity;
		}
		if (mVars.velocity.y > maxVelocity) {
			mVars.velocity.y = maxVelocity;
		}
		else if (mVars.velocity.y < -maxVelocity) {
			mVars.velocity.y = -maxVelocity;
		}
//
//		if (mVars.oldVelocity != mVars.velocity) {
//			mVars.oldVelContainer += mVars.oldVelocity;
////			Vector3 subtractVelocity = mVars.oldVelocity * 0.1f;
//			transform.position -= mVars.oldVelocity;
//		}
			
		transform.position += (mVars.velocity);
	}

	void Decelerate(float multiplier = 1) {
		mVars.velocity *= 0.99f;
		if (mVars.velocity.x < minVelocity)
			mVars.velocity.x = 0;
		if (mVars.velocity.y < minVelocity)
			mVars.velocity.y = 0;
	}

	void Shoot() {
		if (inputManager.GetKeyDown ("Fire")) {
			GameObject bullet = GameObject.Instantiate (bulletGO);
			bullet.transform.position = transform.position;
			bullet.transform.rotation = transform.rotation;
			bullet.GetComponent<Bullet> ().SetBulletSpeed (bulletSpeed);
		}
	}

	struct MovementVars{
		public Vector3 oldVelocity;
		public Vector3 velocity;
		public float oldMovementAngle;
		public Vector3 faceDir;
		public float faceAngle;

		public void Reset() {
			oldVelocity = Vector3.zero;
			velocity = Vector3.zero;
			faceDir = Vector3.zero;
			oldMovementAngle = 0;
			faceAngle = 0;
		}
	}
}
