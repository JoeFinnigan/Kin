using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour {
	const string privateCode = "IbmIloa_80S8AGAlswg2rAIopnS7EfrUCFV22SS7VtMg";
	const string publicCode = "5a8ecc2d39992d09e4e0911d";
	const string siteLink = "http://dreamlo.com/lb/";

	public Highscore[] highScores;
	private HighscoreDisplay highscoreDisplay;
	public static Leaderboard instance;

	void Awake(){
		highscoreDisplay = GetComponent<HighscoreDisplay> ();
		instance = this;
	}

	public void UploadScore(string username, int score){
		StartCoroutine (UploadScoreCo (username, score));
	}

	private IEnumerator UploadScoreCo (string username, int score){
		WWW www = new WWW (siteLink + privateCode + "/add/" + WWW.EscapeURL (username) + "/" + score);
		yield return www;

		if (string.IsNullOrEmpty (www.error)) {
			print ("Upload complete");
		} else {
			print ("Error occurred: " + www.error);
		}
	}

	public void DownloadScores(){
		StartCoroutine (DownloadScoresCo ());
	}

	private IEnumerator DownloadScoresCo (){
		WWW www = new WWW (siteLink + publicCode + "/pipe/");
		yield return www;

		if (string.IsNullOrEmpty (www.error)) {
			FormatScores (www.text);
			highscoreDisplay.OutputScores (highScores);
		} else {
			print ("Error occurred: " + www.error);
		}
	}

	private void FormatScores(string scoreList){
		string[] entries = scoreList.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
		highScores = new Highscore[entries.Length];

		for (int i = 0; i < entries.Length; i++){
			string[] entryInfo = entries [i].Split (new char[] {'|'});
			string name = entryInfo [0];
			int score = int.Parse(entryInfo [1]);
			string date = DateTime.Parse (entryInfo [4]).ToString ("dd MMM yyyy");
	
			highScores [i] = new Highscore (name, score, date);
		}
	}


}
