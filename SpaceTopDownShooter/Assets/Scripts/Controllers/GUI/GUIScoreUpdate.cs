using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIScoreUpdate : MonoBehaviour {

	Text t;
	GameManager gm;

	int score;

	Vector3 tPosition;

	// Use this for initialization
	void Awake () {
		t = GetComponent<Text> ();
		gm = FindObjectOfType < GameManager> ();
		score = gm.Score;
		gm.OnScoreUpdated += UpdateScore;
		t.text = score.ToString();

		tPosition = t.rectTransform.position;
	}
	
	void UpdateScore (int add){
		score += add;
		t.text = score.ToString();
	}

}
