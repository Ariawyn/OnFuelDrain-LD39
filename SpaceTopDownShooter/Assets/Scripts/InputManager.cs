
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	// Basic input axis for horizontal and vertical movement
	public InputAxis horizontalAxis;
	public InputAxis verticalAxis;

	// Basic controls dictionary
	private Dictionary<string, KeyCode> controls;

	// Use this for initialization
	void Start () {
		// Initialize controls
		this.initControls();
	}

	public bool GetKeyDown(string keyName) {
		// Check if the key exists
		if(this.controls.ContainsKey(keyName)) {
			// Then we do have the key in the control layout
			return Input.GetKeyDown(this.controls[keyName]);
		}

		// We do not have the key registered in our layout
		return false;
	}

	private void initControls() {
		// Attempt to load from player prefs
		this.controls = this.loadControlLayout();

		// If we have nothing in the dictionary, then we add default controls
		if(this.controls.Count == 0) {
			// Basic movement keys:
			this.controls["Up"] = KeyCode.W;
			this.controls["Down"] = KeyCode.S;
			this.controls["Right"] = KeyCode.D;
			this.controls["Left"] = KeyCode.A;

			// Set the axis inputs
			this.horizontalAxis.setPositiveKey(this.controls["Right"]);
			this.horizontalAxis.setNegativeKey(this.controls["Left"]);
			this.verticalAxis.setPositiveKey(this.controls["Up"]);
			this.verticalAxis.setNegativeKey(this.controls["Down"]);
		}
	}

	private Dictionary<string, KeyCode> loadControlLayout() {
		// TODO: Implement reading controls from player prefs
		return new Dictionary<string, KeyCode>();
	}

	private void saveControlLayout() {
		// TODO: Implement saving controls to player prefs
		return;
	}

}