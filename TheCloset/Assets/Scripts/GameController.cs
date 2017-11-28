using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour 
{

	float _timeLeft;


	public Text ClockText;
	public GameObject DeadObject;

	void Start () 
	{
		_timeLeft = 5 * 60; // 5 mins
	}
	
	void Update () 
	{
		if (_timeLeft > 0) {
			_timeLeft -= Time.deltaTime;

			int minutes = ((int)_timeLeft) / 60;
			int seconds = ((int)_timeLeft) % 60;

			ClockText.text = string.Format ("{0:00}:{1:00}", minutes, seconds);

			if (_timeLeft < 0) {
				DeadObject.SetActive (true);
			}
		}
	}
}
