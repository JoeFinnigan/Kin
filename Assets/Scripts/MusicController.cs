using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {
	public static MusicController instance;
	private MainMenuController mainMenu;

	public AudioClip bloodSplat, explosionSound, swordSwing, swordHit, wallHit, playerIsHit; 

	void Awake(){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}

	void Start(){
		mainMenu = GameObject.FindObjectOfType<MainMenuController> ();
	}

	public void SetVolume(float volume){
		PlayerPrefController.SetVolume (volume);
		GetComponent<AudioSource>().volume = volume;
	}

	public void RestoreVolume(){
		StopAllCoroutines ();
		PlayerPrefController.SetVolume (mainMenu.volumeBeforeVoiceClip);
	}

	public void PlaySoundEffect(AudioClip sound){
		AudioSource.PlayClipAtPoint (sound, transform.position, 1f);
	}
}
