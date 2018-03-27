using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
	public float startHealth, currentHealth;
	public float regenTime;
	public int regenAmount;
	private Animator anim;

	private bool deathTrigger;
	private bool bomberExplosionTriggered;

	private GameController gameController;
	private GameObject player;

	void Start () {
		player = GameObject.Find ("Player");
		gameController = GameObject.FindObjectOfType<GameController> ();
		anim = GetComponent<Animator> ();
		currentHealth = startHealth;
		regenTime = 0;
		deathTrigger = false;
		bomberExplosionTriggered = false;
	}

	void Update () {
		if (tag == "Player") {
			// Regeneration per second
			regenTime += Time.deltaTime;

			if (regenTime >= 1) {
				currentHealth += regenAmount;
				currentHealth = Mathf.Clamp (currentHealth, 0, startHealth);
				regenTime = 0;
			}
		}
			
		if (currentHealth <= 0 && !deathTrigger) {
			deathTrigger = true;
			StartCoroutine (CharacterDead ());
		}
	}

	private IEnumerator CharacterDead(){
		if (tag == "Enemy") {
			GetComponent<Enemy> ().StopMovement ();
		}
		if (tag != "Wall") {
			foreach (Transform warningSquare in gameObject.transform) {
				if (warningSquare.name == "WarningSquare") {
					Destroy (warningSquare.gameObject);
				}
			}
				
			anim.Play ("Die");

			MusicController.instance.PlaySoundEffect (MusicController.instance.bloodSplat);
			yield return new WaitForSeconds (0.5f);
		} else {
			MusicController.instance.PlaySoundEffect (MusicController.instance.wallHit);
		}

		if (tag == "Player" || tag == "Wall") {
			gameController.GameOver ();
			deathTrigger = false;
			currentHealth = startHealth;

			if (tag == "Player") {
				GameObject.Find ("Wall").GetComponent<Health> ().currentHealth = GameObject.Find ("Wall").GetComponent<Health> ().startHealth;
				GameObject.Find ("Wall").GetComponent<Health> ().deathTrigger = false;
			} else {
				player.SetActive (true);
				player.GetComponent<Health> ().currentHealth = GameObject.Find ("Player").GetComponent<Health> ().startHealth;
				player.GetComponent<Health> ().deathTrigger = false;
				player.SetActive (false);
			}
		} else {
			Destroy (gameObject);
			gameController.UpdateKillCount ();
		}
	}

	public void TakeDamage(int damage){
		currentHealth -= damage;
		if (tag == "BomberEnemy" & !bomberExplosionTriggered) {
			bomberExplosionTriggered = true;
			StartCoroutine(GetComponent<Enemy>().DelayedAttack());
		}

		if (tag == "Player") {
			MusicController.instance.PlaySoundEffect (MusicController.instance.playerIsHit);
			Camera.main.GetComponentInChildren<CameraShake> ().shakeDuration = 0.25f;

		}
	}
}
