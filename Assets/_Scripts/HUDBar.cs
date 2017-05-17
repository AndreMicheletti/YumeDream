using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUDBar : MonoBehaviour {

	public Image thisImage;
	public int state = 0;
	public float[] stateFill;
	
	// Update is called once per frame
	void FixedUpdate () {
		thisImage.fillAmount = stateFill [state];
	}
}
