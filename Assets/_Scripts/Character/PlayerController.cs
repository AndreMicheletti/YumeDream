using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character {

	public Animator swordAnimator;
	public SpriteRenderer swordRender;
	public Transform swordTransform;
	public float healTimeInSeconds = 2f;
	private float lastY;

	protected override void InitializeCharacter() {
		Heal ();
		dir = Direction.DOWN;
		resolveIdle ();
		resolveDirection ();
	}

	void Update () {
		if (isDead ())
			return;
		
		handleInput ();
	}

	protected override void updateCharacter () {
		updateSwordCollider ();
		updateTimers ();
	}	

	protected override void updateWhenGameOver () {
		swordTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		swordTransform.localPosition = new Vector3 (0f, 0f, 0f);
	}

	void updateSwordCollider() {
		if (!isAttacking ()) {
			swordTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			return;
		}
		swordTransform.localScale = new Vector3 (1, 1, 1);
		switch (dir) {
		case Direction.LEFT:
			swordTransform.localPosition = new Vector3 (-0.108f, -0.096f, 0f);
			swordTransform.localRotation = Quaternion.Euler (0, 0, 90f);
			swordTransform.localScale = new Vector3 (-1, 1, 1);
			swordRender.sortingOrder = render.sortingOrder - 1;
			break;
		case Direction.RIGHT:
			swordTransform.localPosition = new Vector3 (0.139f, -0.064f, 0f);
			swordTransform.localRotation = Quaternion.Euler (0, 0, -90f);
			swordRender.sortingOrder = render.sortingOrder + 1;
			break;
		case Direction.UP:
			swordTransform.localPosition = new Vector3 (0.101f, 0.113f, 0f);
			swordTransform.localRotation = Quaternion.Euler (0, 0, 0);
			swordRender.sortingOrder = render.sortingOrder - 1;
			break;
		case Direction.DOWN:
			swordTransform.localPosition = new Vector3 (-0.261f, -0.51f, 0f);
			swordTransform.localRotation = Quaternion.Euler (0, 0, 180f);
			swordRender.sortingOrder = render.sortingOrder + 1;
			break;
		}
	}

	void Heal() {
		if (characterState == CharacterState.DEAD)
			return;

		if (hitPoints < maxHitPoints)
			hitPoints += 1;

		Invoke ("Heal", healTimeInSeconds);
	}

	public override void takeHit (int damage) {
		hitPoints -= damage;
		CancelInvoke ("Heal");
		Invoke ("Heal", healTimeInSeconds + 1f);
		if (hitPoints <= 0) {
			animator.SetTrigger ("Die");
			hitPoints = 0;
			Dying ();
		} else
			animator.SetTrigger ("Hit");
	}

	protected override void Dying () {
		changeState (CharacterState.DEAD);
	}

	public override void Die () {
		Destroy (gameObject);
		GameManager.instance.changeState (GameState.GAMEOVER);
	}

	void handleInput() {

		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical =   Input.GetAxisRaw ("Vertical");

		if (horizontal != 0 && vertical != 0) {
			vertical = 0;
		}

		Move (horizontal, vertical);

		float attack = Input.GetAxisRaw ("Fire1");
		if (attack == 1 && recoverTimer == 0) {
			Attack ();
		}
	}

	protected override void Attack() {
		recoverTimer = attackRecover;
		swordAnimator.SetTrigger ("Attack");
		attacking = true;
		Invoke ("disableAttacking", 0.49f);
	}

	protected override void resolveDirection() {
		float x = body.velocity.x;
		float y = body.velocity.y;

		animator.SetFloat ("MoveX", lastY);
		if (y == 0) {
			animator.SetFloat ("MoveY", lastY);
		} else {
			animator.SetFloat ("MoveY", y);
			lastY = y;
		}

		if (x == 0 && y == 0) {
			animator.SetBool ("Moving", false);
			resolveIdle ();
			return;
		}

		animator.SetBool ("Moving", true);

		if (Mathf.Abs (x) > Mathf.Abs (y)) {
			//dir = (x > 0 ? dir = Direction.RIGHT : dir = Direction.LEFT);
		} else {
			dir = (y > 0 ? dir = Direction.UP : dir = Direction.DOWN);
		}
	}

}

public enum CharacterState {
	IDLE,
	MOVING,
	DEAD
}

public enum CombatState {
	IDLE,
	ATTACK,
	DEFEND
}

public enum Direction {
	UP, DOWN, LEFT, RIGHT
}
