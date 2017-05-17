using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Game : MonoBehaviour {

	public PlayerController player;

	public HUDBar healthBar;
	public HUDBar crystalBar;

	void Awake() {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		updateHealthBar ();
		updateCrystalBar ();
	}

	void updateHealthBar() {
		int current = player.getHitPoints ();
		healthBar.state = current;
	}
	void updateCrystalBar() {
		crystalBar.state = GameManager.instance.cristalsCollected;
	}
}
