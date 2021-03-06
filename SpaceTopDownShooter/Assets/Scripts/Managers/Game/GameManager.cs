﻿using UnityEngine;
using UnityEngine.UI;

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

	// Timer variable for enemy spawns
	private float gameTimer;
	private bool timerIsCounting;

	// Enemy prefab objects
	public GameObject basicEnemyPrefab;
	public GameObject otherEnemyPrefab;

	// Enemy spawn time values
	public int basicEnemySpawnTime; 
	public int otherEnemySpawnTime;

	// Max enemy count variables
	private int basicEnemyCount;
	private int maxBasicEnemyAmount;
	private int otherEnemyCount;
	private int maxOtherEnemyAmount;

	// Spawner variables
	private bool hasSpawnedForBasicEnemyTime;
	private bool hasSpawnedForOtherEnemyTime;
	private int maxBasicEnemyGroupSpawnAmount;
	private int maxOtherEnemyGroupSpawnAmount;
	private int lastSpawnTime;
	private int lastOtherEnemySpawnTime;
	private int spawnOffsetDistance;

	// UI specific variables that cannot be handled elsewhere
	private GameObject pauseMenu;
	private Text scoreGUItext;
	private Text timerGUItext;
	private Text endScoreText;


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

		// Set basic enemy spawn times
		this.basicEnemySpawnTime = 5;

		// Set basic enemy max amount and count
		this.maxBasicEnemyAmount = 25;
		this.basicEnemyCount = 0;

		// Set basic enemy spawner variables
		this.hasSpawnedForBasicEnemyTime = false;
		this.maxBasicEnemyGroupSpawnAmount = 5;
		this.lastSpawnTime = 0;
		this.spawnOffsetDistance = 20;

		// Set other enemy spawn times
		this.otherEnemySpawnTime = 10;

		// Set other enemy max amount and count
		this.maxOtherEnemyAmount = 10;
		this.otherEnemyCount = 0;

		// Set other enemy spawner variables
		this.hasSpawnedForOtherEnemyTime = false;
		this.lastOtherEnemySpawnTime = 0;
		this.maxOtherEnemyGroupSpawnAmount = 3;
	}

	void Update() {
		// Attempt to locate player object and add UpdateScore to OnPlayerTookDamage
		if(!this.player) {
			this.player = FindObjectOfType<Player>();
			if(this.player) {
				this.player.OnPlayerWasHit += UpdateScore;
			}
		}

		// Attempt to locate pause menu if the game is running
		if(this.state == GAME_STATE.RUNNING && !this.pauseMenu) {
			//Debug.Log("The game is running and we do not have instance of pause menu");
			this.pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");

			// Check if we found the pause menu
			if(this.pauseMenu) {
				// We found it
				//Debug.Log("We found it!");
				this.pauseMenu.SetActive(false);
			}
		}

		// Attempt to locate score text if the game is running
		if(this.state == GAME_STATE.RUNNING && !this.scoreGUItext) {
			//Debug.Log("The game is running and we do not have instance of scoreGUItext");
			GameObject temp = GameObject.FindGameObjectWithTag("ScoreText");
			
			// See if we found an object with the score text tag
			if(temp) {
				// If we do then we set the text
				this.scoreGUItext = temp.GetComponent<Text>() as Text;
			}

			// Init
			if(this.scoreGUItext) {
				//Debug.Log("Found score text");
				this.scoreGUItext.text = "0";
			}
		}

		// Attempt to locate timer text if the game is running
		if(this.state == GAME_STATE.RUNNING && !this.timerGUItext) {
			//Debug.Log("The game is running and we do not have instance of timerGUItext");
			GameObject temp = GameObject.FindGameObjectWithTag("TimeText");

			// See if we found an object with the time text tag
			if(temp) {
				// Set the timer text variable instance
				this.timerGUItext = temp.GetComponent<Text>() as Text;
			}

			// So if we have found and set the text instance variable
			if(this.timerGUItext) {
				//Debug.Log("Found timer text");
				this.timerGUItext.text = this.gameTimer.ToString();
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


		// Attempt to locate and display score text if the game has ended
		if(this.state == GAME_STATE.FINISHED && !this.endScoreText) {
			//Debug.Log("The game is running and we do not have instance of timerGUItext");
			GameObject temp = GameObject.FindGameObjectWithTag("EndScoreText");

			// See if we found an object with the end score text tag
			if(temp) {
				// Set the end score text variable instance
				this.endScoreText = temp.GetComponent<Text>() as Text;
			}

			// So if we have found and set the text instance variable
			if(this.endScoreText) {
				//Debug.Log("Found timer text");
				// Display the score
				this.endScoreText.text = this.score.ToString();
			}
		}
	}

	void FixedUpdate() {
		// Increment the timer when we need to
		if(this.state == GAME_STATE.RUNNING && this.timerIsCounting) {
			// Increment timer
			this.gameTimer += Time.deltaTime;
			
			// See if we need to attempt to locate timer text in GUI if the game is running
			if(!this.timerGUItext) {
				//Debug.Log("We want to update timerGUItext and we do not have instance of timerGUItext");
				GameObject temp = GameObject.FindGameObjectWithTag("TimeText");

				// See if we found an object with the time text tag
				if(temp) {
					// Set the timer text variable instance
					this.timerGUItext = temp.GetComponent<Text>() as Text;
				}
			}

			// Update the timer in the GUI
			double roundedCurrentTime = System.Math.Round(this.gameTimer, 2);
			this.timerGUItext.text = roundedCurrentTime.ToString();

			// RoundedTimer
			int roundedTimer = Mathf.CeilToInt(this.gameTimer);

			// Check whether we should reset spawn variables to spawn next wave of basic enemies
			if(this.lastSpawnTime != 0) {
				if(((roundedTimer % this.lastSpawnTime) + 1) == this.basicEnemySpawnTime) {
					this.hasSpawnedForBasicEnemyTime = false;
				}
			}

			// Check whether we should reset spawn variables to spawn next wave of non-basic or "other" enemies
			if(this.lastOtherEnemySpawnTime != 0) {
				if(((roundedTimer % this.lastOtherEnemySpawnTime) + 1) == this.otherEnemySpawnTime) {
					this.hasSpawnedForOtherEnemyTime = false;
				}
			}

			// Check whether it is time to spawn next wave of basic enemies
			if((roundedTimer % this.basicEnemySpawnTime == 0) && (roundedTimer != 0) && (!this.hasSpawnedForBasicEnemyTime)) {
				// We now need to spawn the basic enemies at the right amount

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

			// Check whether it is time to spawn next wave of non-basic or "other" enemies
			if((roundedTimer % this.otherEnemySpawnTime == 0) && (roundedTimer != 0) && (!this.hasSpawnedForOtherEnemyTime)) {
				// We now need to spawn the other enemies at the right amount
				
				// Calculate desired amount
				int amountOfEnemiesToSpawn = 1 * Mathf.FloorToInt(roundedTimer / this.otherEnemySpawnTime) / 2;

				// Check if the amount we want to spawn is greater than max amount allowed to spawn in a wave
				// If it is then we set the amount to the max amount
				amountOfEnemiesToSpawn = (amountOfEnemiesToSpawn > this.maxOtherEnemyGroupSpawnAmount)? 
					this.maxOtherEnemyGroupSpawnAmount : amountOfEnemiesToSpawn;

				// Set spawner variables to reset clock on next wave
				this.hasSpawnedForOtherEnemyTime = true;
				this.lastOtherEnemySpawnTime = roundedTimer;

				// Call spawn function with desired amount
				this.SpawnOtherEnemies(amountOfEnemiesToSpawn);
			}
		}
	}

	public void Menu() {
		this.state = GAME_STATE.MAIN_MENU;

		// Change to Tutorial scene
		UnityEngine.SceneManagement.SceneManager.LoadScene(1);
	}

	public void Tutorial() {
		this.state = GAME_STATE.TUTORIAL;

		// Change to Tutorial scene
		UnityEngine.SceneManagement.SceneManager.LoadScene(2);
	}

	// Function to be called when Play button is hit in main menu
	public void Play() {
		// Change to game scene
		UnityEngine.SceneManagement.SceneManager.LoadScene(3);

		this.preloadEnemySpawns();

		// Set game state
		this.state = GAME_STATE.RUNNING;

		// Start counting timer
		this.timerIsCounting = true;
	}

	// Function to be called when esc is pressed in game
	public void Pause() {
		// Set game state
		this.state = GAME_STATE.PAUSED;

		// Check if we have the pause menu
		if(!this.pauseMenu) {
			this.pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
		}

		// Enable pause menu
		this.pauseMenu.SetActive(true);

		// Pause game
		Time.timeScale = 0;

		// Set timer to not be counting
		this.timerIsCounting = false;

		// Pause the BGM in audio manager
		this.audioManager.PauseBGM();
	}

	// Function to be called when esc or play is pressed from pause menu
	public void Unpause() {
		// Set game state
		this.state = GAME_STATE.RUNNING;

		// Check if we have the pause menu
		if(!this.pauseMenu) {
			this.pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
		}

		// Disable pause menu
		this.pauseMenu.SetActive(false);

		// Unpause game
		Time.timeScale = 1;

		// Set timer to counting
		this.timerIsCounting = true;

		// Unpause the BGM in audio manager
		this.audioManager.UnpauseBGM();

	}

	public void EndGame() {

		// Set game state
		this.state = GAME_STATE.FINISHED;


		this.timerIsCounting = false;

		UnityEngine.SceneManagement.SceneManager.LoadScene(4);

		// Check for high score
		if(this.score > this.highscore) {
			this.highscore = this.score;
			this.SaveHighScore();
		}
	}

	public void UpdateScore(float pointsToAdd) {
		// Add points to score
		int roundedPoints = Mathf.RoundToInt(pointsToAdd);

		if(roundedPoints < 0) {
			// This is because we have been taking damage so we get passed a negative amount
			roundedPoints = -roundedPoints;
		}

		this.score += roundedPoints;

		// Attempt to locate score text if the game is running
		if(!this.scoreGUItext) {
			//Debug.Log("We want to update scoreGUItext and we do not have instance of scoreGUItext");
			GameObject temp = GameObject.FindGameObjectWithTag("ScoreText");
			
			// See if we found an object with the score text tag
			if(temp) {
				// If we do then we set the text
				this.scoreGUItext = temp.GetComponent<Text>() as Text;
			}
		}

		this.scoreGUItext.text = this.score.ToString();
	}

	public void ResetScore() {
		this.score = 0;
	}

	public void ResetTimer() {
		this.gameTimer = 0f;
	}

	public void ResetAudio() {
		this.audioManager.initLibrary();
	}

	public void DecrementBasicEnemyCount() {
		this.basicEnemyCount--;
	}

	public void DecrementOtherEnemyCount() {
		this.otherEnemyCount--;
	}

	private void preloadEnemySpawns() {
		SimplePool.Preload(this.basicEnemyPrefab, this.maxBasicEnemyAmount);
		SimplePool.Preload(this.otherEnemyPrefab, this.maxOtherEnemyAmount);
	}

	private void SpawnBasicEnemies(int amount) {

		//Debug.Log("Attempting to spawn: " + amount + " of basic enemies at " + Mathf.RoundToInt(this.gameTimer));

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
				//GameObject instantiated = (GameObject)Instantiate(this.basicEnemyPrefab, spawnPoint, Quaternion.identity);
				GameObject instantiated = SimplePool.Spawn(this.basicEnemyPrefab, spawnPoint, Quaternion.identity);
				Enemy spawned = instantiated.GetComponent<Enemy>();

				// SimplePool can't call Awake() or Start() on spawn, so we need to manually call that stuff.
				spawned.InitializeEverything ();

				// Set the players position as target
				spawned.target = this.player.transform;

				// Set isBasic to true
				spawned.isBasic = true;

				// TODO: Determine whether the enemy drops health. This probably shouldn't just be random.
				float dropsHealth = Random.Range(0,2);

				if (dropsHealth == 1)
					//Debug.Log ("The enemy should have dropsHealth set to true");

				spawned.dropsHealth = (dropsHealth < 1) ? false : true;

				// Increment basic enemy count
				this.basicEnemyCount++;
			} else {
				// If we would exceed the max basic enemy amount allowed, then we just return early and do nothing more
				return;
			}
		}
	}

	private void SpawnOtherEnemies(int amount) {
		//Debug.Log("Attempting to spawn: " + amount + " of other enemies at " + Mathf.RoundToInt(this.gameTimer));

		// Loop through amount
		for(int i = 0; i < amount; i++) {
			// Check if we would exceed the max basic enemy amount allowed
			if(this.otherEnemyCount + 1 < this.maxOtherEnemyAmount) {
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
				GameObject instantiated = SimplePool.Spawn(this.otherEnemyPrefab, spawnPoint, Quaternion.identity);
				Enemy spawned = instantiated.GetComponent<Enemy>();

				// SimplePool can't call Awake() or Start() on spawn, so we need to manually call that stuff.
				spawned.InitializeEverything ();

				// Set the players position as target
				spawned.target = this.player.transform;

				// Set isBasic to false
				spawned.isBasic = false;

				// TODO: Determine whether the enemy drops health. This probably shouldn't just be random.
				float dropsHealth = Random.Range(0,2);

				if (dropsHealth >= 1)
					//Debug.Log ("The enemy should have dropsHealth set to true");

				spawned.dropsHealth = (dropsHealth < 1) ? false : true;

				// Increment basic enemy count
				this.otherEnemyCount++;
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
