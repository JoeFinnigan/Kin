using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour { 
	public GameObject[] spawnSpots, enemies;

	private Transform enemyParent;
	private float currentSpawnTime;

	void Start(){
		currentSpawnTime = EnemySpeed.instance.spawnTime;

		enemyParent = GameObject.Find("Enemies").transform;
		SpawnEnemy ();
	}

	void Update () {
		currentSpawnTime -= Time.deltaTime;

		if (currentSpawnTime <= 0f) {
			SpawnEnemy();
			currentSpawnTime = EnemySpeed.instance.spawnTime;
		}
	}

	private void SpawnEnemy(){
		int randomSpawnIndex = Random.Range (0, spawnSpots.Length);
		int randomEnemyIndex = Random.Range (0, enemies.Length);

		GameObject spawnedEnemy = Instantiate (enemies[randomEnemyIndex], spawnSpots [randomSpawnIndex].transform.position, Quaternion.identity) as GameObject;
		spawnedEnemy.transform.SetParent (enemyParent);
	}

	public void DestroyAllEnemies(){
		foreach (Transform enemy in enemyParent){
			Destroy(enemy.gameObject);
		}
	}

	public void ToggleEnemyVisibility(bool visible){
		foreach (Transform enemy in enemyParent) {
			foreach (SpriteRenderer sprite in enemy.GetComponent<Enemy>().bodySprites) {
				sprite.enabled = visible;
			}
		}
	}
}
