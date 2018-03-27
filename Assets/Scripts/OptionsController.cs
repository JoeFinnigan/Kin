using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour {
	public GameObject[] instructions;
	public int instructionPage;
	public Text pageListText;

	public GameObject quitButton;
	public Slider volumeSlider;
	public GameObject optionsMenu;

	void Start () {
		instructionPage = 1;

		if (SceneManager.GetActiveScene ().name == "MainLevel") {
			quitButton.SetActive (true);
		} else {
			quitButton.SetActive (false);
		}

		volumeSlider.value = PlayerPrefController.GetVolume ();
	}

	public void ResetStats(){
		PlayerPrefController.SetHighScore (0);
		PlayerPrefController.SetBestTime (0);

		MainMenuController mainMenu = GameObject.FindObjectOfType<MainMenuController> ();
		mainMenu.UpdateBestScore ();
	}

	public void UpdatePageText(){
		pageListText.text = instructionPage + " / " + instructions.Length;
	}

	public void OpenOptionsScreen(){
		optionsMenu.SetActive (true);
	}

	public void CloseOptionsScreen(){
		optionsMenu.SetActive (false);
	}

	public void PreviousPage(){
		if (instructionPage <= instructions.Length && instructionPage > 1) {
			instructionPage--;
			instructions [instructionPage].SetActive (false);
			instructions [instructionPage - 1].SetActive (true);
		}

		UpdatePageText ();
	}

	public void NextPage(){
		if (instructionPage > 0 && instructionPage < instructions.Length) {
			instructionPage++;
			instructions [instructionPage - 1].SetActive (true);
			instructions [instructionPage - 2].SetActive (false);
		}

		UpdatePageText ();
	}
}
