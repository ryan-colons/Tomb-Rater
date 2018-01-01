﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMaterial {
	private string name;
	private int costPerTile;

	public void setName (string str) {
		this.name = str;
	}
	public string getName () {
		return this.name;
	}

	public void setCostPerTile (int n) {
		this.costPerTile = n;
	}
	public int getCostPerTile () {
		return this.costPerTile;
	}

	public bool isSameAs (BuildMaterial otherMat) {
		if (this.getName().Equals(otherMat.getName())) {
			return true;
		} else {
			return false;
		}
	}
}

public class Mat_Marble : BuildMaterial {
	public Mat_Marble () {
		setName ("Marble Brick");
		setCostPerTile (10);
	}
}

public class Mat_Clay : BuildMaterial {
	public Mat_Clay () {
		setName ("Clay Brick");
		setCostPerTile (5);
	}
}