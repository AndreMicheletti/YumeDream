using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

	public ChestBehaviour chest;
	public Character target;
	public float followDistance = 5f;
	public float attackDistance = 0.8f;

	public Transform hitboxCollider;

	private AIState enemyState = AIState.IDLE;

	private float moveX;
	private float moveY;


	protected override void InitializeCharacter() {
		chest.enemiesLeft += 1;
		changeAIState (AIState.MOVING_RANDOM);
	}

	protected override void updateCharacter ()
	{
		updateHitboxCollider ();
		if (isDead ())
			return;
		updateState ();
		updateTimers ();
		animator.SetBool ("Moving", (moveX != 0 || moveY != 0));

	}

	protected override void updateWhenGameOver () {
		hitboxCollider.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		hitboxCollider.localPosition = new Vector3(0f, 0f, 0f);
	}

	void updateState() {
		switch (enemyState) {
		case AIState.MOVING_RANDOM:
			Move (moveX, moveY);
			if (distanceToTarget() < followDistance) {
				changeAIState (AIState.ATTACKING);
				//Debug.Log ("CHARGEEEE!");
			}
			/* -- BACKUP
			if (movingTo != Vector2.zero) {
				if (MoveTo (movingTo) == true) {
					movingTo = Vector2.zero;
				}
			} else {
				float x = Random.Range (-speed * 2.0f, speed * 2.0f);
				float y = Random.Range (-speed * 2.0f, speed * 2.0f);
				movingTo = new Vector2 (x, y);
				Debug.Log ("Moving to: " + movingTo);
			}
			*/
			break;
		case AIState.WAITING:
			break;
		case AIState.ATTACKING:
			if (distanceToTarget() > followDistance) {
				changeAIState (AIState.MOVING_RANDOM);
			} else {
				updateAttackingState ();
			}
			break;
		}
	}

	protected override void resolveDirection() {}

	protected override void resolveIdle() {}

	void updateAttackingState() {
		if (distanceToTarget () < attackDistance) {
			if (recoverTimer == 0 && isAttacking () == false) {
				Attack ();
				//Debug.Log ("TAKE THIS!");
			}
		} else {
			Move (moveX, moveY);
		}
	}

	void updateHitboxCollider() {
		if (characterState == CharacterState.DEAD) {
			hitboxCollider.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
		} else {
			hitboxCollider.localScale = new Vector3(1f, 1f, 1f);
		}
		/*
		if (!isAttacking ()) {
			hitboxCollider.localPosition = new Vector3 (0f, 0f, 0f);
			hitboxCollider.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			return;
		}
		hitboxCollider.localScale = new Vector3 (1, 1, 1);
		switch (dir) {
		case Direction.LEFT:
			hitboxCollider.localPosition = new Vector3 (-0.5f, 0f, 0f);
			//hitboxCollider.localRotation = Quaternion.Euler (0, 0, 90f);
			//hitboxCollider.localScale = new Vector3 (-1, 1, 1);
			//swordRender.sortingOrder = render.sortingOrder - 1;
			break;
		case Direction.RIGHT:
			hitboxCollider.localPosition = new Vector3 (0.5f, 0f, 0f);
			//hitboxCollider.localRotation = Quaternion.Euler (0, 0, -90f);
			//swordRender.sortingOrder = render.sortingOrder + 1;
			break;
		case Direction.UP:
			hitboxCollider.localPosition = new Vector3 (0f, 0.7f, 0f);
			//hitboxCollider.localRotation = Quaternion.Euler (0, 0, 0);
			//swordRender.sortingOrder = render.sortingOrder - 1;
			break;
		case Direction.DOWN:
			hitboxCollider.localPosition = new Vector3 (0f, -0.7f, 0f);
			//hitboxCollider.localRotation = Quaternion.Euler (0, 0, 180f);
			//swordRender.sortingOrder = render.sortingOrder + 1;
			break;
		}
		*/
	}

	protected override void Attack () {
		attacking = true;
		recoverTimer = attackRecover;
		Invoke ("disableAttacking", 0.4f);
	}

	void changeAIState(AIState ai) {
		enemyState = ai;
		if (ai == AIState.MOVING_RANDOM)
			resolveRandomMove ();
		else if (ai == AIState.ATTACKING)
			resolveAttackMove ();
		else if (ai == AIState.WAITING)
			resolveWaiting ();
	}

	void resolveAttackMove() {
		if (enemyState != AIState.ATTACKING)
			return;
		if (GameManager.instance.isGameOver ())
			return;
		
		Vector2 targetPos = (Vector2) target.gameObject.transform.position;
		Vector2 myPos = (Vector2) transform.position;
		float x = targetPos.x - myPos.x;
		float y = targetPos.y - myPos.y;

		if (Mathf.Abs (x) > Mathf.Abs (y)) {
			moveX = (x > 0 ? speed : -speed);
			moveY = 0;
		} else {
			moveX = 0;
			moveY = (y > 0 ? speed : -speed);
		}
		Invoke ("resolveAttackMove", 1.2f);
	}

	void resolveRandomMove() {
		if (enemyState != AIState.MOVING_RANDOM)
			return;
		moveX = moveY = 0;

		if ((int) Random.Range(0,2) == 1)
			moveX = Random.Range (speed * -1.0f, speed * 1.0f);
		else
			moveY = Random.Range (speed * -1.0f, speed * 1.0f);

		if (Mathf.Abs (moveX) < 0.5f)
			moveX = 0f;
		if (Mathf.Abs (moveY) < 0.5f)
			moveY = 0f;

		Invoke ("resolveRandomMove", 2.2f);
	}

	void resolveWaiting() {
		if (enemyState != AIState.WAITING)
			return;

		moveX = 0;
		moveY = 0;

		Invoke ("startMovingRandom", 3f);
	}

	public void startMovingRandom() {
		changeAIState (AIState.MOVING_RANDOM);
	}

	float distanceToTarget() {
		Vector2 targetPos = (Vector2) target.gameObject.transform.position;
		Vector2 myPos = (Vector2) transform.position;
		return Vector2.Distance (targetPos, myPos);
	}

	public override void Die ()
	{
		chest.enemiesLeft -= 1;
		Destroy (gameObject);
	}

	protected override void Dying ()
	{
		changeState (CharacterState.DEAD);
		foreach (Collider2D coll in GetComponents<Collider2D>() ) {
			coll.enabled = false;
		}
	}
}

public enum AIState {
	IDLE,
	MOVING_RANDOM,
	WAITING,
	ATTACKING,
}