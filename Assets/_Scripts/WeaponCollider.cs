using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour {

	public string target;
	public int damage = 1;
	public float PushbackForce = 2f;
	public float recoverTime = 0.5f;

	private bool recover = false;

	void OnTriggerStay2D(Collider2D other) {
		if (other.tag != target)
			return;
		
		Character c = other.gameObject.GetComponent<Character> ();
		
		if (c != null && recover == false) {
			c.takeHit (damage);
			Pushback (other);
		}
	}

	void Pushback(Collider2D other) {
		Rigidbody2D body = other.gameObject.GetComponent<Rigidbody2D> ();

		Vector3 diff = transform.position - other.transform.position;
		body.MovePosition ((Vector2) (other.gameObject.transform.position - (diff * PushbackForce)));
		//Debug.Log (diff * PushbackForce);

		recover = true;
		Invoke ("hitboxRecover", recoverTime);

		//body.AddForce ((Vector2) diff * PushbackForce, ForceMode2D.Impulse);
		body.velocity += (Vector2) diff * PushbackForce;
	}

	void hitboxRecover() {
		recover = false;
	}
}
