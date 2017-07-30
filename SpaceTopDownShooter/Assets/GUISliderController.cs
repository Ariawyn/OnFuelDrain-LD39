using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUISliderController : MonoBehaviour {

	Player p;

	Slider s;

	public bool isFuel;


	// Use this for initialization
	void Awake() {
		p = FindObjectOfType<Player> ();
		s = this.GetComponent<Slider> ();
		if (!isFuel)
			p.OnPlayerHealthChanged += HandlePlayerTookDamage;

		if (!isFuel)
			s.maxValue = p.maxFuel;
		else
			s.maxValue = p.health;
	}
	
	void HandlePlayerTookDamage(float damage) {
		if (!isFuel)
			s.value += damage;
	}

	void Update() {
		if (isFuel) {
			s.value = p.fuel;
		}
	}
}
