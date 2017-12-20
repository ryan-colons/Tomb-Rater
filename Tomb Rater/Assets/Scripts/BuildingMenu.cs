using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMenu : MonoBehaviour {

	private GameController gameController;
	private ManageBuilding buildingManagement;
	//public GameObject tutPanel;
	public GameObject helpPanel;
	//public GameObject advicePanel;

	public GameObject tilePrefab;
	public Sprite wallSpr0, wallSpr1, wallSpr2, wallSpr3;



	private const int MAP_SIZE = 10;
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

	public void returnToOverMenu () {
		gameController.loadScene ("menu");
	}
}

/* THIS IS OLD CODE FOR DYNAMIC TILE PLACEMENT; IT CAN PROBABLY BE DELETED
//private MapTile[,] tiledMap;
//public int tileSizeX, tileSizeY;
//instatiate tiles according to buildingManagement
int[] maxSizes = buildingManagement.getSizes();
Transform tileParent = GameObject.Find ("Map Tiles").transform;
for (int x = 0; x < maxSizes [0]; x++) {
	for (int y = 0; y < maxSizes [1]; y++) {
		//instatiate at position
		GameObject newTile = (GameObject)Instantiate (tilePrefab, tileParent);
		if (y % 2 == 0) {
			newTile.transform.position = new Vector3 (x * tileSizeX + (tileSizeX * 0.5f), y * tileSizeY, 0);
		} else {
			newTile.transform.position = new Vector3 (x * tileSizeX, y * tileSizeY, 0);
		}
		//set appropriate sprite
		BuildTile buildTile = buildingManagement.getTileAtCoord (x, y);
		if (buildTile.getRoom () != null) {
			bool[] tileWalls = buildTile.getSection ().getWalls ();
			Sprite spriteToUse = wallSpr0;
			if (tileWalls [0]) {
				spriteToUse = wallSpr1;
				if (tileWalls [1]) {
					spriteToUse = wallSpr3;
				}
			} else {
				spriteToUse = wallSpr2;
			}
			MapTile mapTile = newTile.GetComponent<MapTile> ();
			mapTile.setSprite (spriteToUse);
		}
	}
}*/
