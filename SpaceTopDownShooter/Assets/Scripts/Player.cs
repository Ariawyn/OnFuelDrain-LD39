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
	public float bulletSpeed;

	public float health = 1000f;

	[Range(0,Mathf.Infinity)]
	public float fuel = 1000f;

	public float fuelLossRate = 1f;

	CharacterMotor motor;

	private InputManager inputManager;
	private AudioManager audioManager;

	// For shooting coroutine
	private float secondsBetweenBulletFire = 0.25f;
	private bool isRunningShootingCoroutine = true;

	public Transform[] gunTransforms;
	public bool alternateGuns = true;
	int gIndex = 0;

	void Awake() {
		inputManager = Object.FindObjectOfType<InputManager> ();
		audioManager = Object.FindObjectOfType<AudioManager>();
		motor = GetComponent<CharacterMotor> ();
		SimplePool.Preload (bulletGO, 20);
	}

	void Start () {
//		bulletSpeed = maxSpeed * 1.5f;

		// Start running the shoot coroutine, which allows for us to wait a few seconds for when the player can fire after he already did
		StartCoroutine(Shoot());
	}

	void FixedUpdate() {
		float hInput = this.inputManager.horizontalAxis.GetRawAxisInput ();
		float vInput = this.inputManager.verticalAxis.GetRawAxisInput ();
		ReduceFuel (fuelLossRate);
		if (fuel % 5 == 0)
			Debug.Log ("Fuel: " + fuel);

		if (fuel <= 0) {
			hInput = 0;
			vInput = 0;
		}
		motor.Move (vInput, hInput);
	}

	public void TakeDamage(float damage) {
		Debug.Log ("Ouch! " + damage);
		health -= damage;
		fuel += damage;
	}

	void ReduceFuel(float fuelLoss) {
		fuel -= fuelLoss;
	}

	IEnumerator Shoot() {
		while(isRunningShootingCoroutine) {
			if (inputManager.GetKey ("Fire")) {
				this.audioManager.Play("Fire!");
				if (alternateGuns) {
					if (gIndex > gunTransforms.Length - 1 || gIndex == null)
						gIndex = 0;
					GameObject bullet = GameObject.Instantiate (bulletGO, gunTransforms[gIndex].position, transform.rotation);
//					GameObject bullet = SimplePool.Spawn(bulletGO,gunTransforms[gIndex].position,transform.rotation);
					bullet.GetComponent<Bullet> ().SetBulletSpeed (bulletSpeed);
					gIndex+=1;
				} else {
					foreach (Transform t in gunTransforms) {
						GameObject bullet = GameObject.Instantiate (bulletGO, t.position, transform.rotation);
//						GameObject bullet = SimplePool.Spawn(bulletGO,t.position,transform.rotation);
						bullet.GetComponent<Bullet> ().SetBulletSpeed (bulletSpeed);
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
