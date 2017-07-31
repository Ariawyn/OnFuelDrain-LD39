using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitRigidbodyVelocity : MonoBehaviour {

	public float maxSpeed = 40f;
	private Rigidbody2D body;

	void Start() {
		this.body = this.GetComponent<Rigidbody2D>();
	}

	void Update() {
		if(this.body.velocity.magnitude > this.maxSpeed) {
			this.body.velocity = this.body.velocity.normalized * this.maxSpeed;
		}
	}

}
