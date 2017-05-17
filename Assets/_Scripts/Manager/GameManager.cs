using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	/** Public Variables */
	public GameObject gameUI;
	public GameObject pauseUI;
	public GameObject gameOverUI;
	public GameObject winUI;

	public GameObject playerObject;

	public int cristalsCollected = 0;

	/** Private Variables */
	private GameState gameState;

	private bool DEBUG = true;

	void Awake () {
		if (GameManager.instance == null)
			GameManager.instance = this;
		else if (GameManager.instance != this)
			Destroy (gameObject);

		InitializeGame ();
	}

	void InitializeGame() {
		// TODO start game variables and states - instantiate objects such as players, etc
		if (playerObject == null)
			playerObject = GameObject.FindGameObjectWithTag ("Player");

		changeState (GameState.PLAY);
		Screen.SetResolution (1280, 720, false);
	}

	void Update () {
		
	}

	void FixedUpdate() {
		if (cristalsCollected >= 4) {
			//TODO Completed Game!
			changeState(GameState.WIN);
		}
	}

	public void changeState(GameState next_state) {
		if (isFinalState ()) {
			if (DEBUG)
				Debug.Log ("GAMEMANAGER: cannot change state. final state reached");
			return;
		}
		gameState = next_state;
		switch (next_state) {
		case GameState.PLAY:
			gameUI.SetActive (true);
			pauseUI.SetActive (false);
			gameOverUI.SetActive (false);
			winUI.SetActive (false);
			break;
		case GameState.PAUSE:
			gameUI.SetActive (false);
			pauseUI.SetActive (true);
			gameOverUI.SetActive (false);
			winUI.SetActive (false);
			break;
		case GameState.GAMEOVER:
			gameUI.SetActive (false);
			pauseUI.SetActive (false);
			gameOverUI.SetActive (true);
			winUI.SetActive (false);
			break;
		case GameState.WIN:
			gameUI.SetActive (false);
			pauseUI.SetActive (false);
			gameOverUI.SetActive (false);
			winUI.SetActive (true);	
			break;
		}
		if (DEBUG)
			Debug.Log ("GAMEMANAGER: changed state to: " + next_state);
	}

	public P getPlayer<P> (P Component) {
		return playerObject.GetComponent<P>();
	}

	public void exitGame() {
		Application.Quit ();
	}

	public bool isPaused() {
		if (gameState == GameState.PAUSE) return true;
		// put here the states which the game will be paused
		return false;
	}

	public bool isGameOver() {
		return gameState == GameState.GAMEOVER || gameState == GameState.WIN;
	}

	private bool isFinalState() {
		if (isGameOver()) return true;
		// put here the states which the game will not change state anymore
		return false;
	}
}

/**
 * Put here your game states
 * */
public enum GameState {
	PLAY,
	PAUSE,
	GAMEOVER,
	WIN
}
