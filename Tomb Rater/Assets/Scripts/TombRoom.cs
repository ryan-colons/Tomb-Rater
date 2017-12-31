using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombRoom {

	private string name;
	private string description;
	private int minSize = 1;
	private BuildMaterial material;

	//return an error message if the requirements are not met
	//return "" otherwise
	public virtual string meetsRequirements () {
		return "";
	}

	public virtual void onBuild () {
		//gets called once the room is built
	}

	public string getName() {
		return name;
	}
	public void setName (string str) {
		this.name = str;
	}

	public string getDescription () {
		return description;
	}
	public void setDescription (string str) {
		this.description = str;
	}

	public int getMinSize () {
		return minSize;
	}
	public void setMinSize (int newSize) {
		this.minSize = newSize;
	}

	public BuildMaterial getMaterial () {
		return material;
	}
	public void setMaterial (BuildMaterial mat) {
		this.material = mat;
	}

}

//*ROOMS************************************************************
public class Room_Hallway : TombRoom {
	public Room_Hallway () {
		setName ("Hallway");
		setDescription ("A hallway, for connecting rooms.");
		setMinSize (1);
	}
}

public class Room_BurialChamber : TombRoom {
	public Room_BurialChamber () {
		setName ("Burial Chamber");
		setDescription ("A chamber designed to house the sarchophagus of our Glorious Leader.");
		setMinSize (6);
	}
}