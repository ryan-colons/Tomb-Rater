using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageTreasure {

	private List<Treasure> treasureList;

	public ManageTreasure () {
		treasureList = new List<Treasure> ();
		treasureList.Add (new Tre_Pawn ());
		treasureList.Add (new Tre_Pawn ());
		treasureList.Add (new Tre_Pawn ());
	}

	public List<Treasure> getTreasureList () {
		return treasureList;
	}

}
