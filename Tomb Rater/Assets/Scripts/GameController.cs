using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	//used for loadEvent(), which is how all special events should be entered
	private SpecialEvent holsteredEvent;

	private bool overmenuTutorial, workTutorial,  buildTutorial;

	private ManageLabour labourManagement;
	private ManageResources resourceManagement;
	private ManageBuilding buildingManagement;
	private ManageYears yearManagement;
	private ManageSpecialEvents specialEventManagement;

	private void Start () {
		SpecialEvent.gameController = this;
		SpecialEventUI.gameController = this;
		ResourceSite.gameController = this;

		overmenuTutorial = true;
		workTutorial = true;
		buildTutorial = true;

		//sites are currently added in the ManageLabour constructor
		labourManagement = new ManageLabour ();
		resourceManagement = new ManageResources ();
		buildingManagement = new ManageBuilding ();
		yearManagement = new ManageYears ();
		specialEventManagement = new ManageSpecialEvents ();

		yearManagement.getYear (0).addSpecialEvent (new Event_SpecialEventTest ());
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

	public bool overmenuTutorialNeeded () {
		return overmenuTutorial;
	}
	public void setOvermenuTutorialNeeded (bool b) {
		overmenuTutorial = b;
	}
	public bool workTutorialNeeded () {
		return workTutorial;
	}
	public void setWorkTutorialNeeded (bool b) {
		workTutorial = b;
	}
	public bool buildTutorialNeeded () {
		return buildTutorial;
	}
	public void setBuildTutorialNeeded (bool b) {
		buildTutorial = b;
	}

	public ManageResources getResourceManagement() {
		return resourceManagement;
	}
	public ManageLabour getLabourManagement () {
		return labourManagement;
	}
	public ManageBuilding getBuildingManagement () {
		return buildingManagement;
	}
	public ManageYears getYearManagement () {
		return yearManagement;
	}
	public ManageSpecialEvents getSpecialEventManagement () {
		return specialEventManagement;
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
}
