using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	//used for loadEvent(), which is how all special events should be entered
	private SpecialEvent holsteredEvent;

	private bool buildTutorial;

	private ManageAdvisors advisorManagement;
	private ManageBuilding buildingManagement;
	private ManageTreasure treasureManagement;
	private ManageYears yearManagement;
	private ManageOpinion opinionManagement;
	private ManageSpecialEvents specialEventManagement;

	private int money = 5000;

	private CharacterData charData;

	private void Start () {
		SpecialEvent.gameController = this;
		SpecialEventUI.gameController = this;
		Treasure.gameController = this;
		AdvisorMilestone.gameController = this;

		buildTutorial = true;

		charData = new CharacterData ();
		charData.generateCivNames ();

		advisorManagement = new ManageAdvisors ();
		buildingManagement = new ManageBuilding ();
		treasureManagement = new ManageTreasure ();
		yearManagement = new ManageYears (50);
		specialEventManagement = new ManageSpecialEvents ();
	}

	private void Awake () {
		DontDestroyOnLoad (this.gameObject);
	}

	public void loadScene (string sceneName) {
		SceneManager.LoadScene (sceneName);
	}

	public void loadEvent (SpecialEvent specialEvent) {
		this.holsteredEvent = specialEvent;
		loadScene ("event");
	}

	public void nextYear () {
		yearManagement.progressThroughCurrentYear (this);
	}

	public SpecialEvent getHolsteredEvent () {
		return holsteredEvent;
	}
		
	public bool buildTutorialNeeded () {
		return buildTutorial;
	}
	public void setBuildTutorialNeeded (bool b) {
		buildTutorial = b;
	}
		
	public ManageAdvisors getAdvisorManagement () {
		return advisorManagement;
	}
	public ManageBuilding getBuildingManagement () {
		return buildingManagement;
	}
	public ManageTreasure getTreasureManagement () {
		return treasureManagement;
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

	public int getMoney () {
		return this.money;
	}
	public void setMoney (int n) {
		this.money = n;
		Debug.Log ("Money: " + n);
	}

	public CharacterData getCharData () {
		return charData;
	}

	public string getTradeCivName () {
		return charData.getTradeCivName();
	}
	public string getRivalCivName () {
		return charData.getRivalCivName();
	}

	//this method returns your score
	public int rateTomb () {
		int score = 0;

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

		return score;
	}

	/* THINGS TO SAVE
	 * GameController
	 *   -money
	 * CharData
	 *   -names
	 * AdvisorManagement
	 *   -advisors (and related milestones)
	 *   -GPT, military might
	 * BuildingManagement
	 *   -map (2d buildtile array)
	 *   -available rooms, available materials
	 * TreasureManagement
	 *   -treasure list
	 * SpecialEventManagement
	 *   -possible event list
	 * YearManagement
	 *   -calendar, index (buffer calendar)
	 */
}
