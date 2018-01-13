using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombRater {

	private CharacterData charData;
	private ManageAdvisors advisorManagement;
	private ManageBuilding buildingManagement;
	private ManageYears yearManagement;
	private ManageOpinion opinionManagement;
	private ManageSpecialEvents specialEventManagement;

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
	public int rateTomb () {
		int score = 0;
		string report = "";
		string kingdomName = charData.getKingdomName ();

		// check reputation
		int favour = opinionManagement.getNetFavour ();
		if (favour >= 9) {
			report += "Your people loved you, and talked fondly of you for a long time.";
			score += 12;
		} else if (favour >= 6) {
			report += "The people of " + kingdomName + " remembered you fondly.";
			score += 8;
		} else if (favour >= 3) {
			report += "The people of " + kingdomName + " liked you as a leader, and said nice things about you when you died.";
			score += 4;
		} else if (favour > 0) {
			report += "Your people said a few nice things about you when you died.";
		} else if (favour >= -3) {
			report += "The people of " + kingdomName + " didn't like you much as a leader.";
			score -= 4;
		} else if (favour >= -6) {
			report += "The people of " + kingdomName + " were glad you died.";
			score -= 8;
		} else if (favour >= -9) {
			report += "Your people hated you, and cast you from their memory as best they could.";
			score -= 12;
		} else {
			//favour less than -9
			report += "Your reign was a time of misery and woe for the people of " + kingdomName + ". They will not soon forget it.";
			score += 8;
		}

		// check tombs
		int sizeX = buildingManagement.getSizes() [0];
		int sizeY = buildingManagement.getSizes() [1];
		for (int x = 0; x < sizeX; x++) {
			for (int y = 0; y < sizeY; y++) {
				BuildTile tile = buildingManagement.getTileAtCoord (x, y);
				if (tile.getRoom () != null) {
					score += 1;
				}
			}
		}

		// simulation


		return score;
	}
}
