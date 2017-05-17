using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour {

	public static FXManager instance;

	public AudioSource MusicSource;
	public AudioSource AudioSource1;
	public AudioSource AudioSource2;
	public AudioSource AudioSource3;
	//public AudioSource AudioSource4;
	//public AudioSource AudioSource5;

	public float distanceToHear = 5f;

	public AudioClip[] clips;

	void Start() {
		instance = this;
	}

	public void playMusic(AudioClip clip) {
		MusicSource.Stop ();
		MusicSource.clip = clip;
		MusicSource.Play ();
	}

	public void play(AudioClip clip) {
		if (clip == null)
			return;
		
		if (AudioSource1.isPlaying == false) {
			AudioSource1.clip = clip;
			AudioSource1.Play ();
			return;
		}
		if (AudioSource2.isPlaying == false) {
			AudioSource2.clip = clip;
			AudioSource2.Play ();
			return;
		}
		if (AudioSource3.isPlaying == false) {
			AudioSource3.clip = clip;
			AudioSource3.Play ();
			return;
		}
		/*if (AudioSource4.isPlaying == false) {
			AudioSource4.clip = clip;
			AudioSource4.Play ();
			return;
		}
		if (AudioSource5.isPlaying == false) {
			AudioSource5.clip = clip;
			AudioSource5.Play ();
			return;
		}*/
	}

	public void play(int index) {
		if (AudioSource1.isPlaying == false) {
			AudioSource1.clip = clips [index];
			AudioSource1.Play ();
			return;
		}
		if (AudioSource2.isPlaying == false) {
			AudioSource2.clip = clips [index];
			AudioSource2.Play ();
			return;
		}
		if (AudioSource3.isPlaying == false) {
			AudioSource3.clip = clips [index];
			AudioSource3.Play ();
			return;
		}
	}
}
