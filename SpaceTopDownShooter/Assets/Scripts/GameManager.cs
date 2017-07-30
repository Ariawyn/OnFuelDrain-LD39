using UnityEngine;

public class GameManager : MonoBehaviour {

	// Game state variable!
	private GAME_STATE state;

	// Score based variables
	private int score;
	public int Score { get { return score; }}
	private int highscore;

	// Behaviour manager instances
	private InputManager inputManager;
	private AudioManager audioManager;

	// Cache the player to receive score update
	Player player;

	// Event system for updating the score in GUI.
	public delegate void ScoreUpdatedEvent(int score);
	public event ScoreUpdatedEvent OnScoreUpdated;

	// Use this for initialization
	void Start () {
		// Set to proper game state for the beginning
		this.state = GAME_STATE.MAIN_MENU;

		// Initialize highscore and current game score
		this.LoadHighScore();
		this.score = 0;

		// Get instance of input manager
		this.inputManager = Object.FindObjectOfType<InputManager>();

		// Get instance of audio manager
		this.audioManager = Object.FindObjectOfType<AudioManager>();

		// Get player
		player = FindObjectOfType<Player>();
		player.OnPlayerTookDamage += UpdateScore;

		// TODO: Maybe dont do this when we get the main menu done
		this.Play();
	}

	void Update() {
		// Check if game is paused, this should only occur if the game is in the running or paused state, not menu or finished
		if(this.state == GAME_STATE.RUNNING || this.state == GAME_STATE.PAUSED) {
			// If the pause key is pressed
			if(this.inputManager.GetKeyDown("Pause")) {
				// We either pause or unpause
				if(this.isPaused()) {
					this.Unpause();
				} else {
					this.Pause();
				}
			}
		}
	}

	// Function to be called when Play button is hit in main menu
	public void Play() {
		// Set game state
		this.state = GAME_STATE.RUNNING;
		// TODO: Switch scene from main_menu to game scene
		// TODO: Start the enemy spawners and score counting
	}

	// Function to be called when esc is pressed in game
	public void Pause() {
		// Set game state
		this.state = GAME_STATE.PAUSED;

		// Pause game
		Time.timeScale = 0;

		// Pause the BGM in audio manager
		this.audioManager.PauseBGM();

		// TODO: enable pause menu ui overlay
	}

	// Function to be called when esc or play is pressed from pause menu
	public void Unpause() {
		// Set game state
		this.state = GAME_STATE.RUNNING;

		// Unpause game
		Time.timeScale = 1;

		// Unpause the BGM in audio manager
		this.audioManager.UnpauseBGM();

		// TODO: disable pause menu ui overlay
	}

	public void Finish() {
		// Set game state
		this.state = GAME_STATE.FINISHED;

		// Check for high score
		if(this.score > this.highscore) {
			this.highscore = this.score;
			this.SaveHighScore();
		}

		//TODO: Switch to game finished scene

	}

	public void UpdateScore(float pointsToAdd) {
		// Add points to score
		int roundedPoints = Mathf.RoundToInt(pointsToAdd);
		this.score += roundedPoints;

		// Notify ui manager
		OnScoreUpdated(roundedPoints);
	}

	private void LoadHighScore() {
		// TODO: Implement reading high score from player prefs
		this.highscore = 0;
		return;
	}

	private void SaveHighScore() {
		// TODO: Implement saving high score from player prefs
		return;
	}

	public bool isPaused() {
		return this.state == GAME_STATE.PAUSED;
	}
}
