using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombRoom {

	private string name;
	private string description;
	private RoomFeature[] features;

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

	public RoomFeature[] getFeatures() {
		return features;
	}
	public void setParts (RoomFeature[] newFeatures) {
		this.features = (RoomFeature[]) newFeatures.Clone();
	}

}

public class RoomFeature {

	private string name;
	private int cost;

	public string getName() {
		return name;
	}
	public void setName (string str) {
		this.name = str;
	}

	public int getCost () {
		return cost;
	}
	public void setCost (int n) {
		this.cost = n;
	}

}
