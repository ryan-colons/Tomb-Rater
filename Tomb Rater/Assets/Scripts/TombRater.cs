using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombRater {

	private string finalReport = "";
	private int score = 0;

	private int tombSecurity = 0;

	private CharacterData charData;
	private ManageAdvisors advisorManagement;
	private ManageBuilding buildingManagement;
	private ManageYears yearManagement;
	private ManageOpinion opinionManagement;
	private ManageSpecialEvents specialEventManagement;

	private List<PosthumousEvent> earlyEvents;
	private List<PosthumousEvent> midEvents;
	private List<PosthumousEvent> lateEvents;
	private List<PosthumousEvent> reallyLateEvents;

	public TombRater (CharacterData info, ManageAdvisors advisor, ManageBuilding building,
		ManageYears years, ManageOpinion opinion, ManageSpecialEvents spEvent) {
		this.charData = info;
		this.advisorManagement = advisor;
		this.buildingManagement = building;
		this.yearManagement = years;
		this.opinionManagement = opinion;
		this.specialEventManagement = spEvent;
	}

	//this method returns your score
	public void rateTomb () {
		score = 0;
		finalReport = "";
		string kingdomName = charData.getKingdomName ();

		// check reputation
		int favour = opinionManagement.getNetFavour ();
		if (favour >= 9) {
			addToReport ("Your people loved you, and talked fondly of you for a long time.");
			score += 12;
		} else if (favour >= 6) {
			addToReport ("The people of " + kingdomName + " remembered you fondly.");
			score += 8;
		} else if (favour >= 3) {
			addToReport ("The people of " + kingdomName + " liked you as a leader, and said nice things about you when you died.");
			score += 4;
		} else if (favour > 0) {
			addToReport ("Your people said a few nice things about you when you died.");
		} else if (favour >= -3) {
			addToReport ("The people of " + kingdomName + " didn't like you much as a leader.");
			score -= 4;
		} else if (favour >= -6) {
			addToReport ("The people of " + kingdomName + " were glad you died.");
			score -= 8;
		} else if (favour >= -9) {
			addToReport ("Your people hated you, and cast you from their memory as best they could.");
			score -= 12;
		} else {
			//favour less than -9
			addToReport ("Your reign was a time of misery and woe for the people of " + kingdomName + ". They will not soon forget it.");
			score += 8;
		}

		// check tombs
		int sizeX = buildingManagement.getSizes() [0];
		int sizeY = buildingManagement.getSizes() [1];
		List<TombRoom> listOfRooms = new List<TombRoom> ();
		for (int x = 0; x < sizeX; x++) {
			for (int y = 0; y < sizeY; y++) {
				BuildTile tile = buildingManagement.getTileAtCoord (x, y);
				if (tile.getRoom () != null) {
					score += 1;
					if (!listOfRooms.Contains(tile.getRoom())) {
						listOfRooms.Add (tile.getRoom ());
					}
				}
			}
		}
		foreach (TombRoom room in listOfRooms) {
			foreach (Treasure treasure in room.getTreasureList()) {
				score += 1;
				foreach (TreasureType treasureType in treasure.getTypeList()) {
					if (room.getTreasureBonuses ().Contains (treasureType)) {
						score += 1;
					}
				}
			}
		}

		// simulation
		int yearsRemembered = 0;
		while (score > 0) {
			yearsRemembered += Random.Range (5, 9);
			score -= 1;
			// NEED A BETTER CONDITION HERE
			if (true) {
				PosthumousEvent newEvent = choosePosthumousEvent (yearsRemembered);
				if (newEvent != null) {
					addToReport (newEvent.trigger (this));
				}
			}
		}
	}

	public void addToReport (string str) {
		finalReport += str + "\n";
	}
	public void incrementScore (int n) {
		score += n;
	}

	public void incrementTombSecurity (int n) {
		tombSecurity += n;
	}
	public int getTombSecurity () {
		return tombSecurity;
	}

	public PosthumousEvent choosePosthumousEvent (int year) {
		List<PosthumousEvent> list;
		if (year <= 50)
			list = earlyEvents;
		else if (year <= 100)
			list = midEvents;
		else if (year <= 500)
			list = lateEvents;
		else
			list = reallyLateEvents;

		int sum = 0;
		foreach (PosthumousEvent postEvent in list) {
			sum += postEvent.getProbability ();
		}
		int randNum = Random.Range (0, sum);
		sum = 0;
		foreach (PosthumousEvent postEvent in list) {
			sum += postEvent.getProbability ();
			if (randNum < sum) {
				if (!postEvent.getReuse ()) {
					// will this work??
					list.Remove (postEvent);
				}
				return postEvent;
			}
		}
		return null;
	}

	public void removeEventFromAllLists (PosthumousEvent postEvent) {
		if (earlyEvents.Contains (postEvent)) {
			earlyEvents.Remove (postEvent);
		}
		if (midEvents.Contains (postEvent)) {
			midEvents.Remove (postEvent);
		}
		if (lateEvents.Contains (postEvent)) {
			lateEvents.Remove (postEvent);
		}
		if (reallyLateEvents.Contains (postEvent)) {
			reallyLateEvents.Remove (postEvent);
		}
	}
		
	public CharacterData getCharData () {
		return charData;
	}
	public ManageAdvisors getAdvisorManagement () {
		return advisorManagement;
	}
	public ManageBuilding getBuildingManagement () {
		return buildingManagement;
	}
	public ManageYears getYearManagement () {
		return yearManagement;
	}
	public ManageOpinion getOpinionManagement () {
		return opinionManagement;
	}
	public ManageSpecialEvents getSpecialEventManagement () {
		return specialEventManagement;
	}
}
