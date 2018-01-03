using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure {

	private string name;
	private string desc;
	private Sprite sprite;
	private int baseValue;
	private List<TreasureType> typeList;

	public static GameController gameController;

	public void setName (string newName) {
		name = newName;
	}
	public string getName () {
		return name;
	}

	public void setDesc (string newDesc) {
		desc = newDesc;
	}
	public string getDesc () {
		return desc;
	}

	public void setSprite (string sprName) {
		sprite = Resources.Load ("Treasure Sprites/" + sprName, typeof(Sprite)) as Sprite;
	} 
	public Sprite getSprite () {
		return this.sprite;
	}

	public void setBaseValue (int newValue) {
		baseValue = newValue;
	}
	public int getBaseValue () {
		return baseValue;
	}

	public void addType (TreasureType newType) {
		if (typeList == null) {
			typeList = new List<TreasureType> ();
		}
		if (!typeList.Contains (newType)) {
			typeList.Add (newType);
		}
	}
	public List<TreasureType> getTypeList () {
		return typeList;
	}

	public virtual void onRate () {
		//called when the tomb is finally rated
		//probably?
	}
}

public enum TreasureType {
	RICHES,
	BURIAL,
	MECHANICAL,
	HISTORICAL,
	RELIGIOUS,
	MAGICAL
}

/* This was just made for testing, not really good enough for the actual game. */
public class Tre_Pawn : Treasure {
	public Tre_Pawn () {
		setName ("Pawn");
		setSprite ("worker_token");
	}
}

