using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefController : MonoBehaviour {
	const string SCORE_KEY = "score";
	const string TIME_KEY = "time";
	const string VOLUME_KEY = "volume";
	const string INITIAL_KEY = "initial";

	public static void SetHighScore(int score){
		PlayerPrefs.SetInt (SCORE_KEY, score);
	}

	public static int GetHighScore(){
		return PlayerPrefs.GetInt (SCORE_KEY);
	}

	public static void SetBestTime(int time){
		PlayerPrefs.SetInt (TIME_KEY, time);
	}

	public static int GetBestTime(){
		return PlayerPrefs.GetInt (TIME_KEY);
	}

	public static void SetVolume(float volume){
		PlayerPrefs.SetFloat (VOLUME_KEY, volume);
	}

	public static float GetVolume(){
		return PlayerPrefs.GetFloat (VOLUME_KEY);
	}

	public static void SetInitialGameStatus(int firstTimeLoad){
		PlayerPrefs.SetInt (INITIAL_KEY, firstTimeLoad);
	}

	public static int GetInitialGameStatus(){
		return PlayerPrefs.GetInt (INITIAL_KEY);
	}


}
