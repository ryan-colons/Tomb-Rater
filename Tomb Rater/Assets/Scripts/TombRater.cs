using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombRater {

	private List<string> finalReport = new List<string>();
	private int score = 0;

	private int tombSecurity = 0;

	private CharacterData charData;
	private ManageAdvisors advisorManagement;
	private ManageBuilding buildingManagement;
	private ManageYears yearManagement;
	private ManageOpinion opinionManagement;
	private ManageSpecialEvents specialEventManagement;

	private List<PosthumousEvent> earlyEvents = new List<PosthumousEvent>();
	private List<PosthumousEvent> midEvents = new List<PosthumousEvent>();
	private List<PosthumousEvent> lateEvents = new List<PosthumousEvent>();
	private List<PosthumousEvent> reallyLateEvents = new List<PosthumousEvent>();

	public TombRater (CharacterData info, ManageAdvisors advisor, ManageBuilding building,
		ManageYears years, ManageOpinion opinion, ManageSpecialEvents spEvent) {
		this.charData = info;
		this.advisorManagement = advisor;
		this.buildingManagement = building;
		this.yearManagement = years;
		this.opinionManagement = opinion;
		this.specialEventManagement = spEvent;

		addEventAnytime (new Post_Entropy ());
		addEventAnytime (new Post_AmateurTombRobbers ());
		addEventAnytime (new Post_ExpertTombRobbers ());

		addEarlyEvent (new Post_ExcitingNewReign());
		addEarlyEvent (new Post_MassVisitation());
		addEarlyEvent (new Post_Vandalism());

		addMidEvent (new Post_NewTomb());
	}

	//this method returns your score
	public int rateTomb () {
		score = 0;
		finalReport = new List<string>();
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
		return yearsRemembered;
	}

	public void addToReport (string str) {
		finalReport.Add(str);
	}
	public void incrementScore (int n) {
		score += n;
	}

	public List<string> getReport() {
		return this.finalReport;
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
			sum += postEvent.getProbability (this);
		}
		int randNum = Random.Range (0, sum);
		sum = 0;
		foreach (PosthumousEvent postEvent in list) {
			sum += postEvent.getProbability (this);
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
		
	public void addEventAnytime (PosthumousEvent newEvent) {
		addEarlyEvent (newEvent);
		addMidEvent (newEvent);
		addLateEvent (newEvent);
		addReallyLateEvent (newEvent);
	}
	public void addEarlyEvent (PosthumousEvent newEvent) {
		earlyEvents.Add (newEvent);
	}
	public void addMidEvent (PosthumousEvent newEvent) {
		midEvents.Add (newEvent);
	}
	public void addLateEvent (PosthumousEvent newEvent) {
		lateEvents.Add (newEvent);
	}
	public void addReallyLateEvent (PosthumousEvent newEvent) {
		reallyLateEvents.Add (newEvent);
	}

	public void clearAllLists () {
		earlyEvents = new List<PosthumousEvent> ();
		midEvents = new List<PosthumousEvent> ();
		lateEvents = new List<PosthumousEvent> ();
		reallyLateEvents = new List<PosthumousEvent> ();
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

	public List<PosthumousEvent>[] getLists () {
		return new List<PosthumousEvent>[] {
			earlyEvents, midEvents, lateEvents, reallyLateEvents
		};
	}
	public void setLists (List<PosthumousEvent>[] array) {
		earlyEvents = array [0];
		midEvents = array [1];
		lateEvents = array [2];
		reallyLateEvents = array [3];
	}
}
