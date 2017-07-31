using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
	// Camera transform instance
	public Transform camera;

	public Transform cameratarget;

	// Duration of the effect
	public float shakeDuration = 0f;
	
	// Amplitude of the effect.
	public float shakeAmount = 3f;

	// How quickly the effect decreases
	public float decreaseFactor = 2f;
	
	// Speed of shake
	public float shakeSpeed = 12f;

	Vector3 originalPosition;

	void Awake() {
		if(!camera) {
			camera = this.transform;
		}
	}

	// Use this for initialization
	void Start () {
		if(!camera) {
			camera = this.transform;
		}
	}

	// OnEnable
	void OnEnable() {
		this.originalPosition = this.camera.position;
		if(!camera) {
			camera = this.transform;
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (this.shakeDuration > 0)
		{
			Vector3 currentShakePosition = this.cameratarget.position + (Random.insideUnitSphere / 2) * this.shakeAmount; 
			this.camera.localPosition = Vector3.Lerp(this.camera.localPosition, currentShakePosition, Time.deltaTime * this.shakeSpeed);
			
			this.shakeDuration -= Time.deltaTime * this.decreaseFactor;
		}
		else
		{
			this.shakeDuration = 0f;
			this.camera.localPosition = this.cameratarget.position;
			this.enabled = false;
		}
	}
}
