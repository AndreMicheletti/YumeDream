using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour {

	public Character character;
	public AudioClip moveSound;

	public void finishedDieAnim() {
		character.Die ();
	}

	public void playSound() {
		if (moveSound == null)
			return;
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		Vector2 playerPos = (Vector2) player.transform.position;
		Vector2 myPos = (Vector2) transform.position;
		if (Vector2.Distance (playerPos, myPos) < FXManager.instance.distanceToHear) {
			Debug.Log ("I'm CLOSE!");
			FXManager.instance.play (moveSound);
		}
	}
}
