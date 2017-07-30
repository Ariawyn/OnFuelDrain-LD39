using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	// GameObject instance of target, in this case the player
	public Transform target;

	// GameObject prefab for bullet
	public GameObject bullet;

	// CharacterMotor instance
	private CharacterMotor motor;

	// Health
	public float health = 20f;

	private float shootDistanceThreshold;
	private float minDistance;
	private float maxDistance;
	private bool inRange;

	private float speed;

	// Use this for initialization
	void Start () {
		this.maxDistance = this.shootDistanceThreshold = 8f;
		this.minDistance = 4f;

		this.inRange = false;

		this.speed = 5f;

		this.motor = GetComponent<CharacterMotor>();
	}
	
	// Update is called once per frame
	//void FixedUpdate () {
		//this.motor.Move(this.target.transform, this.distanceToTargetOffset, this.speed);
		/*Vector2 direction = this.target.transform.position - this.transform.position;
		float magnitude = direction.magnitude;
		direction.Normalize();
		this.motor.Move(direction.x, direction.y);*/
	//}*/

	void Update() {
		if (health <= 0) {
			Destroy (this.gameObject);
		}
	}

	void LateUpdate() {
		if(!target) {
			return;
		}

		this.motor.Move(this.target, this.minDistance, this.maxDistance, this.speed, ref this.inRange);
	}

	public void TakeDamage(float damage) {
//		Debug.Log ("Yass you got the bad guy");
		health -= damage;
	}
}
