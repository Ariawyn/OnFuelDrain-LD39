using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	/// <summary>
	/// The max speed.
	/// </summary>
	public float maxSpeed = 20f;

	/// <summary>
	/// The max velocity value for any of the x,y velocity values.
	/// </summary>
	public float maxVelocity = 20f;

	/// <summary>
	/// This value should be very small! Added to speed while
	/// input forward, until it reaches maxSpeed.
	/// </summary>
	public float acceleration = 0.2f;

	/// <summary>
	/// This is multiplied to the horizontal axis to rotate the ship.
	/// </summary>
	public float turnStrength = 2f;

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
	void Update () {
		
		Move ();


	}

	void Move() {
		float turnAxis = this.inputManager.horizontalAxis.GetRawAxisInput() * turnStrength * -1;
		float vertAxis = this.inputManager.verticalAxis.GetRawAxisInput ();

		if (vertAxis > 0) {
			if (currentSpeed + acceleration <= maxSpeed) {
				currentSpeed += acceleration;
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

		if (mVars.velocity == Vector3.zero) {
			mVars.velocity = transform.up * currentSpeed * Time.deltaTime;
			mVars.oldMovementAngle = transform.eulerAngles.z;
		} else {
			if (thrusting) {
//				if (mVars.oldMovementAngle != mVars.faceAngle && currentSpeed > 1f) {
//					if (currentSpeed > 1f) Decelerate ();
////					if (currentSpeed < 1f) {
//						mVars.velocity = transform.up * currentSpeed * Time.deltaTime;
//						mVars.oldMovementAngle = transform.eulerAngles.z;
////					}
//				} else {
//					mVars.velocity = transform.up * currentSpeed * Time.deltaTime;
//					mVars.oldMovementAngle = transform.eulerAngles.z;
//				}

				mVars.velocity += transform.up * currentSpeed * Time.deltaTime;
				mVars.oldMovementAngle = transform.eulerAngles.z;
			}
		}

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

		transform.position += (mVars.velocity);
	}

	void Decelerate(float multiplier = 1) {
		if (currentSpeed - (acceleration * multiplier) >= 0) {
			currentSpeed -= acceleration * multiplier;
		} else if (currentSpeed - acceleration >= 0)
			currentSpeed -= acceleration;
		else {
			currentSpeed = 0;
		}
	}

	void Shoot() {
		
	}

	struct MovementVars{
		public Vector3 velocity;
		public float oldMovementAngle;
		public Vector3 faceDir;
		public float faceAngle;

		public void Reset() {
			velocity = Vector3.zero;
			faceDir = Vector3.zero;
			oldMovementAngle = 0;
			faceAngle = 0;
		}
	}
}
