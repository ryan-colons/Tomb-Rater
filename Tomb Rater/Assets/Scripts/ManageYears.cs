using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageYears  {

	private const int CALENDAR_SIZE = 100;

	private Year[] calendar;
	private Year[] bufferCalendar;
	private int yearIndex;

	private string yearReport;

	public ManageYears () {
		calendar = new Year[CALENDAR_SIZE];
		yearIndex = 0;
		int currentYear = 50;
		for (int n = 0; n < 100; n++) {
			calendar [n] = new Year (n + currentYear);
		}
	}

	public Year getYear (int yearNum) {
		if (yearNum < CALENDAR_SIZE) {
			return calendar [yearNum];
		} else {
			//this is just handling for cases where the game goes for lots of turns
			//this will probably never happen
			if (bufferCalendar == null) {
				bufferCalendar = new Year[CALENDAR_SIZE];
			}
			return bufferCalendar [yearNum - CALENDAR_SIZE];
		}
	}

	public void progressThroughCurrentYear (GameController gameController) {
		Year currentYear = calendar [yearIndex];

		//Labour - get resources
		ManageLabour labourManagement = gameController.getLabourManagement ();
		yearReport = labourManagement.collectAllResources ();
		//print string somewhere once we're done

		//Builders - build


		//run special events
		//load scene with basic 'year-lapse' animations
		//store in that scene an array of special events and an index
		//iterate through events; move to event scene, and back
		//once all events are done, move on
		TurnoverScene.specialEvents = currentYear.getEvents().ToArray();
		TurnoverScene.eventIndex = 0;
		gameController.loadScene ("turn");

	}

	public void progressToNextYear (GameController gameController) {
		//move index to the next year
		yearIndex++;
		if (yearIndex >= CALENDAR_SIZE) {
			//this is just handling for cases where the game goes for lots of turns
			//this will probably never happen
			if (bufferCalendar == null) {
				bufferCalendar = new Year[CALENDAR_SIZE];
			}
			calendar = bufferCalendar;
			bufferCalendar = null;
			yearIndex = 0;
		}
		Debug.Log ("Happy Birthday Your Majesty! It is now the year " + calendar [yearIndex].getYearName ());
		Debug.Log ("Stock report this year:\n" + yearReport);
		gameController.loadScene ("menu");
	}
}
