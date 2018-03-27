using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	public float speed;
	public int damage;
	public bool playerEnabled;
	public SpriteRenderer[] bodySprites;
	public GameObject selectedObj;

	private bool playerPositionLocked;
	private EnemyDetector enemyDetector;

	private Transform enemyParent;
	private bool enemyInPosition;
	private GameObject enemyInPosObj = null;

	private Animator anim, swordAnim;
	private Vector2 lastPos;

	void Start () {
		anim = GetComponent<Animator> ();
		swordAnim = GetComponentInChildren<Animator> ();
		enemyDetector = GameObject.FindObjectOfType<EnemyDetector> ();
		SetPlayerActive (true);
	}

	void Update () {
		selectedObj = EventSystem.current.currentSelectedGameObject;

		if (playerEnabled) {
			if (!selectedObj || selectedObj.name == "CloseButton") {
				if (Input.GetButtonDown ("Fire1")) {
					MusicController.instance.PlaySoundEffect (MusicController.instance.swordSwing);
					anim.Play ("Slash");
					swordAnim.Play ("Slash");

					if (enemyDetector.EnemiesInRange ()) {
						MusicController.instance.PlaySoundEffect (MusicController.instance.swordHit);

						foreach (GameObject enemyInRange in enemyDetector.enemiesInRadius) {
							enemyInRange.GetComponent<Health> ().TakeDamage (damage);
						}
					}
				}
			}

				if ((Input.GetButtonDown ("Horizontal") || Input.GetButtonDown ("Vertical")) && !playerPositionLocked) {
					lastPos = transform.position;
					playerPositionLocked = true;

					float h = Input.GetAxisRaw ("Horizontal");
					float v = Input.GetAxisRaw ("Vertical");

					Vector3 newPos = lastPos + new Vector2 (h, v);

					Transform enemyParent = GameObject.Find ("Enemies").transform;

					foreach (Transform enemy in enemyParent) {
						if (enemy.position == newPos) {
							enemyInPosition = true;
							enemyInPosObj = enemy.gameObject;
							break;
						}

						enemyInPosition = false;
					}
						
					if (enemyInPosition) {
						GetComponent<Health> ().TakeDamage (enemyInPosObj.GetComponent<DamagePlayer> ().damage);

						if (enemyInPosObj.tag == "BomberEnemy") {
							StartCoroutine (enemyInPosObj.GetComponent<Enemy> ().DelayedAttack ());
						} else {
							enemyInPosObj.GetComponent<Animator> ().Play ("Attack");
						}

						enemyInPosition = false;
					} else {
						transform.position = newPos;

						float clampedPosX = Mathf.Clamp (transform.position.x, 1f, 7f);
						float clampedPosY = Mathf.Clamp (transform.position.y, 1f, 4f);

						transform.position = new Vector2 (clampedPosX, clampedPosY);

						if (h != 0) {
							transform.localScale = new Vector2 (h, transform.localScale.y);
						}
					}
				}
				
				if (Input.GetButtonUp ("Horizontal") || Input.GetButtonUp ("Vertical")) {
					playerPositionLocked = false;
				}
		}
	}

	public void SetPlayerActive(bool active){
		playerEnabled = active;
		foreach (SpriteRenderer sprite in bodySprites) {
			sprite.enabled = active;
		}
	}
}
