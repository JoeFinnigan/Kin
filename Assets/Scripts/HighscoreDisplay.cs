using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreDisplay : MonoBehaviour {
	public GameObject nameTemplate, scoreTemplate, dateTemplate;
	public GameObject nameColumn, scoreColumn, dateColumn, positionColumn;

	private int displayPositionFrom, displayPositionTo;
	private int tempLoopIndex;

	private Leaderboard leaderboardManager;
	 
	void Start () {
		displayPositionFrom = 1;
		displayPositionTo = 10;
		tempLoopIndex = 1;

		leaderboardManager = GetComponent<Leaderboard> ();
		BuildLeaderboard ();
	}

	public void OutputScores(Highscore[] highScoreList){
		for (int i = displayPositionFrom; i <= displayPositionTo; i++) {
			positionColumn.transform.Find ("Position" + tempLoopIndex).GetComponent<Text> ().text = i.ToString ();

			GameObject name = Instantiate (nameTemplate, nameColumn.transform, false) as GameObject;
			//name.transform.parent = nameColumn.transform;
			name.transform.SetParent(nameColumn.transform);

			GameObject score = Instantiate (scoreTemplate, scoreColumn.transform, false) as GameObject;
			//score.transform.parent = scoreColumn.transform;
			score.transform.SetParent(scoreColumn.transform);

			GameObject date = Instantiate (dateTemplate, dateColumn.transform, false) as GameObject;
			//date.transform.parent = dateColumn.transform;
			date.transform.SetParent(dateColumn.transform);

			if (i-1 < highScoreList.Length) {
				name.GetComponent<Text> ().text = highScoreList [i-1].name;
				score.GetComponent<Text> ().text = highScoreList [i-1].score.ToString();
				date.GetComponent<Text> ().text = highScoreList [i-1].date;
			} else {
				name.GetComponent<Text> ().text = "N/A";
				score.GetComponent<Text> ().text = "N/A";
				date.GetComponent<Text> ().text = "N/A";
			}

			tempLoopIndex++;
		}

		tempLoopIndex = 1;
	}

	public void ShowPreviousTenScores(){
		if (displayPositionFrom >= 10){
			displayPositionFrom -=10;
			displayPositionTo -= 10;

			ResetLeaderboard();
			BuildLeaderboard();
		}
		
	}

	public void ShowNextTenScores(){
		if (displayPositionFrom <= 90) {
			displayPositionFrom += 10;
			displayPositionTo += 10;

			ResetLeaderboard ();
			BuildLeaderboard ();
		}
	}

	public void ResetLeaderboard(){
		foreach (Text text in nameColumn.GetComponentsInChildren<Text>()){
			Destroy (text.gameObject);
		}

		foreach (Text text in scoreColumn.GetComponentsInChildren<Text>()){
			Destroy (text.gameObject);
		}

		foreach (Text text in dateColumn.GetComponentsInChildren<Text>()){
			Destroy (text.gameObject);
		}
	}

	private void BuildLeaderboard(){
		leaderboardManager.DownloadScores ();
	}
}
