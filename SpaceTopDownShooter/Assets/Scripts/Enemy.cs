using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	// GameObject instance of target, in this case the player
	public GameObject target;

	// GameObject prefab for bullet
	public GameObject bullet;

	// CharacterMotor instance
	private CharacterMotor motor;

	private float shootDistanceThreshold;
	private float distanceToTargetOffset;

	private float speed;

	// Use this for initialization
	void Start () {
		this.shootDistanceThreshold = 10f;
		this.distanceToTargetOffset = 4f;

		this.speed = 10f;

		this.motor = GetComponent<CharacterMotor>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		this.motor.Move(this.target.transform, this.distanceToTargetOffset, this.speed);
	}
}
