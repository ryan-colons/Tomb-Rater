using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/* How to select tiles for building:
 * have a static variable in ManageBuilding (or just MapTile?) that shows when it's selecting time
 * also need to have a button up for cancelling probably?
 */

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

	//this is just here to test the room building code
	private void OnMouseEnter () {
		GameController gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
		ManageBuilding buildingManagement = gameController.getBuildingManagement ();
		BuildTile buildTile = buildingManagement.getTileAtCoord (this.getX (), this.getY ());
		TombRoom room = buildTile.getRoom ();
		if (room != null) {
			Debug.Log (room.getName () + ": " + room.getMaterial ().getName ());
		}
	}

	private void OnMouseDown () {
		//this block is here to allow clicking through the Error Message text
		//it's a bit silly, but hopefully makes for better UI experiences?
		GameObject canvas = GameObject.Find ("Canvas");
		GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster> ();
		PointerEventData pointer = new PointerEventData (EventSystem.current);
		pointer.position = Input.mousePosition;
		List<RaycastResult> resultsList = new List<RaycastResult> ();
		raycaster.Raycast (pointer, resultsList);
		foreach (RaycastResult result in resultsList) {
			if (!result.gameObject.tag.Equals ("Error Message")) {
				return;
			}
		}

		if (BuildingMenu.currentlyBuilding != null) {
			if (!BuildingMenu.selectedTiles.Contains (this)) {
				//add to selected tiles
				setHighlight (true);
				BuildingMenu.selectedTiles.Add (this);
			} else {
				//remove from selected tiles
				setHighlight (false);
				BuildingMenu.selectedTiles.Remove (this);
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
