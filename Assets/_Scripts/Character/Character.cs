using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

	public float speed;

	public SpriteRenderer render;
	public Animator animator;

	public int maxHitPoints = 10;

	public int attackRecover = 60;

	protected int recoverTimer = 0;

	protected Rigidbody2D body;

	protected CharacterState characterState;
	protected CombatState combatState;
	protected Direction dir;

	protected int hitPoints = 0;
	protected bool attacking = false;

	protected float DIAGONAL_SLOWDOWN = 1.3f;
	protected float LINEAR_DRAG = 35f;

	void Start() {
		body = GetComponent<Rigidbody2D> ();
		//body.drag = LINEAR_DRAG;
		characterState = CharacterState.IDLE;
		combatState = CombatState.IDLE;
		dir = Direction.DOWN;
		hitPoints = maxHitPoints;
		InitializeCharacter ();
	}

	void FixedUpdate() {
		if (GameManager.instance.isGameOver ()) {
			animator.SetFloat ("MoveX", 0);
			animator.SetFloat ("MoveY", 0);
			animator.SetBool ("Moving", false);
			animator.SetBool ("Dead", true);
			updateWhenGameOver ();
			return;
		}
		if (GameManager.instance.isGameOver () || GameManager.instance.isPaused ())
			return;
		updateOrderInLayer ();
		updateCharacter ();
	}

	public int getHitPoints() {
		return hitPoints;
	}
		
	protected abstract void updateWhenGameOver ();

	protected abstract void updateCharacter ();

	protected abstract void Attack ();

	protected abstract void Dying ();
	public abstract void Die ();

	protected void updateTimers() {
		if (recoverTimer > 0)
			recoverTimer -= 1;
	}

	public virtual void takeHit (int damage) {
		hitPoints -= damage;
		if (hitPoints <= 0) {
			animator.SetTrigger ("Die");
			hitPoints = 0;
			Dying ();
		} else
			animator.SetTrigger ("Hit");
	}

	protected void updateOrderInLayer() {
		render.sortingOrder = (int) (transform.position.y * -1.0f);
		/*Vector3 position = transform.position;
		position.z = position.y;
		transform.position = position;*/
	}

	protected virtual void InitializeCharacter() {
		
	}

	protected void Move(float horizontal, float vertical) {
		if (GameManager.instance.isGameOver () || GameManager.instance.isPaused ())
			return;
		if (characterState == CharacterState.DEAD)
			return;

		if (Mathf.Abs (horizontal) > 0 && Mathf.Abs (vertical) > 0) {
			horizontal /= DIAGONAL_SLOWDOWN;
			vertical /= DIAGONAL_SLOWDOWN;
		}

		//body.AddForce (new Vector2 (horizontal * speed, vertical * speed), ForceMode2D.Impulse);
		body.velocity = new Vector2 (horizontal * speed, vertical * speed);
		/*Vector2 target = new Vector2(transform.position.x + (horizontal * speed), transform.position.y + (vertical * speed));
		body.MovePosition (target);*/

		resolveDirection ();
	}

	protected bool MoveTo(Vector2 target) {

		if (Vector2.Distance (target, new Vector2 (transform.position.x, transform.position.y)) < 0.2f) {
			return true;
		} else {

			Vector2 towards = Vector2.MoveTowards (target, new Vector2 (transform.position.x, transform.position.y), 0.1f);
			Vector2 diff = towards - new Vector2 (transform.position.x, transform.position.y);

			float velX = Mathf.Clamp (diff.x, -speed, speed);
			float velY = Mathf.Clamp (diff.y, -speed, speed);

			Debug.Log ("diff: " + diff);

			if (Mathf.Abs (velX) > 0 && Mathf.Abs (velY) > 0) {
				velX /= DIAGONAL_SLOWDOWN;
				velY /= DIAGONAL_SLOWDOWN;
			}
				
			body.AddForce (new Vector2 (velX, velY), ForceMode2D.Impulse);

			resolveDirection ();
			return false;
		}
	}

	protected virtual void resolveDirection() {
		float x = body.velocity.x;
		float y = body.velocity.y;

		animator.SetFloat ("MoveX", x);
		animator.SetFloat ("MoveY", y);

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

	protected virtual void resolveIdle() {
		switch (dir) {
		case Direction.DOWN:
			animator.SetFloat ("Direction", 2f);
			animator.SetFloat ("MoveX", 0);
			animator.SetFloat ("MoveY", -1);
			break;
			/*
		case Direction.LEFT:
			animator.SetFloat ("Direction", 4f);
			animator.SetFloat ("MoveX", -1);
			animator.SetFloat ("MoveY", 0);
			break;
		case Direction.RIGHT:
			animator.SetFloat ("Direction", 6f);
			animator.SetFloat ("MoveX", 1);
			animator.SetFloat ("MoveY", 0);
			break;
			*/
		case Direction.UP:
			animator.SetFloat ("MoveX", 0);
			animator.SetFloat ("MoveY", 1);
			break;
		}
	}

	protected bool isAttacking() {
		return attacking;
	}

	protected void disableAttacking() {
		attacking = false;
	}

	public bool isDead() {
		if (characterState == CharacterState.DEAD)
			return true;
		return false;
	}

	protected void changeState(CharacterState cs) {
		characterState = cs;
	}

}
