using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour {
	public int damage;

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag == "Player" || collider.tag == "Wall") {
			collider.gameObject.GetComponent<Health> ().TakeDamage (damage);
		}
	}
}
