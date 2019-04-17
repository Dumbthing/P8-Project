using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour {

public Transform hoursTrans, minutesTrans, secondsTrans;
public bool continuous;
const float degreesPerHour = 60;
const float degreesPerMinutes = 6;
const float degreesPerSeconds = 6;

	// Update is called once per frame
	void Update() {
		if(continuous) {
			UpdateContinuous();
		}
		else {
			UpdateDiscrete();
		}
	}
	void UpdateContinuous () {
		TimeSpan time = DateTime.Now.TimeOfDay;
		hoursTrans.localRotation = 
			Quaternion.Euler(0f, (float)time.TotalHours * degreesPerHour, 0);

		minutesTrans.localRotation = 
			Quaternion.Euler(0f, (float)time.TotalMinutes * degreesPerMinutes, 0);

		secondsTrans.localRotation = 
			Quaternion.Euler(0f, (float)time.TotalSeconds * degreesPerSeconds, 0);
	}

	void UpdateDiscrete() {
				// DateTime time = DateTime.Now;
		hoursTrans.localRotation = 
			Quaternion.Euler(0f, DateTime.Now.Hour * degreesPerHour, 0);

		minutesTrans.localRotation = 
			Quaternion.Euler(0f, DateTime.Now.Minute * degreesPerMinutes, 0);

		secondsTrans.localRotation = 
			Quaternion.Euler(0f, DateTime.Now.Second * degreesPerSeconds, 0);

	}
}
