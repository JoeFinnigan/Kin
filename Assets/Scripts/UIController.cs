using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {
	public Text killCountText;
	public Text timerText;
	public Image healthBar, wallHealthBar;

	public GameObject statsPanel, highScorePanel;
	public Animator menusAnim;
	public Button kinButton;
	public Button statsConfirmButton, statsCloseButton;
	public Text skillPointsText, strengthText, constitutionText, regenerationText;

	private PlayerController player;
	public Health wall;
	private PlayerStats playerStats;
	private GameController gameController;
	private EnemySpawner enemySpawner;

	void Start(){
		player = GameObject.FindObjectOfType<PlayerController> ();
		playerStats = GameObject.FindObjectOfType<PlayerStats> ();
		gameController = GameObject.FindObjectOfType<GameController> ();
		enemySpawner = GameObject.FindObjectOfType<EnemySpawner> ();
	}

	void Update(){
		UpdateTimer ();
		UpdatePlayerHealthBar ();
		UpdateWallHealthBar ();

		if (statsPanel.activeSelf) {
			skillPointsText.text = playerStats.skillPoints.ToString ();
			strengthText.text = playerStats.strengthLevel.ToString ();
			constitutionText.text = playerStats.constitutionLevel.ToString ();
			regenerationText.text = playerStats.regenerationLevel.ToString ();

			if (playerStats.skillPoints == 0) {
				statsConfirmButton.interactable = true;
				statsCloseButton.interactable = true;
			} 
		}
	}

	private void UpdateTimer(){
		int gameTime = (int)Mathf.RoundToInt (gameController.currentTime);
		int minutes = (int)(gameTime / 60) % 60;
		int seconds = (int)gameTime % 60;
		timerText.text = string.Format ("{00:00}:{01:00}", minutes, seconds);

		if (gameTime >= gameController.lastKinTime) {
			timerText.color = Color.green;
		} else {
			timerText.color = Color.red;
		}
	}

	private void UpdatePlayerHealthBar(){
		//TODO Conversion will need to be changed when starting health of player is updated or altered later on
		float playerHealth = player.GetComponent<Health> ().currentHealth;
		float healthToWidthConversion = 5f;
		healthBar.rectTransform.sizeDelta = new Vector2 (playerHealth * healthToWidthConversion, healthBar.rectTransform.sizeDelta.y);
	}

	private void UpdateWallHealthBar(){
		float wallHealth = wall.currentHealth;
		wallHealthBar.rectTransform.sizeDelta = new Vector2 (wallHealthBar.rectTransform.sizeDelta.x, wallHealth * 7.5f);
	}

	public void UpdateScoreText(int killCount){
		killCountText.text = "Kills: " + killCount;
	}

	public void ShowGameOverScreen(bool kinAvailable){
		kinButton.interactable = kinAvailable;

		if (!kinAvailable) {
			//Future fix - check if score is in top 100.
			highScorePanel.transform.Find ("Header").GetComponent<Text> ().text = "New High Score: " + gameController.score;
		}

		highScorePanel.SetActive (!kinAvailable);

		PauseGame ();
		menusAnim.Play ("GameOver_FadeIn");
	}

	public void HideGameOverScreen(){
		menusAnim.Play ("GameOver_FadeOut");
	}

	public void OpenInGameMenu(){
		PauseGame ();
		menusAnim.Play ("Menu_FadeIn");
	}

	public void CloseInGameMenu(){
		ResumeGame();
		menusAnim.Play ("Menu_FadeOut");
	}

	public void OpenInGameStats(){
		PauseGame ();
		menusAnim.Play ("Stats_FadeIn");

		if (playerStats.skillPoints > 0) {
			statsConfirmButton.interactable = false;
			statsCloseButton.interactable = false;
		} else {
			statsConfirmButton.interactable = true;
			statsCloseButton.interactable = true;
		}
	}

	public void CloseInGameStats(){
		menusAnim.Play ("Stats_FadeOut");
		ResumeGame ();

		if (gameController.kinAvailable && gameController.playerLost) {
			gameController.StartGameWithNewKin ();
		}
	}

	public void CloseHighScoreScreen(bool highScoreSet){
		highScorePanel.SetActive (false);

		if (highScoreSet) {
			string name = highScorePanel.transform.Find ("NameEntry/NameText").GetComponent<Text> ().text;
			Leaderboard.instance.UploadScore (name, gameController.score);
		}
	}

	private void PauseGame(){
		Time.timeScale = 0;
		player.SetPlayerActive (false);

		enemySpawner.ToggleEnemyVisibility (false);
	}

	private void ResumeGame(){
		Time.timeScale = 1;
		player.SetPlayerActive (true);

		enemySpawner.ToggleEnemyVisibility (true);
	}

	public void ExitGame(){
		SceneManager.LoadScene ("MainMenu");
	}
}
