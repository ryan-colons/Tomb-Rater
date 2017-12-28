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

	public Year getCurrentYear () {
		return calendar [yearIndex];
	}
	public int getYearIndex () {
		return yearIndex;
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

	public void addSpecialEventInXYears (SpecialEvent specialEvent, int numYears) {
		Year year = getYear (yearIndex + numYears);
		year.addSpecialEvent (specialEvent);
	}

	public void progressThroughCurrentYear (GameController gameController) {
		Year currentYear = calendar [yearIndex];

		//Labour - get resources
		ManageLabour labourManagement = gameController.getLabourManagement ();
		yearReport = labourManagement.collectAllResources ();

		//Builders - build
		ManageBuilding buildingManagement = gameController.getBuildingManagement();
		if (BuildingMenu.currentlyBuilding != null) {
			bool success = buildingManagement.addRoom (BuildingMenu.currentlyBuilding, BuildingMenu.selectedTiles.ToArray ());
			if (!success) {
				Debug.Log ("Something went wrong in the building process! Error!");
			}
			BuildingMenu.currentlyBuilding = null;
			BuildingMenu.selectedTiles.Clear ();
		}

		//need to subtract money (etc?) for worker/builder costs (etc?)

		//run special events, move on once they're all done
		//shift to "turn" scene, give script all the info it needs
		//current picking just 1 random event each year
		ManageSpecialEvents specialEventManagement = gameController.getSpecialEventManagement();
		SpecialEvent randEvent = specialEventManagement.chooseSpecialEventRandomly ();
		if (randEvent != null) {
			currentYear.addSpecialEvent (randEvent);
		}
		TurnoverScene.specialEvents = currentYear.getEvents().ToArray();
		TurnoverScene.eventIndex = 0;
		TurnoverScene.yearlyReport = yearReport;
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
	}
}
