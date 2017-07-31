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
	bool playerFound = false;

	// Event system for updating the score in GUI.
	public delegate void ScoreUpdatedEvent(int score);
	public event ScoreUpdatedEvent OnScoreUpdated;

	// Timer variable for enemy spawns
	private float gameTimer;
	private bool timerIsCounting;

	// Enemy prefab objects
	public GameObject basicEnemyPrefab;
	public GameObject bossEnemyPrefab;

	// Enemy spawn time values
	public int basicEnemySpawnTime; 

	// Max total basic enemy count
	private int basicEnemyCount;
	private int maxBasicEnemyAmount;

	// Spawner variables
	private bool hasSpawnedForBasicEnemyTime;
	private int maxBasicEnemyGroupSpawnAmount;
	private int lastSpawnTime;
	private int spawnOffsetDistance;

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
		this.player = FindObjectOfType<Player>();
		//this.player.OnPlayerTookDamage += UpdateScore;

		// Initialise timer variablse
		this.gameTimer = 0.0f;
		this.timerIsCounting = false;

		// Set enemy spawn times
		this.basicEnemySpawnTime = 5;

		// Set enemy max amount
		this.maxBasicEnemyAmount = 25;
		this.basicEnemyCount = 0;

		// Set spawner variables
		this.hasSpawnedForBasicEnemyTime = false;
		this.maxBasicEnemyGroupSpawnAmount = 5;
		this.lastSpawnTime = 0;
		this.spawnOffsetDistance = 20;
	}

	void Update() {
		// Attempt to locate player object and add UpdateScore to OnPlayerTookDamage
		if(!this.playerFound) {
			this.player = FindObjectOfType<Player>();
			if(this.player) {
				this.player.OnPlayerHealthChanged += UpdateScore;
				this.playerFound = true;
			}
		}

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

	void FixedUpdate() {
		// Increment the timer when we need to
		if(this.state == GAME_STATE.RUNNING && this.timerIsCounting) {
			// Increment timer
			this.gameTimer += Time.deltaTime;

			// RoundedTimer
			int roundedTimer = Mathf.CeilToInt(this.gameTimer);

			if(this.lastSpawnTime != 0) {
				if(((roundedTimer % this.lastSpawnTime) + 1) == this.basicEnemySpawnTime) {
					this.hasSpawnedForBasicEnemyTime = false;
				}
			}

			if((roundedTimer % this.basicEnemySpawnTime == 0) && (roundedTimer != 0) && (!this.hasSpawnedForBasicEnemyTime)) {
				// We now need to spawn enemies

				// Calculate amount we wish to spawn based on the timer
				int amountOfEnemiesToSpawn = 1 * Mathf.FloorToInt(roundedTimer / this.basicEnemySpawnTime) / 2;

				// Check if the amount we want is greater than the max amount allowed to spawn at once
				// If it is, then we just set the desired amount to the max amount
				amountOfEnemiesToSpawn = (amountOfEnemiesToSpawn > this.maxBasicEnemyGroupSpawnAmount)? 
					this.maxBasicEnemyGroupSpawnAmount : amountOfEnemiesToSpawn;

				// Set spawner variables
				this.hasSpawnedForBasicEnemyTime = true;
				this.lastSpawnTime = roundedTimer;

				// Call spawn function with desired amount
				this.SpawnBasicEnemies(amountOfEnemiesToSpawn);
			}
		}
	}

	public void Tutorial() {
		Debug.Log("Now in tutorial");
		UnityEngine.SceneManagement.SceneManager.LoadScene(2);
	}

	// Function to be called when Play button is hit in main menu
	public void Play() {
		UnityEngine.SceneManagement.SceneManager.LoadScene(3);

		Debug.Log("Now playing");
		// Set game state
		this.state = GAME_STATE.RUNNING;
		// TODO: Switch scene from main_menu to game scene
		// TODO: Start the enemy spawners and score counting

		// Start counting timer
		this.timerIsCounting = true;
	}

	// Function to be called when esc is pressed in game
	public void Pause() {
		// Set game state
		this.state = GAME_STATE.PAUSED;

		// Pause game
		Time.timeScale = 0;

		// Set timer to not be counting
		this.timerIsCounting = false;

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

		// Set timer to counting
		this.timerIsCounting = true;

		// Unpause the BGM in audio manager
		this.audioManager.UnpauseBGM();

		// TODO: disable pause menu ui overlay
	}

	public void EndGame() {

		// Set game state
		this.state = GAME_STATE.FINISHED;

		// Destroy player object
		Destroy(this.player);

		Debug.Log("Game over");
		UnityEngine.SceneManagement.SceneManager.LoadScene(4);

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

		if(roundedPoints < 0) {
			// This is because we have been taking damage so we get passed a negative amount
			roundedPoints = -roundedPoints;
		}

		this.score += roundedPoints;

		// Notify ui manager
		OnScoreUpdated(roundedPoints);
	}

	public void DecrementEnemyCount() {
		this.basicEnemyCount--;
	}

	private void SpawnBasicEnemies(int amount) {

		Debug.Log("Attempting to spawn: " + amount + " of basic enemies at " + Mathf.RoundToInt(this.gameTimer));

		// Loop through amount
		for(int i = 0; i < amount; i++) {
			// Check if we would exceed the max basic enemy amount allowed
			if(this.basicEnemyCount + 1 < this.maxBasicEnemyAmount) {
				// Get random radian angle
				float radian = Random.Range(0f, Mathf.PI*2);
				
				// Calculate x and y pos of angle
				float xPos = Mathf.Cos(radian);
				float yPos = Mathf.Sin(radian);

				// Calculate spawn point from spawn offset distance and the player position
				Vector3 spawnPoint = new Vector3(xPos, yPos, 0) * this.spawnOffsetDistance;
				spawnPoint = this.player.transform.position + spawnPoint;

				// Instantiate the enemy
//				GameObject instantiated = (GameObject)Instantiate(this.basicEnemyPrefab, spawnPoint, Quaternion.identity);
				GameObject instantiated = SimplePool.Spawn(this.basicEnemyPrefab, spawnPoint, Quaternion.identity);
				Enemy spawned = instantiated.GetComponent<Enemy>();

				// SimplePool can't call Awake() or Start() on spawn, so we need to manually call that stuff.
				spawned.InitializeEverything ();

				// Set the players position as target
				spawned.target = this.player.transform;

				// TODO: Determine whether the enemy drops health. This probably shouldn't just be random.
				float dropsHealth = Random.Range(0,1);

				spawned.dropsHealth = (dropsHealth == 0) ? false : true;

				// Increment basic enemy count
				this.basicEnemyCount++;
			} else {
				// If we would exceed the max basic enemy amount allowed, then we just return early and do nothing more
				return;
			}
		}
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
