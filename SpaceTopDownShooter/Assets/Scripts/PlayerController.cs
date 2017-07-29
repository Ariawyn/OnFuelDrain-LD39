using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float maxSpeed = 20f;
	public float acceleration = 0.2f;
	public float turnStrength = 2f;

	float currentSpeed;
	private InputManager inputManager;

	void Awake() {
		inputManager = Object.FindObjectOfType<InputManager> ();
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

		if (vertAxis > 0) {
			if (currentSpeed + acceleration <= maxSpeed) {
				currentSpeed += acceleration;
			}
		}
		if (vertAxis < 0) {
			if (currentSpeed + acceleration >= 0) {
				currentSpeed -= acceleration;
			}
		}

		transform.position += (transform.up * currentSpeed * Time.deltaTime);
	}
}
