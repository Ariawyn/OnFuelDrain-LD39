using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : MonoBehaviour {

	/** 
	 * MOVEMENT VARIABLES
	 * =========================
	 */

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

	public Vector3 debugCurrentVelocity;

	float currentSpeed;

	MovementVars mVars;

	/**
	 * =========================
	 * END MOVEMENT VARIABLES
	 */

	void Awake() {
		mVars.Reset ();
		debugCurrentVelocity = mVars.moveAmount;
	}

	// Use this for initialization
	void Start () {
		currentSpeed = 0;
	}

	// TODO: This is the luftrausers attempt. DNE
//	void Move() {
//		float turnAxis = this.inputManager.horizontalAxis.GetRawAxisInput() * turnStrength * -1;
//		float vertAxis = this.inputManager.verticalAxis.GetRawAxisInput ();
//
//		transform.Rotate (new Vector3 (0, 0, turnAxis));
//
//		Vector3 targetAngle = transform.up.normalized;
//		Vector3 currentAngle = mVars.moveAmount.normalized;
//
//		bool thrusting = false;
//		if (vertAxis > 0) {
//			if (currentSpeed + acceleration < maxSpeed) {
//				currentSpeed += acceleration;
//			} else if (currentSpeed > maxSpeed) {
//				currentSpeed = maxSpeed;
//			}
//			thrusting = true;
//		} else if (vertAxis < 0) {
//			Decelerate (2);
//		}
//
//
//
//		Vector3 moveAmountNew = (thrusting)? targetAngle : Vector3.zero ;
//
//		mVars.moveAmount += moveAmountNew;
//
//		if (currentSpeed == 0 || (Mathf.Abs (mVars.moveAmount.x) < 0.01f && Mathf.Abs (mVars.moveAmount.y) < 0.01f))
//			mVars.Reset ();
//
//		transform.position = Vector3.MoveTowards (transform.position, mVars.moveAmount, currentSpeed * Time.fixedDeltaTime);
//
//	}

	public void Move(float vertInput, float HorizInput) {
		float turnAmount = HorizInput * turnStrength * -1;
		transform.Rotate (new Vector3 (0, 0, turnAmount));

		if (vertInput > 0) {
			if (currentSpeed + acceleration < maxSpeed) {
				currentSpeed += acceleration;
			} else if (currentSpeed > maxSpeed) {
				currentSpeed = maxSpeed;
			}
		} else if (vertInput < 0) {
			Decelerate ();
		}

		mVars.moveAmount = transform.up * currentSpeed * Time.fixedDeltaTime;
		transform.position += mVars.moveAmount;
	}

	void Decelerate(float multiplier = 1) {
		currentSpeed -= acceleration * multiplier;
		if (currentSpeed < 0)
			currentSpeed = 0;
	}

	struct MovementVars{
		public Vector3 moveAmount;

		public void Reset() {
			moveAmount = Vector3.zero;
		}
	}
}
