using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {







	// Use this for initialization

	
	// Update is called once per frame
	void FixedUpdate () {

//		debugCurrentVelocity = mVars.moveAmount;
		


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






}
