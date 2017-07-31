using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

	// Sounds array, this should be set up through the inspector, useful for looping through audio instances
	public Sound[] sounds;

	// Dictionary of sounds to their name, useful for playing without having to find the audio track
	// This is initialized in a function, not through inspector
	Dictionary<string, Sound> library;

	// Instance of current BGM sound
	private Sound currentBGM;

	// Volume and pitch values
	private float musicVolume = 1f;
	private float effectsVolume = 1f;
	private float defaultPitch = 1f;

	// Initialize manager
	void Awake() {
		this.initLibrary();
		this.loadSavedSoundSettings();
	}

	void Start() {
		this.Play("Theme");
		this.currentBGM = this.library["Theme"];
	}

	public void Play(string name) {
		if(this.library.ContainsKey(name)) {
			Sound s = this.library[name];

			// Check if the sound is SFX or BGM
			if(s.isSFX) {
				// Randomize the effects volume and pitch by their random values
				s.source.volume = this.effectsVolume * (1 + Random.Range(-s.randomVolumeValue / 2f, s.randomVolumeValue / 2f));
				s.source.pitch = this.defaultPitch * (1 + Random.Range(-s.randomPitchValue / 2f, s.randomPitchValue / 2f));
				
				// Play the effect
				s.source.Play();
				return;
			} else {
				// Play the song
				s.source.Play();
			}
			return;
		}

		// If we get to this point, then we have no sound file for the given name
		Debug.Log("No sound file found for name: " + name);
	}

	public void PauseBGM() {
		if(this.currentBGM.source.isPlaying) {
			this.currentBGM.source.Pause();
		}
	}

	public void UnpauseBGM() {
		if(!this.currentBGM.source.isPlaying) {
			this.currentBGM.source.Play();
		}
	}

	public void setMusicVolume(float volume) {
		// Set new music volume value
		this.musicVolume = volume;

		// Change all music sound instances to new volume
		this.updateVolume();
	}

	public void setEffectsVolume(float volume) {
		// Set new music volume value
		this.effectsVolume = volume;

		// Change all effects sound instances to new volume
		this.updateVolume();
	}

	// Loops through and resets the sound sources to the correct current volume
	private void updateVolume() {
		foreach(Sound s in sounds) {
			if(s.isSFX) {
				s.source.volume = this.effectsVolume;
			} else {
				s.source.volume = this.musicVolume;
			}
		}
	}

	// Gets settings related to audio, if none saved, then everything is still set to default
	private void loadSavedSoundSettings() {
		// TODO: Read sound settings from player prefs overriding default values for music and effects volume
		return;
	}

	// Saves sound settings to player prefs
	private void saveSoundSettings() {
		// TODO: Store / save sound settings into player prefs
		return;
	}

	// Initializes sound dictionary "library" using the sounds array set through inspector
	private void initLibrary() {
		// Instantiate a new dictionary
		this.library = new Dictionary<string, Sound>();

		// Add all sound instances into dictionary
		foreach(Sound s in this.sounds) {
			// Add sound to library
			this.library[s.name] = s;

			// We also want to create all the audio sources here
			s.source = this.gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.isLooping;
		}
	}
}