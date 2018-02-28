using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenu : MonoBehaviour {

	private GameController gameController;
	private ManageBuilding buildingManagement;
	//public GameObject tutPanel;
	public GameObject helpPanel;
	//public GameObject advicePanel;
	public GameObject roomPanel;
	public GameObject confirmBuildButton;
	public Text errorMessageText;

	public static Treasure currentlyPlacing = null;
	public static TombRoom currentlyBuilding = null;
	public static List<MapTile> selectedTiles = new List<MapTile> ();
	public static BuildMaterial materialToUse = null;

	private int workerPaymentThisYear = 0;
	private int totalBuildCostThisYear = 0;

	public GameObject tilePrefab;
	public Sprite wallSpr0, wallSpr1, wallSpr2, wallSpr3;

	private static int MAP_SIZE = MapTile.gridSize + 1;
	private GameObject[,] tileMap = new GameObject[MAP_SIZE, MAP_SIZE];

	public void setTileAtCoord (GameObject obj, int x, int y) {
		if (x < 0 || y < 0 || x >= MAP_SIZE || y >= MAP_SIZE) {
			Debug.Log ("Bad coords! x: " + x + ", y: " + y);
			return;
		}
		tileMap [x, y] = obj;
	}
	public GameObject getTileAtCoord (int x, int y) {
		return tileMap [x, y];
	}
	public MapTile getMapTileAtCoord (int x, int y) {
		// THIS CAN BE NULL
		//   oh
		//   no
		//
		Debug.Log ("Getting " + tileMap [x, y]);
		return getTileAtCoord (x, y).GetComponent<MapTile> ();
	}

	private void Start () {
		this.gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		this.buildingManagement = gameController.getBuildingManagement ();

		placeAllTiles ();
		setTileAdjacencies ();

		gameController.setBuildTutorialNeeded (false);
	}

	private void placeAllTiles () {
		this.gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		this.buildingManagement = gameController.getBuildingManagement ();

		//need to set sprites (etc?) for tiles based on info from buildingManagement.map
		for (int x = 0; x < MAP_SIZE; x++) {
			for (int y = 0; y < MAP_SIZE; y++) {
				placeTile (x, y);
			}
		}
	}

	public void placeTile (int x, int y) {
		BuildTile buildTile = buildingManagement.getTileAtCoord (x, y);
		MapTile mapTile = getMapTileAtCoord (x, y);
		if (buildTile.getRoom () != null) {
			WallsToShow tileWalls = buildTile.getSection ().getWalls ();
			Sprite spriteToUse = wallSpr0;
			if (tileWalls == WallsToShow.LEFT)
				spriteToUse = wallSpr1;
			else if (tileWalls == WallsToShow.RIGHT)
				spriteToUse = wallSpr2;
			else if (tileWalls == WallsToShow.BOTH)
				spriteToUse = wallSpr3;
			mapTile.setSprite (spriteToUse);
			if (buildTile.getSection ().getDecorationSprite () != null) {
				mapTile.setActualColor (Color.magenta);
				mapTile.setHighlight (false);
			}
		}
		//mapTile.gameObject.AddComponent<PolygonCollider2D> ();
		//set highlights according to what has been selected
		if (selectedTiles.Contains (mapTile)) {
			mapTile.setHighlight (true);
		}
	}

	public Text moneyText;
	private void Update () {
		moneyText.text = gameController.getMoney ().ToString ();
	}

	public void setTileAdjacencies () {
		// SET TILE ADJACENCIES FOR MANUALLY PLACED TILES
		for (int i = 0; i < MAP_SIZE; i++) {
			for (int j = 0; j < MAP_SIZE; j++) {
				GameObject north = null, west = null, east = null, south = null;
				if (i > 0) {
					north = tileMap [i - 1, j];
				}
				if (i < MAP_SIZE - 1) {
					south = tileMap [i + 1, j];
				}
				if (j > 0) {
					west = tileMap[i, j - 1];
				}
				if (j < MAP_SIZE - 1) {
					east = tileMap [i, j + 1];
				}
				//Debug.Log ("TILE: " + i + ", " + j + ": " + tileMap [i, j]);
				tileMap [i, j].GetComponent<MapTile> ().setAdjacentObjects (north, east, west, south);
			}
		}
	}

	public void toggleHelpPanel () {
		if (helpPanel.activeSelf == true) {
			helpPanel.SetActive (false);
		} else {
			helpPanel.SetActive (true);
		}
	}

	public void openRoomPanel (TombRoom room) {
		closeDecorationMenu ();
		closeBuildingMenu ();
		roomPanel.SetActive (true);
		Text nameText = roomPanel.transform.Find ("Name Text").GetComponent<Text>();
		Text descText = roomPanel.transform.Find ("Description Text").GetComponent<Text> ();
		Dropdown materialDropdown = roomPanel.transform.Find ("Material Dropdown").GetComponent<Dropdown> ();
		Slider workerPaymentSlider = roomPanel.transform.Find ("Builder Payment Slider").GetComponent<Slider> ();

		//name and description
		nameText.text = room.getName ();
		descText.text = room.getDescription ();
		//dropdown menu
		materialDropdown.ClearOptions ();
		List<string> matNames = new List<string> ();
		foreach (BuildMaterial mat in buildingManagement.getAvailableMaterials()) {
			matNames.Add (mat.getName ());
		}
		if (matNames.Count == 0) {
			matNames.Add ("No available building materials!");
		}
		materialDropdown.AddOptions (matNames);
		//payment slider
		workerPaymentSlider.minValue = 0;
		workerPaymentSlider.maxValue = gameController.getMoney ();
		if (buildingManagement.getWorkerExpectation () <= gameController.getMoney ()) {
			workerPaymentSlider.value = buildingManagement.getWorkerExpectation ();
		} else {
			workerPaymentSlider.value = gameController.getMoney ();
		}
		//text for worker payment and overall cost
		setWorkerPaymentText ();
		setCostText ();
		//confirm button
		Button buildButton = roomPanel.transform.Find ("Build Button").GetComponent<Button> ();
		buildButton.onClick.AddListener (delegate {
			BuildMaterial mat = getSelectedMaterialFromDropdown ();
			if (mat != null) {
				closeRoomPanel ();
				openBuildingMenu (room, mat);
			}
		});
	}
	public void closeRoomPanel () {
		roomPanel.SetActive (false);
	}

	public void setWorkerPayment () {
		Slider workerPaymentSlider = roomPanel.transform.Find ("Builder Payment Slider").GetComponent<Slider> ();
		workerPaymentThisYear = (int)workerPaymentSlider.value;
		setWorkerPaymentText ();
	}
	public int getWorkerPayment () {
		return workerPaymentThisYear;
	}

	public BuildMaterial getSelectedMaterialFromDropdown () {
		Dropdown materialDropdown = roomPanel.transform.Find ("Material Dropdown").GetComponent<Dropdown> ();
		string matName = materialDropdown.options [materialDropdown.value].text;
		BuildMaterial mat = null;
		foreach (BuildMaterial availableMat in buildingManagement.getAvailableMaterials()) {
			if (availableMat.getName ().Equals (matName)) {
				mat = availableMat;
			}
		}
		return mat;
	}

	public void setWorkerPaymentText () {
		Text paymentText = roomPanel.transform.Find ("Builder Payment Text").GetComponent<Text> ();
		paymentText.text = "How much shall we pay your workers? They expect " + buildingManagement.getWorkerExpectation () +
		" per tile this year.\n Payment: " + workerPaymentThisYear + "g";
		setCostText ();
	}

	public void setCostText () {
		//set cost text based on currently selected material in dropdown
		Text costText = roomPanel.transform.Find ("Cost Text").GetComponent<Text> ();

		BuildMaterial mat = getSelectedMaterialFromDropdown ();
		if (mat == null) {
			costText.text = "";
		} else {
			int matCost = mat.getCostPerTile ();
			costText.text = "Using " + mat.getName() + " (" + matCost + "g per tile), this construction will cost " + (matCost + workerPaymentThisYear) + "g per tile.";
				
		}
	}

	public void openBuildingMenu (TombRoom room, BuildMaterial mat) {
		closeDecorationMenu ();
		setHighlightForSelectedTiles (false);
		selectedTiles.Clear();
		gameController.setMoney (gameController.getMoney () + totalBuildCostThisYear);
		totalBuildCostThisYear = 0;
		currentlyBuilding = room;
		materialToUse = mat;
		confirmBuildButton.SetActive (true);
	}

	public void closeBuildingMenu () {
		setHighlightForSelectedTiles (false);
		selectedTiles.Clear ();
		currentlyBuilding = null;
		materialToUse = null;
		confirmBuildButton.SetActive (false);
	}

	public void setHighlightForSelectedTiles (bool b) {
		foreach (MapTile mapTile in selectedTiles) {
			int x = mapTile.getX ();
			int y = mapTile.getY ();
			getMapTileAtCoord (x, y).setHighlight (b);
		}
	}

	public void confirmSelectedTiles () {
		bool passedTests = validateSelectedTiles ();
		if (!passedTests) {
			Debug.Log ("Bad tile selection!");
			closeBuildingMenu ();
		} else {
			Debug.Log (currentlyBuilding + ": " + selectedTiles.Count);
			totalBuildCostThisYear = getWorkerPayment() + materialToUse.getCostPerTile() * selectedTiles.Count;
			gameController.setMoney (gameController.getMoney () - totalBuildCostThisYear);
			confirmBuildButton.SetActive (false);
		}
	}

	public bool validateSelectedTiles () {
		if (currentlyBuilding == null || selectedTiles.Count == 0) {
			reportBuildError ("You haven't selected anything to build!");
			return false;
		}
		//check if enough tiles have been selected
		if (selectedTiles.Count < currentlyBuilding.getMinSize ()) {
			reportBuildError ("You haven't selected enough tiles (" + selectedTiles.Count + "/" + currentlyBuilding.getMinSize() + ").");
			return false;
		}
		//check if you have enough money
		int cost = (getWorkerPayment() + materialToUse.getCostPerTile()) * selectedTiles.Count;
		if (cost > gameController.getMoney ()) {
			reportBuildError ("You don't have enough money for that project. (Total cost: " + cost + ")");
			return false;
		}
		//check for external contiguity (i.e. attached to all other rooms)
		//can check for this just by seeing if the room is attached to any other room
		bool attachedToOtherRooms = false;
		foreach (MapTile mapTile in selectedTiles) {
			BuildTile buildTile = buildingManagement.getTileAtCoord (mapTile.getX (), mapTile.getY ());
			//check all tiles are unoccupied
			if (buildTile.getRoom () != null) {
				reportBuildError ("You cannot build on tiles that have already been built on.");
				return false;
			}
			//check for internal contiguity
			MapTile[] adjacentMapTiles = mapTile.getAdjacentTiles();
			bool hasContiguity = false;
			foreach (MapTile neighbour in adjacentMapTiles) {
				if (neighbour != null && selectedTiles.Contains(neighbour)) {
					hasContiguity = true;
					break;
				}
				if (neighbour != null && !selectedTiles.Contains (neighbour)) {
					attachedToOtherRooms = true;
				}
			}
			if (!hasContiguity && selectedTiles.Count > 1) {
				reportBuildError ("You need to select a single, contiguous set of tiles. (Tiles selected: " + selectedTiles.Count + ")");
				return false;
			}
		}
		if (!attachedToOtherRooms) {
			//maybe put in an exception for the first room or something
			reportBuildError("All rooms must be connected!");
			return false;
		}
		// all tests passed!
		return true;
	}

	public void reportBuildError (string errorMessage) {
		StopCoroutine ("displayErrorMessage");
		StartCoroutine ("displayErrorMessage", errorMessage);
	}

	public void openDecorationMenu (Treasure treasure) {
		//close all other menus, except the Treasure Panel
		closeDecorationMenu();
		closeBuildingMenu ();
		closeRoomPanel ();
		//set Treasure field
		currentlyPlacing = treasure;
		//make sprite that follows the mouse
		Instantiate (Resources.Load ("FollowSprite"), 
			new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0f), Quaternion.identity);
	}
	public void closeDecorationMenu () {
		currentlyPlacing = null;
		GameObject followingSpr = GameObject.FindWithTag ("Follow Sprite");
		if (followingSpr != null) {
			Destroy (followingSpr);
		}
	}
	public void placeDecoration (MapTile mapTile) {
		BuildTile buildTile = buildingManagement.getTileAtCoord (mapTile.getX (), mapTile.getY ());
		TombRoom room = buildTile.getRoom ();
		RoomSection section = buildTile.getSection ();

		//validate here
		if (room == null || section == null) {
			reportBuildError ("That needs to placed in an existing room.");
			return;
		}
		if (section.getDecorationSprite () != null) {
			reportBuildError ("There is already something on that tile.");
			return;
		}
				
		section.setDecorationSprite (currentlyPlacing.getSprite ());
		room.getTreasureList ().Add (currentlyPlacing);
		ManageTreasure treasureManagement = gameController.getTreasureManagement ();
		treasureManagement.getTreasureList ().Remove (currentlyPlacing);
		closeDecorationMenu ();
		placeTile (mapTile.getX (), mapTile.getY ());
	}

	public IEnumerator displayErrorMessage (string errorMessage) {
		errorMessageText.text = errorMessage;
		yield return new WaitForSeconds (2f);
		errorMessageText.text = "";
	}

	public void returnToOverMenu () {
		//if the confirm button is active, selected tiles haven't been confirmed
		//therefore, selected tiles should be forgotten
		if (confirmBuildButton.activeSelf) {
			selectedTiles.Clear ();
		}
		gameController.loadScene ("menu");
	}
}

