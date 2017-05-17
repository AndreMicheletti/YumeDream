using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTrigger : MonoBehaviour {

	public CameraScroller scroller;
	public string targetTag = "Player";
	public bool horizontalScroll = false;
	public bool verticalScroll = false;

	private bool active = true;

	void trigger(Transform other) {
		if (scroller == null)
			return;
		if (active == false)
			return;

		if (horizontalScroll) {
			if (other.position.x > transform.position.x) {
				scroller.scroll (-1, 0);
			} else {
				scroller.scroll (1, 0);
			}
		} else if (verticalScroll) {
			if (other.position.y > transform.position.y) {
				scroller.scroll (0, -1);
			} else {
				scroller.scroll (0, 1);
			}
		}
		//Invoke ("resetScale", 1f);
		//active = false;
	}

	void resetScale() {
		transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == targetTag) {
			trigger (other.gameObject.transform);
		}
	}
	/*void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == targetTag) {
			active = true;
		}
	}*/
}
