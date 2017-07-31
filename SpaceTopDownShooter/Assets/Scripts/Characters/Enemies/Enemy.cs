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

	// Game manager instance, used for updating score
	private GameManager gameManager;

	// Health
	public float health = 10f;

	private float shootDistanceThreshold;
	private float minDistance;
	private float maxDistance;
	private bool inRange;

	private float speed;

	private int worth;

	void Awake() {
		this.gameManager = Object.FindObjectOfType<GameManager>();
	}

	// Use this for initialization
	void Start () {
		this.maxDistance = this.shootDistanceThreshold = 8f;
		this.minDistance = 4f;

		this.inRange = false;

		this.worth = 5;

		this.speed = 14f;

		this.motor = GetComponent<CharacterMotor>();
	}

	void Update() {
		if (health <= 0) {
			Destroy (this.gameObject);
			this.gameManager.UpdateScore(this.worth);
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
