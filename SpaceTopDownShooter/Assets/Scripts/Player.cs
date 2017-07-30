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

	CharacterMotor motor;

	private InputManager inputManager;
	private AudioManager audioManager;

	// For shooting coroutine
	private float secondsBetweenBulletFire = 0.25f;
	private bool isRunningShootingCoroutine = true;

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
		motor.Move (vInput, hInput);
	}

	IEnumerator Shoot() {
		while(isRunningShootingCoroutine) {
			if (inputManager.GetKey ("Fire")) {
				this.audioManager.Play("Fire!");
				GameObject bullet = GameObject.Instantiate (bulletGO,transform.position,transform.rotation);
				bullet.GetComponent<Bullet> ().SetBulletSpeed (bulletSpeed);
				yield return new WaitForSeconds(secondsBetweenBulletFire);
			} else {
				yield return null;
			}
		}
		yield return null;
	}
}
