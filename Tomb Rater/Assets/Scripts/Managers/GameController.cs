using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
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
	private PathToDeath healthPathToDeath;
	private PathToDeath revolutionPathToDeath;

	private TombRater tombRater;

	private void Start () {
		SpecialEvent.gameController = this;
		SpecialEventUI.gameController = this;
		Treasure.gameController = this;
		AdvisorMilestone.gameController = this;

		buildTutorial = true;

		charData = new CharacterData ();
	}

	// this is called from the introductory event
	public void createManagers () {
		advisorManagement = new ManageAdvisors ();
		buildingManagement = new ManageBuilding ();
		treasureManagement = new ManageTreasure ();
		opinionManagement = new ManageOpinion ();
		yearManagement = new ManageYears (50);
		specialEventManagement = new ManageSpecialEvents ();

		tombRater = new TombRater (charData, advisorManagement, buildingManagement, yearManagement, opinionManagement, specialEventManagement);

		charData.generateCivNames ();
		charData.generateComplainer ();
		SpecialEvent[] healthEvents = new SpecialEvent[] {
			new Event_EatingLotsOfCheese(),
			new Event_CheeseSickness(),
			new Event_OveractiveParasites(),
			new Event_TerminalIllness()
		};
		healthPathToDeath = new PathToDeath (healthEvents);
		SpecialEvent[] revolutionEvents = new SpecialEvent[] {
			new Event_Unpopular(),
			new Event_Protests(),
			new Event_AssassinationPrevented(),
			new Event_AssassinationSuccessful()
		};	
		revolutionPathToDeath = new PathToDeath (revolutionEvents);
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

	public TombRater getTombRater () {
		return tombRater;
	}

	public int getMoney () {
		return this.money;
	}
	public void setMoney (int n) {
		this.money = n;
	}

	public CharacterData getCharData () {
		return charData;
	}
	public PathToDeath getHealth () {
		return healthPathToDeath;
	}
	public PathToDeath getRevolution () {
		return revolutionPathToDeath;
	}
	public void setHealth (PathToDeath h) {
		healthPathToDeath = h;
	}
	public void setRevolution (PathToDeath r) {
		revolutionPathToDeath = r;
	}

	public string getTradeCivName () {
		return charData.getTradeCivName();
	}
	public string getRivalCivName () {
		return charData.getRivalCivName();
	}


	public void save () {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.tr");
		SaveData save = new SaveData (this);
		bf.Serialize (file, save);
		file.Close ();
	}

	public bool load () {
		if (File.Exists (Application.persistentDataPath + "/playerInfo.tr")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerInfo.tr", FileMode.Open);
			SaveData save = (SaveData)bf.Deserialize (file);
			file.Close ();
			save.unload (this);
			return true;
		} else {
			Debug.Log ("Couldn't find a save file!");
			return false;
		}
	}
}
