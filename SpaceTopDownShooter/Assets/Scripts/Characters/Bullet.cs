﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float moveSpeed;

	public float destroyTimer = 5;
	public float currentTimer;

	public bool hurtsPlayer = false;

	/// <summary>
	/// THIS SHOULD BE NEGATIVE
	/// </summary>
	public float damage = -5;

	/// <summary>
	/// This defaults to -damage * 2;
	/// </summary>
	public float updateFuelAmount;

	void OnEnable() {
//		Debug.Log ("The bullets are being enabled");
		currentTimer = destroyTimer;
//		updateFuelAmount = -damage * 2;
	}

	void FixedUpdate () {
		currentTimer -= Time.fixedDeltaTime;
		if (currentTimer <= 0) {
//			Destroy (this.gameObject);
			SimplePool.Despawn(this.gameObject);
		}
		transform.position += transform.up * moveSpeed * Time.fixedDeltaTime;
	}

	public void SetBulletSpeed(float f){
		moveSpeed = f;
	}

	void OnTriggerEnter2D(Collider2D other) {
//		Debug.Log ("Found collider");
		if (other.gameObject.tag == "Player"){
			Player p = other.gameObject.GetComponent<Player> ();
			if (this.hurtsPlayer) {
				other.gameObject.SendMessage("WasHit", this.damage);
				other.gameObject.SendMessage ("UpdateFuel", updateFuelAmount);
//				Destroy(this.gameObject);
				SimplePool.Despawn(this.gameObject);
			}
		}
		if (other.gameObject.tag == "Enemy") {
//			Debug.Log ("I found the enemy");
			Enemy e = other.gameObject.GetComponent<Enemy> ();
			if (!this.hurtsPlayer) {
//				Debug.Log ("I'm the right kind of bullet");
				other.gameObject.SendMessage ("UpdateHealth", this.damage);
//				other.gameObject.SendMessage ("UpdateFuel", updateFuelAmount);
//				Destroy(this.gameObject);
				SimplePool.Despawn(this.gameObject);
			}
		}
	}

}
