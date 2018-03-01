using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {

	// CharData
	public int age;
	public string name, title, kingdom;
	public string tradeCiv, rivalCiv, complainer;
	public string[] complainerPronouns;

	// GameController
	public int money;
	public PathToDeath healthDeathPath;
	public PathToDeath revolutionDeathPath;
	public bool buildTutorialNeeded;

	// TombRater
	public List<PosthumousEvent> early;
	public List<PosthumousEvent> mid;
	public List<PosthumousEvent> late;
	public List<PosthumousEvent> vLate;

	// ManageAdvisors
	public Advisor[] advisors;
	public int gpt;
	public int offence, defence;

	// ManageBuilding
	public BuildTile[,] buildTileMap;
	public List<TombRoom> roomsToBuild;
	public List<BuildMaterial> availableMaterials;

	// ManageOpinion
	public int favour, disfavour, awe, fear;

	// ManageSpecialEvents
	public int complaints;
	public List<SpecialEvent> events;

	// ManageTreasure
	public List<Treasure> treasureList;

	// ManageYears
	public Year[] calendar;
	public Year[] bufferCalendar;
	public int yearIndex;

	public SaveData (GameController game) {
		CharacterData info = game.getCharData ();
		this.name = info.getPlayerName ();
		this.age = info.getPlayerAge ();
		this.title = info.getPlayerTitle ();
		this.kingdom = info.getKingdomName ();
		this.rivalCiv = info.getRivalCivName ();
		this.tradeCiv = info.getTradeCivName ();
		this.complainer = info.getComplainName ();
		this.complainerPronouns = info.getComplainPronouns ();

		this.money = game.getMoney ();
		this.healthDeathPath = game.getHealth ();
		this.revolutionDeathPath = game.getRevolution ();
		this.buildTutorialNeeded = game.buildTutorialNeeded ();

		TombRater rater = game.getTombRater ();
		List<PosthumousEvent>[] array = rater.getLists ();
		this.early = array [0];
		this.mid = array [1];
		this.late = array [2];
		this.vLate = array [3];

		ManageAdvisors advisorManagement = game.getAdvisorManagement ();
		this.advisors = advisorManagement.getAdvisors ();
		this.gpt = advisorManagement.getGPT ();
		this.offence = advisorManagement.getOffensiveMight ();
		this.defence = advisorManagement.getDefensiveMight ();

		ManageBuilding buildingManagement = game.getBuildingManagement ();
		this.buildTileMap = buildingManagement.getTileMap ();
		this.roomsToBuild = buildingManagement.getAvailableRoomList ();
		this.availableMaterials = buildingManagement.getAvailableMaterialList ();

		ManageOpinion opinion = game.getOpinionManagement ();
		this.favour = opinion.getPublicFavour ();
		this.disfavour = opinion.getPublicDisfavour ();
		this.awe = opinion.getPublicAwe ();
		this.fear = opinion.getPublicFear ();

		ManageSpecialEvents spEvents = game.getSpecialEventManagement ();
		this.complaints = spEvents.getComplaintsHeard ();
		this.events = spEvents.getPossibleEvents ();

		ManageTreasure treasureManagement = game.getTreasureManagement ();
		this.treasureList = treasureManagement.getTreasureList ();

		ManageYears years = game.getYearManagement ();
		this.calendar = years.getCalendar ();
		this.bufferCalendar = years.getBufferCalendar ();
		this.yearIndex = years.getYearIndex ();
	}

	public void unload (GameController game) {
		CharacterData info = game.getCharData ();
		info.setPlayerName (this.name);
		info.setPlayerAge (this.age);
		info.setPlayerTitle (this.title);
		info.setKingdomName (this.kingdom);
		info.setRivalCivName (this.rivalCiv);
		info.setTradeCivName (this.tradeCiv);
		info.setComplainName (this.complainer);
		info.setComplainPronouns (this.complainerPronouns);

		game.setMoney (this.money);
		game.setHealth (this.healthDeathPath);
		game.setRevolution (this.revolutionDeathPath);
		game.setBuildTutorialNeeded (buildTutorialNeeded);

		TombRater rater = game.getTombRater ();
		List<PosthumousEvent>[] listArray = new List<PosthumousEvent>[] {
			this.early, this.mid, this.late, this.vLate
		};
		rater.setLists (listArray);

		ManageAdvisors advisorManagement = game.getAdvisorManagement ();
		advisorManagement.setAdvisors (this.advisors);
		advisorManagement.setGPT (this.gpt);
		advisorManagement.setOffensiveMight (this.offence);
		advisorManagement.setDefensiveMight (this.defence);

		ManageBuilding buildingManagement = game.getBuildingManagement ();
		buildingManagement.setTileMap (this.buildTileMap);
		buildingManagement.setRoomsToBuild (this.roomsToBuild);
		buildingManagement.setAvailableMaterials (this.availableMaterials);

		ManageOpinion opinion = game.getOpinionManagement ();
		opinion.setPublicFavour (this.favour);
		opinion.setPublicDisfavour (this.disfavour);
		opinion.setPublicAwe (this.awe);
		opinion.setPublicFear (this.fear);

		ManageSpecialEvents spEvents = game.getSpecialEventManagement ();
		spEvents.setComplaintsHeard (this.complaints);
		spEvents.setPossibleEvents (this.events);

		ManageTreasure treasureManagement = game.getTreasureManagement ();
		treasureManagement.setTreasureList (this.treasureList);

		ManageYears years = game.getYearManagement ();
		years.setCalendar (this.calendar);
		years.setBufferCalendar (this.bufferCalendar);
		years.setYearIndex (this.yearIndex);
	}
}
