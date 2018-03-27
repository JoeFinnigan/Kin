using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Highscore{
	public string name;
	public int score;
	public string date;

	public Highscore(string _name, int _score, string _date){
		name = _name;
		score = _score;
		date = _date;
	}
}
