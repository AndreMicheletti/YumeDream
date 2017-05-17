using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UI_GameOver : MonoBehaviour {

	public string sceneName = "Test";

	public void retry() {
		SceneManager.LoadScene (sceneName);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
