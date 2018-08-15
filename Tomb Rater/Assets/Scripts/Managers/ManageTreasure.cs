using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageTreasure {

	private List<Treasure> treasureList;

	public ManageTreasure () {
		treasureList = new List<Treasure> ();
	}

	public List<Treasure> getTreasureList () {
		return treasureList;
	}

	public void setTreasureList (List<Treasure> list) {
		treasureList = list;
	}
}
