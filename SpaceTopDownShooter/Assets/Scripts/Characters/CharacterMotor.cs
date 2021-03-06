﻿using System.Collections;
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

	public float currentSpeed;

	Rigidbody2D body;

	MovementVars mVars;

	public bool thrusting = false;

	/**
	 * =========================
	 * END MOVEMENT VARIABLES
	 */

	void Awake() {
		mVars.Reset ();
		debugCurrentVelocity = mVars.moveAmount;
		body = GetComponent<Rigidbody2D> ();
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

	public void Move(Transform target, float minDistance, float maxDistance, float speed, ref bool inRange) {
		// Get direction vector between target and this
		Vector2 direction = target.position - this.transform.position;
		// Get magnitude for distance
		float magnitude = direction.magnitude;
		// Normalized direction
		direction.Normalize();

		// Calculate velocity
		Vector2 velocity = direction * speed;

		// Check if we were already in range
		if(inRange) {
			// If we were in range, but now the magnitude is greater than max distance
			if(magnitude >= maxDistance) {
				// We are no longer in range
				inRange = false;

				// Move the player
				this.body.velocity = velocity;
			}
		} else {
			// We were not in range
			if(magnitude <= minDistance) {
				// Set that we are in range now
				inRange = true;

				// Slowly go to halt
				this.body.velocity = Vector2.Lerp(this.body.velocity, Vector2.zero, speed);
			} else {
				// We are not set to be in range right now
				this.body.velocity = velocity;
			}
		}

		// Rotate to look at target
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		this.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);;
	}

	public void Move(float vertInput, float HorizInput) {
		thrusting = false;
		float turnAmount = HorizInput * turnStrength * -1;
//		transform.Rotate (new Vector3 (0, 0, turnAmount));
		body.rotation = body.rotation + turnAmount;


		if (vertInput > 0) {
			if (currentSpeed + acceleration < maxSpeed) {
				currentSpeed += acceleration;
			} else if (currentSpeed > maxSpeed) {
				currentSpeed = maxSpeed;
			}
			thrusting = true;
		} else if (vertInput < 0) {
			Decelerate (mVars.moveAmount);
		}
		mVars.oldMoveAmount = mVars.moveAmount;
		if (thrusting)
			mVars.moveAmount = transform.up * currentSpeed * Time.fixedDeltaTime;
//		body.position += mVars.moveAmount;
		if (mVars.oldMoveAmount != mVars.moveAmount) {
			Decelerate (mVars.oldMoveAmount,4);
			mVars.moveAmount *= 2;
		}

		if (thrusting)
			body.AddForce(mVars.moveAmount,ForceMode2D.Force);

		if (mVars.oldMoveAmount != mVars.moveAmount) {
			mVars.moveAmount /= 2;
		}
	}

	void Decelerate(Vector3 move, float multiplier = 1) {
//		currentSpeed -= acceleration * multiplier;
//		if (currentSpeed < 0)
//			currentSpeed = 0;
		body.AddForce(-mVars.moveAmount * multiplier, ForceMode2D.Force);
	}

	struct MovementVars{
		public Vector3 moveAmount;
		public Vector3 oldMoveAmount;

		public void Reset() {
			moveAmount = Vector3.zero;
			oldMoveAmount = Vector3.zero;
		}
	}
}
