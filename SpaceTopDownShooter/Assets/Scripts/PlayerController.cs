using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float maxSpeed = 20f;
	public float acceleration = 0.2f;
	public float turnStrength = 2f;

	float currentSpeed;

	// Use this for initialization
	void Start () {
		currentSpeed = 0;
	}
	
	// Update is called once per frame
	void Update () {

		float turnAxis = Input.GetAxis ("Horizontal") * turnStrength * -1;

		transform.Rotate (new Vector3 (0, 0, turnAxis));

		if (Input.GetKey (KeyCode.W)) {
			if (currentSpeed + acceleration <= maxSpeed) {
				currentSpeed += acceleration;
			}
		}
		if (Input.GetKey (KeyCode.S)) {
			if (currentSpeed + acceleration >= 0) {
				currentSpeed -= acceleration;
			}
		}

		transform.Translate (transform.up * currentSpeed * Time.deltaTime);
	}
}
