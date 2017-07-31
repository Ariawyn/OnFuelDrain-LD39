using UnityEngine;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour {
	// Manager instances
	private InputManager inputManager;
	private AudioManager audioManager;
	private GameManager gameManager;

	// Variable for holding currently selected item in menu
	public GameObject currentlySelectedObject;

	// EventSystem variable, in-built unity stuff
	public EventSystem eventSystem;	

	// Boolean variable to hold information on whether or not a button has been selected
	private bool buttonSelected = false;

	// Even earlier initialization
	void Awake () {
		// Get singleton manager instances
		this.inputManager = Object.FindObjectOfType<InputManager>();
		this.audioManager = Object.FindObjectOfType<AudioManager>();
		this.gameManager = Object.FindObjectOfType<GameManager>();
	}

	// Update is called once per frame
	void Update () {
		// Check for input on vertical axis
		if(this.inputManager.verticalAxis.GetRawAxisInput() != 0 && buttonSelected == false) {
			this.eventSystem.SetSelectedGameObject(currentlySelectedObject);
			this.buttonSelected = true;
		}
	}

		// When disabled
	private void OnDisable() {
		// button selection is set to false
		buttonSelected = false;
		this.audioManager.Play("Fire!");
	}

	// Play() function triggered on click of Play menu button
	public void Tutorial() {
		this.gameManager.Tutorial();
	}

	public void Play() {
		this.gameManager.Play();
	}

	public void Resume() {
		this.gameManager.Unpause();
	}

	// Quit() function triggered on click of Quit menu button
	public void Quit() {
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}
}
