using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour {
	public List<GameObject> enemiesInRadius = new List<GameObject> ();

	// Build a list of enemies currently in the vicinity of the player - removing them as they leave the trigger.
	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag == "RangedEnemy" || collider.tag == "MeleeEnemy" || collider.tag == "BomberEnemy") {
			enemiesInRadius.Add(collider.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D collider){
		if (collider.tag == "RangedEnemy" || collider.tag == "MeleeEnemy" || collider.tag == "BomberEnemy") {
			enemiesInRadius.Remove(collider.gameObject);
		}
	}

	// Check if there are enemies currently in radius
	public bool EnemiesInRange(){
		if (enemiesInRadius.Count > 0) {
			return true;
		} else {
			return false;
		}
	}
}
