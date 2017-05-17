using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroller : MonoBehaviour {

	public Transform followThis;
	public float horizontalScroll = 16f;
	public float verticalScroll = 10f;
	public float maxDelta = 1f;

	public bool moving = false;

	private float targetX = 0;
	private float targetY = 0;

	public void scroll(int horizontal, int vertical) {
		targetX = transform.position.x - (horizontalScroll * horizontal);
		targetY = transform.position.y - (verticalScroll * vertical);
		StartCoroutine (scroll ());
		//canFollow = !canFollow;
	}

	IEnumerator scroll() {
		moving = true;
		while (transform.position.x != targetX || transform.position.y != targetY) {
			float moveX = Mathf.MoveTowards (transform.position.x, targetX, maxDelta);
			float moveY = Mathf.MoveTowards (transform.position.y, targetY, maxDelta);
			transform.position = new Vector3 (moveX, moveY);
			yield return new WaitForFixedUpdate ();
		}
		moving = false;
	}

	void FixedUpdate() {
		if (followThis != null && moving == false) {
			if (targetX != 0 || targetY != 0) {
				transform.position = new Vector2((targetX != 0 ? followThis.position.x : 0), (targetY != 0 ? followThis.position.y : 0));
				if (Mathf.Abs(transform.position.x) < Mathf.Abs (targetX) || Mathf.Abs(transform.position.y) < Mathf.Abs (targetY)) {
					transform.position = new Vector2 (targetX, targetY);
				}
			}
		}
	}
}
