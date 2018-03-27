using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public int killCount, score;
	public int waveSeconds, currentWave;
	public int kinGeneration;
	public float currentTime, waveTime, lastKinTime;
	public bool playerLost, kinAvailable;

	private UIController uiController;
	private CameraShake cameraShake;
	private PlayerController player;
	private PlayerStats playerStats;

	void Start () {
		cameraShake = GameObject.FindObjectOfType<CameraShake> ();
		uiController = GameObject.FindObjectOfType<UIController> ();
		player = GameObject.FindObjectOfType<PlayerController> ();
		playerStats = GameObject.FindObjectOfType<PlayerStats> ();

		killCount = 0;
		kinGeneration = 1;
		kinAvailable = true;
		Time.timeScale = 1;
	}

	private void Update(){
		currentTime += Time.deltaTime;
		waveTime += Time.deltaTime;

		// Enemy 'wave' is incremented every 30 secs, increasing speed & spawn rate
		if (waveTime >= 30) {
			EndOfWave ();
		}
	}

	private void EndOfWave(){
		currentWave++;

		EnemySpeed.instance.IncreaseSpeed ();
		waveTime = 0;
	}

	public void UpdateKillCount(){
		killCount++;
		uiController.UpdateScoreText (killCount);
	}

	private void ResetGameGrid(){
		// Resetting player position
		player.gameObject.SetActive(false);
		player.transform.position = new Vector2 (1, 3);

		// Destroying enemies
		GameObject.FindObjectOfType<EnemySpawner> ().DestroyAllEnemies ();

		// Restoring all enemy speeds to original values
		EnemySpeed.instance.ResetSpeed ();
	}

	public void GenerateKin(){
		// Increment generation/run number and allow player to increase their stats before the new run
		uiController.HideGameOverScreen ();
		uiController.OpenInGameStats ();
		kinGeneration++;
	}

	public void StartGameWithNewKin(){
		// Initialising stats to prepare for new run
		cameraShake.shakeAmount = 0.1f;
		kinAvailable = false;
		player.gameObject.SetActive (true);
		currentTime = 0;
		currentWave = 1;
		playerLost = false;
	}

	public void GameOver(){
		// Calculate score for this particular run and add it to the existing score
		int waveMultiplier = 1 + (1 / currentWave);
		score += (int)currentTime * waveMultiplier;

		// Compare score & time to current (local) high score & time
		if (score >= PlayerPrefController.GetHighScore ()) {
			PlayerPrefController.SetHighScore (score);
		}

		if (Time.timeSinceLevelLoad > PlayerPrefController.GetBestTime ()) {
			PlayerPrefController.SetBestTime ((int)Time.timeSinceLevelLoad);
		}

		playerLost = true;

		// If the current run time was better than the last attempt (or it is run #1), player can continue
		if (kinGeneration > 1 && currentTime < lastKinTime) {
			kinAvailable = false;
		} else {
			kinAvailable = true;
		}

		lastKinTime = currentTime;

		// Fixing bug with camera getting stuck mid-shake
		if (cameraShake.shakeAmount > 0) {
			cameraShake.shakeAmount = 0;
			//cameraShake.ShakeCamera ();
		}

		playerStats.skillPoints = currentWave - 1;

		ResetGameGrid ();
		uiController.ShowGameOverScreen (kinAvailable);
	}
}