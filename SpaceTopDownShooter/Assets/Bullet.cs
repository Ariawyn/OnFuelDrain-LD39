using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float moveSpeed;

	public float destroyTimer = 5;

	void FixedUpdate () {
		destroyTimer -= Time.deltaTime;
		if (destroyTimer <= 0) {
			SimplePool.Despawn (this.gameObject);
		}
		transform.position += transform.up * moveSpeed * Time.fixedDeltaTime;
	}

	public void SetBulletSpeed(float f){
		moveSpeed = f;
	}
}
