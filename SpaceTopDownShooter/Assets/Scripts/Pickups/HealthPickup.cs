using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {

	public float healthValue = 20;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<Player>().SendMessage("UpdateHealth",healthValue);
//			Destroy (this.gameObject);
			SimplePool.Despawn(this.gameObject);
		}
	}
}
