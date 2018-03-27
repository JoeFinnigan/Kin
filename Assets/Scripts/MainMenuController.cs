using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
	public Text scoreText, timeText, versionText;
	public GameObject tutorialMenu;

	public float volumeBeforeVoiceClip;
	private OptionsController optionsController;
	private int clipIndex;
	public AudioClip[] voiceClips;

	private void Awake(){
		if (PlayerPrefController.GetInitialGameStatus () != 1) {
			PlayerPrefController.SetHighScore(0);
			PlayerPrefController.SetBestTime (0);
			PlayerPrefController.SetVolume(0.4f);
			PlayerPrefController.SetInitialGameStatus (1);
		}
	}

	private void Start(){
		versionText.text = "v" + Application.version;

		//PlayerPrefController.GetVolume ();

		optionsController = GameObject.FindObjectOfType<OptionsController> ();

		UpdateBestScore ();

		clipIndex = 0;
		InvokeRepeating ("TriggerVoiceMessage", 2f, 35f);
	}

	public void UpdateBestScore(){
		scoreText.text = PlayerPrefController.GetHighScore ().ToString();

		int bestTime = PlayerPrefController.GetBestTime();
		int minutes = (int)(bestTime / 60) % 60;
		int seconds = (int)bestTime % 60;
		timeText.text = string.Format ("{00:00}:{01:00}", minutes, seconds);
	}

	private void TriggerVoiceMessage(){
		AudioClip clipToPlay = voiceClips [clipIndex];

		volumeBeforeVoiceClip = PlayerPrefController.GetVolume ();
		optionsController.volumeSlider.value = 0.05f;


		AudioSource.PlayClipAtPoint (clipToPlay, transform.position, 1f);

		if (clipIndex == 0) {
			clipIndex = 1;
		} else {
			clipIndex = 0;
		}
	}

	public void ShowTutorial(){
		optionsController.instructions [optionsController.instructionPage - 1].SetActive (false);
		optionsController.instructionPage = 1;
		optionsController.instructions [optionsController.instructionPage - 1].SetActive (true);
		tutorialMenu.SetActive (true);
		optionsController.UpdatePageText ();
	}

	public void HideTutorial(){
		tutorialMenu.SetActive (false);
	}


	public void ExitGame(){
		Application.Quit ();
	}

}
