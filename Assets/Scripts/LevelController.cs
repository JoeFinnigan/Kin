using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {
	private MainMenuController mainMenu;
	private OptionsController options;

	void Start(){
		mainMenu = GameObject.FindObjectOfType<MainMenuController> ();
		options = GameObject.FindObjectOfType<OptionsController> ();
	}

	public void LoadScene(string scene){
		if (SceneManager.GetActiveScene ().name == "MainMenu") {
			if (mainMenu.volumeBeforeVoiceClip != 0) {
				options.volumeSlider.value = mainMenu.volumeBeforeVoiceClip;
				PlayerPrefController.SetVolume (mainMenu.volumeBeforeVoiceClip);
			}
		}

		SceneManager.LoadScene (scene);
	}
}
