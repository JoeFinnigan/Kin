using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spriter2UnityDX;

public class Enemy : MonoBehaviour {
	private Rigidbody2D rb;
	private float moveWaitTime, delayWaitTime;
	public float chargeTime, chargeCounter;

	public int stopPosX;
	public SpriteRenderer[] bodySprites;

	private bool isMoving;
	private GameObject player, wall, currentTarget;
	private Animator anim;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		wall = GameObject.FindGameObjectWithTag ("Wall");
		isMoving = true;
		chargeTime = 3f;
		chargeCounter = 0f;

		moveWaitTime = EnemySpeed.instance.enemySpeed;

		if (this.tag == "RangedEnemy") {
			InvokeRepeating ("RangedAttack", Random.Range (1, 3), Random.Range (5, 7));
		} else if (this.tag == "BomberEnemy") {
			moveWaitTime = EnemySpeed.instance.bomberSpeed;
		}
	}

	void Update () {
		// Reset sprite & counter after an attack
		if (chargeCounter >= chargeTime) {
			GetComponent<EntityRenderer> ().Color = Color.white;
			chargeCounter = 0;
		}

		// When enemy reaches its destination, stop moving and start attacking the wall
		if (transform.position.x <= stopPosX) {
			transform.position = new Vector2 (stopPosX, transform.position.y);
			StopMovement ();

			if (this.tag == "RangedEnemy") {
				InvokeRepeating ("Attack", 0.001f, 3f);
			}

			if (this.tag == "BomberEnemy") {
				delayWaitTime += Time.deltaTime;

				if (delayWaitTime >= chargeTime && this.enabled) {
					StartCoroutine (DelayedAttack ());
					delayWaitTime = 0;
				}
			}
		}
	}

	void FixedUpdate(){
		if (isMoving) {
			moveWaitTime -= Time.deltaTime;

			if (moveWaitTime <= 0) {
				Vector3 newPos = new Vector3 (transform.position.x - 1, transform.position.y);

				// Give player damage if walking into them - otherwise move there
				if (newPos == player.transform.position) {
					player.GetComponent<Health>().TakeDamage (GetComponent<DamagePlayer> ().damage);
				} else {
					rb.MovePosition(newPos);
				}

				moveWaitTime = EnemySpeed.instance.enemySpeed;
			}
		}
	}

	public void StopMovement(){
		isMoving = false;
	}

	private void ChargeUpAttack(float chargeTime){
		StartCoroutine (BlinkSprite ());
	}

	private IEnumerator BlinkSprite(){
		// Blink sprite red for a fixed amount of time
		for (int i = 0; i < 5; i++){
			yield return new WaitForSeconds (0.3f);
			GetComponent<EntityRenderer> ().Color = Color.red;
			yield return new WaitForSeconds (0.3f);
			GetComponent<EntityRenderer> ().Color = Color.white;
		}
	}

	void Attack(){
		StopMovement ();
		anim.Play ("Attack");

		// This should probably be changed - wall should take damage via OnTriggerEnter
		if (currentTarget == wall && this.tag != "BomberEnemy") {
			wall.GetComponent<Health> ().TakeDamage (GetComponent<DamagePlayer> ().damage);
		}

		StartCoroutine (CooldownPeriod ());
	}

	private void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag == "Player" || collider.tag == "Wall"){
			if (this.tag == "MeleeEnemy"){
				currentTarget = collider.gameObject;
				InvokeRepeating ("Attack", 0f, 3f);
			}
		}
	}

	private void OnTriggerExit2D (Collider2D collider){
		if (collider.tag == "Player" || collider.tag == "Wall"){
			if (this.tag == "MeleeEnemy") {
				CancelInvoke ("Attack");
			}
		}
	}

	public IEnumerator DelayedAttack(){
		StopMovement();
		ChargeUpAttack (chargeTime);
		yield return new WaitForSeconds(chargeTime);

		var fireAttack = Resources.Load("Fireball");

		if (this.tag == "BomberEnemy") {
			fireAttack = Resources.Load ("RingOfFire");
			MusicController.instance.PlaySoundEffect (MusicController.instance.explosionSound);
			anim.Play ("Die");
		} else {
			anim.Play ("Attack");
		}

		Instantiate (fireAttack, transform, false);
		GetComponent<EntityRenderer> ().Color = Color.white;
		StartCoroutine (CooldownPeriod ());
	}

	private void RangedAttack(){
		StartCoroutine (DelayedAttack ());
	}

	private IEnumerator CooldownPeriod(){
		yield return new WaitForSeconds (1.5f);

		if (this.tag == "BomberEnemy") {
			Destroy (gameObject);
		}

		isMoving = true;
	}
}