using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float maxSpeed = 20f;
	public float acceleration = 0.2f;
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
			Decelerate (2);
		}

		bool thrusting = (vertAxis > 0);

		transform.Rotate (new Vector3 (0, 0, turnAxis));

		mVars.faceDir = transform.up;
		mVars.faceAngle = transform.eulerAngles.z;

		if (mVars.movementDir == Vector3.zero) {
			mVars.movementDir = transform.up * currentSpeed * Time.deltaTime;
			mVars.oldMovementAngle = transform.eulerAngles.z;
		} else {
			if (thrusting) {
				if (mVars.oldMovementAngle != mVars.faceAngle) {
					if (currentSpeed > 1f) Decelerate ();
					if (currentSpeed < 1f) {
						mVars.movementDir = transform.up * currentSpeed * Time.deltaTime;
						mVars.oldMovementAngle = transform.eulerAngles.z;
					}
				} else {
					mVars.movementDir = transform.up * currentSpeed * Time.deltaTime;
					mVars.oldMovementAngle = transform.eulerAngles.z;
				}

			} else if (turnAxis != 0) {
				Decelerate ();
			}
		}

		transform.position += (mVars.movementDir);
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
		public Vector3 movementDir;
		public float oldMovementAngle;
		public Vector3 faceDir;
		public float faceAngle;

		public void Reset() {
			movementDir = Vector3.zero;
			faceDir = Vector3.zero;
			oldMovementAngle = 0;
			faceAngle = 0;
		}
	}
}
