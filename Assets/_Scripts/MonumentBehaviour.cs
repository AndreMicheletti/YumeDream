using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonumentBehaviour : MonoBehaviour {

	public SpriteRenderer crystalsRender;
	public float[] WidthForCrystalCount;

	private bool playerIsHere = false;

	// Update is called once per frame
	void FixedUpdate () {
		if (crystalsRender == null)
			return;

		crystalsRender.size = 
			new Vector2 (WidthForCrystalCount [GameManager.instance.cristalsCollected], crystalsRender.size.y);

		/*if (Input.GetKeyDown (KeyCode.F))
			GameManager.instance.cristalsCollected += 1;*/
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player")
			playerIsHere = true;
	}
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Player")
			playerIsHere = false;
	}
		
}
