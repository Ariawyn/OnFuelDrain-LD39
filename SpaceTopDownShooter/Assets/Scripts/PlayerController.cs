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
		

		float turnAxis = this.inputManager.horizontalAxis.GetRawAxisInput() * turnStrength * -1;
		float vertAxis = this.inputManager.verticalAxis.GetRawAxisInput ();

		transform.Rotate (new Vector3 (0, 0, turnAxis));

		bool thrusting = false;

		if (vertAxis > 0) {
			if (currentSpeed + acceleration <= maxSpeed) {
				currentSpeed += acceleration;
			}
			thrusting = true;
		}
		if (vertAxis < 0) {
			if (currentSpeed - acceleration >= 0) {
				currentSpeed -= acceleration;
			}
		}

		mVars.faceDir = transform.up;

		if (mVars.movementDir == Vector3.zero) {
			mVars.movementDir = transform.up * currentSpeed * Time.deltaTime;
		} else {
			if (thrusting)
				mVars.movementDir = transform.up * currentSpeed * Time.deltaTime;
			else if (turnAxis != 0)
				currentSpeed -= acceleration;
		}
		transform.position += (mVars.movementDir);
	}

	struct MovementVars{
		public Vector3 movementDir;
		public Vector3 faceDir;

		public void Reset() {
			movementDir = Vector3.zero;
			faceDir = Vector3.zero;
		}
	}
}
