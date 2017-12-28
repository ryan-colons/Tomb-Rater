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

	public static TombRoom currentlyBuilding = null;
	public static List<MapTile> selectedTiles = new List<MapTile> ();

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
		return getTileAtCoord (x, y).GetComponent<MapTile> ();
	}

	private void Start () {
		this.gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		this.buildingManagement = gameController.getBuildingManagement ();

		setTileAdjacencies ();

		//need to set sprites (etc?) for tiles based on info from buildingManagement.map
		for (int x = 0; x < MAP_SIZE; x++) {
			for (int y = 0; y < MAP_SIZE; y++) {
				BuildTile buildTile = buildingManagement.getTileAtCoord (x, y);
				MapTile mapTile = getMapTileAtCoord (x, y);
				// buildTile and mapTile should correspond
				// buildTile has game info, mapTile handles the graphical representation (basically)
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
				}
			}
		}
		//set highlights according to what has been selected
		foreach (MapTile mapTile in selectedTiles) {
			int x = mapTile.getX ();
			int y = mapTile.getY ();
			getMapTileAtCoord (x, y).setHighlight (true);
		}

		/* THERE NEEDS TO BE SOME TUTORIAL EVENTUALLY!
		if (gameController.buildTutorialNeeded ()) {
			tutPanel.SetActive(true);
			gameController.setBuildTutorialNeeded (false);
		}
		*/
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
		roomPanel.SetActive (true);
		Text nameText = roomPanel.transform.Find ("Name Text").GetComponent<Text>();
		Text descText = roomPanel.transform.Find ("Description Text").GetComponent<Text> ();
		nameText.text = room.getName ();
		descText.text = room.getDescription ();
		Button buildButton = roomPanel.transform.Find ("Build Button").GetComponent<Button> ();
		buildButton.onClick.AddListener (delegate {
			closeRoomPanel();
			openBuildingMenu(room);
		});
	}
	public void closeRoomPanel () {
		roomPanel.SetActive (false);
	}

	public void openBuildingMenu (TombRoom room) {
		setHighlightForSelectedTiles (false);
		selectedTiles.Clear();
		currentlyBuilding = room;
		confirmBuildButton.SetActive (true);
	}

	public void closeBuildingMenu () {
		setHighlightForSelectedTiles (false);
		selectedTiles.Clear ();
		currentlyBuilding = null;
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
				reportBuildError ("You need to select a single, contiguous set of tiles.");
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

