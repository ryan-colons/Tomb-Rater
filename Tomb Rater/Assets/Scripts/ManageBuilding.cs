using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* TOMB BUILDING IMPLEMENTATION
 * In the building scene, have an isometric grid of tiles (size x)
 * GameController has an x-by-x 2d array of BuildTiles, which have game info about what is on a tile
 * The building scene has an x-by-x 2d array of MapTiles, which handle all in the in-scene stuff
 * These map tiles are attached to the tiles in the isometric grid
 * The building scene uses the BuildTiles to set up the MapTiles correctly.
 * Other scenes can still access important game info, via GameController's array of BuildTiles
 * MORE INFO:
 * Each BuildTile specifies the TombRoom it is part of - multiple BuildTiles can refer to the same TombRoom
 * Each BuildTile has a RoomSection, which indicates how to set up that particular tile (i.e. walls, decorations, etc)
 * PROBLEMS:
 * This could make it hard to change (on the dev side) later (the manual placement of isometric tiles is arduous as heck)
 * It would be very important to ensure the arrays could never fall out of sync
 */

public class ManageBuilding {

	private BuildTile[,] map;
	private int sizeX = 30, sizeY = 60;

	public ManageBuilding () {
		map = new BuildTile[sizeX, sizeY];
		for (int x = 0; x < sizeX; x++) {
			for (int y = 0; y < sizeY; y++) {
				map [x, y] = new BuildTile ();
			}
		}

		//chuck a room in just for testing
		//addRectangularRoom (new TombRoom(), 1, 1, 3, 2);
	}

	public BuildTile getTileAtCoord (int x, int y) {
		if (!(x < 0 || y < 0 || x >= sizeX || y >= sizeY)) {
			return map [x, y];
		} else {
			Debug.Log ("Bad coords!");
			return null;
		}
	}

	public int[] getSizes () {
		return new int[]{ sizeX, sizeY };
	}

	public void addRectangularRoom (TombRoom room, int posX, int posY, int width, int height) {
		bool positionOutOfBounds = (posX < 0 || posY < 0 || posX >= sizeX || posY >= sizeY);
		bool sizeCannotFit = posX + width >= sizeX || posY + height >= sizeY;
		bool valuesTooLow = width <= 0 || height <= 0;
		if (positionOutOfBounds || sizeCannotFit || valuesTooLow) {
			Debug.Log ("Bad coords! Failed to add room!");
			return;
		}

		//set rooms
		for (int x = posX; x < posX + width; x++) {
			for (int y = posY; y < posY + height; y++) {
				map [x, y].setRoom (room);
			}
		}
		//add floors and walls
		//this is done separately so we're sure where all the tiles have ended up (could be worth changing eventually)
		for (int x = posX; x < posX + width; x++) {
			for (int y = posY; y < posY + height; y++) {
				RoomSection section = new RoomSection ();
				bool[] walls = new bool[4];
				BuildTile[] adjacentTiles = getAdjacentTiles (x, y);
				for (int i = 0; i < walls.Length; i++) {
					walls [i] = adjacentTiles != null;
				}
				section.setWalls (walls);
			}
		}
	}

	/* x and y specify a tile by coordinates
	 * this method returns an array of adjacent tiles
	 * the array is ordered north-east-west-south
	 * array is length 4 - empty tiles are null
	 */
	public BuildTile[] getAdjacentTiles (int x, int y) {
		BuildTile[] array = { null, null, null, null };
		if (x < 0 || y < 0 || x >= sizeX || y >= sizeY) {
			return array;
		}
		if (y > 0) {
			array [0] = map [x, y - 1];
		}
		if (x < sizeX - 1) {
			array [1] = map [x + 1, y];
		}
		if (y < sizeY - 1) {
			array [2] = map [x, y + 1];
		}
		if (x > 0) {
			array [3] = map [x - 1, y];
		}
		return array;
	}
}

public class BuildTile {

	//the room that this is part of
	private TombRoom room = null;
	//information about what goes on this particular tile
	private RoomSection section = null;

	public BuildTile () {
	}
	public BuildTile (TombRoom newRoom, RoomSection newSection) {
		room = newRoom;
		section = newSection;
	}


	public TombRoom getRoom () {
		return room;
	}
	public void setRoom (TombRoom newRoom) {
		this.room = newRoom;
	}

	public RoomSection getSection () {
		return section;
	}
	public void setSection (RoomSection newSection) {
		this.section = newSection;
	}

}

public class RoomSection {
	//indicates which sides have walls
	private bool[] walls = { false, false, false, false };
	private Resource wallResource;
	private Resource floorResource;

	public bool[] getWalls () {
		return walls;
	}
	public void setWalls (bool[] b) {
		if (b.Length == 4) {
			walls = b;
		} else {
			Debug.Log ("Something has probably gone wrong! Does this tile really not have exactly 4 sides?");
			Application.Quit ();
		}
	}

	public Resource getFloorResource () {
		return floorResource;
	}
	public void setFloorResource (Resource res) {
		this.floorResource = res;
	}

	public Resource getWallResource () {
		return wallResource;
	}
	public void setWallResource (Resource res) {
		this.wallResource = res;
	}
}