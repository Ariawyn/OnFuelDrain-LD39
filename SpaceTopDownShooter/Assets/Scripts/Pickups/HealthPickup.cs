using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {

	public float healthValue = 20;

	public float timerDefault = 30;

	float timer = 0;

	void OnEnable() {
		timer = timerDefault;
	}

	void Update() {
		timer -= Time.deltaTime;

		if (timer <= 0)
			SimplePool.Despawn (this.gameObject);
	}


	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<Player>().SendMessage("UpdateHealth",healthValue);
//			Destroy (this.gameObject);
			SimplePool.Despawn(this.gameObject);
		}
	}
}
