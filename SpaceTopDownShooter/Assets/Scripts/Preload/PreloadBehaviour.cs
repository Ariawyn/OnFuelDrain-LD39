using UnityEngine;

public class PreloadBehaviour : MonoBehaviour {

	void Start () {
		UnityEngine.SceneManagement.SceneManager.LoadScene(1);
	}
}