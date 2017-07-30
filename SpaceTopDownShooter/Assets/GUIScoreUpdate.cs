using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIScoreUpdate : MonoBehaviour {

	Text t;
	GameManager gm;

	int score;

	// Use this for initialization
	void Start () {
		t = GetComponent<Text> ();
		gm = FindObjectOfType < GameManager> ();
		score = gm.Score;
		gm.OnScoreUpdated += UpdateScore;
		t.text = score.ToString();
	}
	
	void UpdateScore (int add){
		score += add;
		t.text = score.ToString();
	}

}
