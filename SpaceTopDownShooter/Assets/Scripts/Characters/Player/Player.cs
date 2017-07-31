using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	/// <summary>
	/// The bullet gameObject
	/// </summary>
	public GameObject bulletGO;

	/// <summary>
	/// The speed bullets will move at. NOT the fire rate.
	/// </summary>
	public float bulletSpeed = 5f;

	public float health = 1000f;

	public delegate void PlayerTookDamageEvent (float hp);

	public event PlayerTookDamageEvent OnPlayerHealthChanged;

	[Range(0,Mathf.Infinity)]
	public float fuel = 1000f;

	public float maxFuel;

	public float fuelLossRate = 1f;

	public delegate void PlayerFuelIncreasedEvent (float f);
	public event PlayerFuelIncreasedEvent OnPlayerFuelIncreased;

	CharacterMotor motor;

	private InputManager inputManager;
	private AudioManager audioManager;
	private GameManager gameManager;

	// For shooting coroutine
	private float secondsBetweenBulletFire = 0.25f;
	private bool isRunningShootingCoroutine = true;

	public Transform[] gunTransforms;
	public bool alternateGuns = true;
	int gIndex = 0;

	// Camera instance to call shake effect
	private CameraController cam;

	void Awake() {
		inputManager = Object.FindObjectOfType<InputManager> ();
		audioManager = Object.FindObjectOfType<AudioManager>();
		gameManager = Object.FindObjectOfType<GameManager>();
		motor = GetComponent<CharacterMotor> ();
		SimplePool.Preload (bulletGO, 20);
		maxFuel = fuel;
	}

	void Start () {
//		bulletSpeed = maxSpeed * 1.5f;

		cam = Object.FindObjectOfType<CameraController>();

		// Start running the shoot coroutine, which allows for us to wait a few seconds for when the player can fire after he already did
		StartCoroutine(Shoot());

	}

	void Update() {
		// Check if the player has died
		if (this.health <= 0) {
			// Call game manager finish function
			this.gameManager.EndGame();
		}
	}

	void FixedUpdate() {
		/*if(!cam) {
			cam = Object.FindObjectOfType<CameraController>();
		}*/

//		bulletSpeed = (motor.currentSpeed > motor.maxSpeed) ? motor.currentSpeed : motor.maxSpeed;
		float hInput = this.inputManager.horizontalAxis.GetRawAxisInput ();
		float vInput = this.inputManager.verticalAxis.GetRawAxisInput ();
		ReduceFuel (fuelLossRate);
		/*
		if (fuel % 5 == 0)
			Debug.Log ("Fuel: " + fuel);
		*/
		if (fuel <= 0) {
			hInput = 0;
			vInput = 0;
		}
		motor.Move (vInput, hInput);
	}

	public void UpdateHealth(float amount) {
		float oldHealth = this.health;
		this.health += amount;
		bool takingdamage = (this.health < oldHealth);

		if(takingdamage) {
			// Add fuel to amount * 2
			if (this.fuel + (-amount * 2) <= this.maxFuel)
				this.fuel += (-amount * 2);
			else
				this.fuel = this.maxFuel;

			// Do camera shake effect
			if(this.cam) {
				this.cam.Shake();
			}
		
			if (OnPlayerFuelIncreased != null)
				OnPlayerFuelIncreased (this.fuel);

		}
		
		if (OnPlayerHealthChanged != null)
			OnPlayerHealthChanged(amount);
	}

	void ReduceFuel(float fuelLoss) {
		this.fuel -= fuelLoss;

		if(this.fuel < 0f) {
			this.fuel = 0f;
		}
	}

	IEnumerator Shoot() {
		while(isRunningShootingCoroutine) {
			if (inputManager.GetKey ("Fire")) {
				this.audioManager.Play("Fire!");
				if (alternateGuns) {
					if (gIndex > gunTransforms.Length - 1 || gIndex == null)
						gIndex = 0;
//					GameObject bullet = GameObject.Instantiate (bulletGO, gunTransforms[gIndex].position, transform.rotation);
					GameObject bullet = SimplePool.Spawn(bulletGO,gunTransforms[gIndex].position,transform.rotation);
					bullet.GetComponent<Bullet> ().SetBulletSpeed (bulletSpeed);
					bullet.GetComponent<Bullet> ().hurtsPlayer = false;
					gIndex+=1;
				} else {
					foreach (Transform t in gunTransforms) {
//						GameObject bullet = GameObject.Instantiate (bulletGO, t.position, transform.rotation);
						GameObject bullet = SimplePool.Spawn(bulletGO,t.position,transform.rotation);
						bullet.GetComponent<Bullet> ().SetBulletSpeed (bulletSpeed);
						bullet.GetComponent<Bullet> ().hurtsPlayer = false;
					}
				}

				yield return new WaitForSeconds(secondsBetweenBulletFire);
			} else {
				yield return null;
			}
		}
		yield return null;
	}
}
