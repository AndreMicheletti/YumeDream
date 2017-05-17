using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestBehaviour : MonoBehaviour {

	public Sprite openChestSprite;
	public Sprite closedChestSprite;
	public AudioSource playThisOnOpen;
	public AudioClip openSFX;
	public GameObject helpText;

	public KeyCode triggerKey = KeyCode.E;
	public float distanceToOpen = 1f;

	[HideInInspector]
	public int enemiesLeft = 0;

	public GameObject player;

	private SpriteRenderer render;
	private Animator animator;
	private bool open = false;

	// Use this for initialization
	void Start () {
		render = GetComponent<SpriteRenderer> ();	
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (GameManager.instance.isPaused ())
			return;

		//helpText.SetActive (DistanceToPlayer () <= distanceToOpen);

		if (open)
			return;

		if (DistanceToPlayer () <= distanceToOpen) {
			if (enemiesLeft == 0 && Input.GetKeyDown (triggerKey)) {
				Open ();
			}
		}
	}

	float DistanceToPlayer() {
		Vector2 targetPos = (Vector2) player.transform.position;
		Vector2 myPos = (Vector2) transform.position;
		return Vector2.Distance (targetPos, myPos);
	}

	void Open() {
		render.sprite = openChestSprite;
		GameManager.instance.cristalsCollected += 1;
		open = true;
		playThisOnOpen.Play ();
		animator.SetTrigger ("Open");	
		FXManager.instance.play (openSFX);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			helpText.SetActive (true);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			helpText.SetActive (false);
		}
	}
}
