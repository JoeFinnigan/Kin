using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
	public int strengthLevel, constitutionLevel, regenerationLevel;
	public int skillPoints;

	private PlayerController player;

	private void Start(){
		player = GameObject.FindObjectOfType<PlayerController> ();

		strengthLevel = 0;
		constitutionLevel = 0;
		regenerationLevel = 0;
	}

	public void IncreaseStrength(){
		int increaseAmount = 0;

		if (skillPoints > 0) {
			strengthLevel++;
			//currentStrength = strength [strengthLevel];
			skillPoints--;

			if (strengthLevel >= 1 && strengthLevel <= 10) {
				increaseAmount = 1;
			} else if (strengthLevel > 10) {
				increaseAmount = 5;
			}

			player.damage += increaseAmount;
		}
	}

	public void IncreaseConstitution(){
		int increaseAmount = 0;

		if (skillPoints > 0) {
			constitutionLevel++;
			//currentConstitution = constitution [constitutionLevel];
			skillPoints--;

			if (constitutionLevel >= 1 && constitutionLevel <= 10) {
				increaseAmount = 1;
			} else if (constitutionLevel > 10) {
				increaseAmount = 5;
			}

			player.GetComponent<Health> ().currentHealth += increaseAmount;
			player.GetComponent<Health> ().startHealth += increaseAmount;
		}
	}

	public void IncreaseRegeneration(){
		int increaseAmount = 0;

		if (skillPoints > 0) {
			regenerationLevel++;
			//currentRegeneration = regeneration [regenerationLevel];
			skillPoints--;

			if (regenerationLevel >= 1 && regenerationLevel <= 10) {
				increaseAmount = 1;
			} else if (regenerationLevel > 10) {
				increaseAmount = 5;
			}

			player.GetComponent<Health> ().regenAmount += increaseAmount;
		}
	}
}
