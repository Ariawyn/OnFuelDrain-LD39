using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float moveSpeed;

	public float destroyTimer = 5;

	public bool hurtsPlayer = false;

	public float damage = 5;

	void FixedUpdate () {
		destroyTimer -= Time.fixedDeltaTime;
		if (destroyTimer <= 0) {
			Destroy (this.gameObject);
//			SimplePool.Despawn(this.gameObject);
		}
		transform.position += transform.up * moveSpeed * Time.fixedDeltaTime;
	}

	public void SetBulletSpeed(float f){
		moveSpeed = f;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Bullet"){
			Bullet b = other.gameObject.GetComponent<Bullet> ();
			if (b.hurtsPlayer) {
				other.SendMessage("TakeDamage", b.damage);
			}
		}
		else {

		}
	}

}
