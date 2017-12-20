using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class MapTile : MonoBehaviour {

	private GameObject north, east, west, south;
	private SpriteRenderer sprRenderer;
	private Color actualColor;
	private TombRoom room;
	public int xCoord, yCoord;
	public static float xSize = 4, ySize = 2;
	public static int gridSize = 9; //subtract 1 from actual grid size

	private void Start () {
		sprRenderer = this.GetComponent<SpriteRenderer> ();
		actualColor = sprRenderer.color;
		BuildingMenu buildingMenu = GameObject.Find ("Canvas").GetComponent<BuildingMenu> ();
		buildingMenu.setTileAtCoord (this.gameObject, xCoord, yCoord);
	}

	private void OnValidate() {
		//place tile according to xCoord and yCoord
		if (!Application.isPlaying) {
			this.name = xCoord + " " + yCoord;
			float xOffset = (float)gridSize * xSize * 0.5f;
			//float yOffset = (float)gridSize * ySize * 0.5f;

			float xPos = (xCoord + yCoord) * (xSize / 2f);
			float yPos = (xCoord - yCoord) * (ySize / 2f);
			float zPos = xCoord - yCoord;
			this.transform.position = new Vector3 (xPos - xOffset, yPos, zPos);
		}
	}

	public TombRoom getRoom () {
		return room;
	}
	public void setRoom (TombRoom newRoom) {
		this.room = newRoom;
	}
		
	public GameObject[] getAdjacentObjects () {
		return new GameObject[]{north, east, west, south};
	}
	public MapTile[] getAdjacentTiles () {
		GameObject[] adjacentsObjs = getAdjacentObjects ();
		MapTile[] adjacentTiles = new MapTile[adjacentsObjs.Length];
		for (int i = 0; i < adjacentsObjs.Length; i++) {
			if (adjacentsObjs [i] != null) {
				adjacentTiles [i] = adjacentsObjs [i].GetComponent<MapTile> ();
			} else {
				adjacentTiles [i] = null;
			}
		}
		return adjacentTiles;
	}


	public void setAdjacentObjects (GameObject n, GameObject e, GameObject w, GameObject s) {
		north = n;
		east = e;
		west = w;
		south = s;
	}

	public void setHighlight (bool highlight) {
		if (highlight) {
			this.sprRenderer.color = Color.blue;
		} else {
			this.sprRenderer.color = actualColor;
		}
	}

	public void setSprite (Sprite newSprite) {
		sprRenderer.sprite = newSprite;
	}

	private void OnMouseDown () {
		GameObject[] neighbours = getAdjacentObjects ();
		foreach (GameObject tile in neighbours) {
			if (tile != null) {
				tile.GetComponent<MapTile> ().setHighlight (true);
			}
		}
	}

	private void OnMouseUp () {
		GameObject[] neighbours = getAdjacentObjects ();
		foreach (GameObject tile in neighbours) {
			if (tile != null) {
				tile.GetComponent<MapTile> ().setHighlight (false);
			}
		}
	}

	public int getX () {
		return xCoord;
	}
	public int getY () {
		return yCoord;
	}

}
