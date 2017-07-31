using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	// GameObject instance of target, in this case the player
	public Transform target;

	// GameObject prefab for bullet
	public GameObject bullet;

	// GameOBject prefab for the health pickups
	public GameObject healthPickup;

	// Health pickup amount to heal.
	public float pickupHealAmount;

	// Whether or not this enemy drops health when killed.
	public bool dropsHealth;

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

	private float shootTime = 2.0f;
	private float currentShootTimer;

	private float bulletSpeed = 5f;

	private AudioManager audioManager;

	public Transform[] gunTransforms;

//	void Awake() {
//		this.gameManager = Object.FindObjectOfType<GameManager>();
//		this.audioManager = Object.FindObjectOfType<AudioManager>();
//	}
//
//	// Use this for initialization
//	void Start () {
//		this.maxDistance = this.shootDistanceThreshold = 8f;
//		this.minDistance = 4f;
//
//		this.inRange = false;
//
//		this.worth = 5;
//
//		this.speed = 14f;
//
//		this.shootTime = 2.0f;
//		this.currentShootTimer = this.shootTime;
//
//		this.motor = GetComponent<CharacterMotor>();
//	}

	/// <summary>
	/// SimplePool can't call Start() or Awake() on Spawning, so I'm moving all of the intialization stuff to this function..
	/// That way, we can call Initialize in the game manager for every spawn.
	/// </summary>
	public void InitializeEverything() {
		this.gameManager = Object.FindObjectOfType<GameManager>();
		this.audioManager = Object.FindObjectOfType<AudioManager>();

		this.maxDistance = this.shootDistanceThreshold = 8f;
		this.minDistance = 4f;

		this.inRange = false;

		this.worth = 5;

		this.speed = 14f;

		this.shootTime = 2.0f;
		this.currentShootTimer = this.shootTime;

		this.motor = GetComponent<CharacterMotor>();

	}

	void Update() {
		if (health <= 0) {
//			Destroy (this.gameObject);
			SimplePool.Despawn(this.gameObject);
			if (dropsHealth)
				SpawnHealthPickup ();
			this.gameManager.UpdateScore(this.worth);
			this.gameManager.DecrementEnemyCount();
		}
	}

	void LateUpdate() {
		if(!target) {
			return;
		}

		this.motor.Move(this.target, this.minDistance, this.maxDistance, this.speed, ref this.inRange);

		if(this.currentShootTimer < 0) {
			this.Shoot();
			this.currentShootTimer = this.shootTime;
		} else {
			this.currentShootTimer -= Time.deltaTime;
		}
	}

	private void Shoot() {
		this.audioManager.Play("Fire!");
		foreach (Transform t in gunTransforms) {
			Vector3 startPosition = t.position;
			//		GameObject instantiatedBullet = GameObject.Instantiate (this.bullet, startPosition, transform.rotation);
			GameObject instantiatedBullet = SimplePool.Spawn(this.bullet,startPosition,transform.rotation);
			Bullet currentBullet = instantiatedBullet.GetComponent<Bullet>();

			currentBullet.SetBulletSpeed (bulletSpeed);
			currentBullet.hurtsPlayer = true;
		}

	}

	public void UpdateHealth(float damage) {
//		Debug.Log ("Yass you got the bad guy");
		health += damage;
	}

	void SpawnHealthPickup(bool customPickup = false) {
		Debug.Log ("I dropped a pickup!");
		GameObject pickupGO = SimplePool.Spawn (healthPickup, this.transform.position, Quaternion.identity);
		HealthPickup pickup = pickupGO.GetComponent<HealthPickup> ();
		if (customPickup)
			pickup.healthValue = pickupHealAmount;
	}
}
