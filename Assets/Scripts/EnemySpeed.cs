using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpeed : MonoBehaviour {
	public static EnemySpeed instance { get; private set; }

	public float enemySpeed, projectileSpeed, spawnTime, bomberSpeed;
	private float initialEnemySpeed, initialProjectileSpeed, initialSpawnTime, initialBomberSpeed;

	void Awake(){
		instance = this;
	}

	void Start(){
		initialEnemySpeed = enemySpeed;
		initialProjectileSpeed = projectileSpeed;
		initialSpawnTime = spawnTime;
		initialBomberSpeed = bomberSpeed;
	}

	public void IncreaseSpeed(){
		// Increase speed by 15% after each 'wave' of 30 seconds
		enemySpeed *= 0.85f;
		projectileSpeed *= 0.85f;
		spawnTime *= 0.85f;
		bomberSpeed *= 0.85f;
	}

	public void ResetSpeed(){
		// To be called when game over condition is met
		enemySpeed = initialEnemySpeed;
		projectileSpeed = initialProjectileSpeed;
		spawnTime = initialSpawnTime;
		bomberSpeed = initialBomberSpeed;
	}
}
