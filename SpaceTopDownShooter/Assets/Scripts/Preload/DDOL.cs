using UnityEngine;

// DDOL: Dont Die on Load. Literally does just that
public class DDOL : MonoBehaviour {
	// Awake here is used for init before anything else
	public void Awake() {
		DontDestroyOnLoad(gameObject);
		//Debug.Log("DDOL: " +  gameObject.name);
	}	
}