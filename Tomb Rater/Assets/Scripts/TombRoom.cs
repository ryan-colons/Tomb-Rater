using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombRoom {

	private string name;
	private string description;
	private int minSize = 1;
	private BuildMaterial material;
	private List<Treasure> treasures = new List<Treasure> ();
	private List<TreasureType> treasureBonuses = new List<TreasureType> ();

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

	public List<Treasure> getTreasureList () {
		return treasures;
	}
	public List<TreasureType> getTreasureBonuses () {
		return treasureBonuses;
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
		getTreasureBonuses ().Add (TreasureType.BURIAL);
	}
}

public class Room_TreasureVault : TombRoom {
	public Room_TreasureVault() {
		setName ("Treasure Vault");
		setDescription ("Show off your riches in this ostentatious, but secure, vault.");
		setMinSize (4);
		getTreasureBonuses ().Add (TreasureType.RICHES);
	}
}

public class Room_ServantBurialChamber : TombRoom {
	public Room_ServantBurialChamber () {
		setName ("Servant Burial Chamber");
		setDescription ("It just wouldn't be right to leave this mortal coil without your trusty Advisors. " +
		"Entomb them with you in this cosy death chamber!");
		setMinSize (2);
		getTreasureBonuses ().Add (TreasureType.BURIAL);
	}
	public override void onBuild () {
		//game prepares to bury Advisors
		//Advisors may get worried
	}
}

public class Room_FalseBurialChamber : TombRoom {
	public Room_FalseBurialChamber () {
		setName ("False Burial Chamber");
		setDescription ("Keep the greasy mitts of grave robbers off your beautiful corpse with a misleading decoy!");
		setMinSize (2);
		getTreasureBonuses ().Add (TreasureType.BURIAL);
	}
}

public class Room_HallOfHeroes : TombRoom {
	public Room_HallOfHeroes () {
		setName ("Hall of Heroes");
		setDescription ("A magnificent room in which to praise/honor your ancestors, allies and other great heroes.");
		setMinSize (2);
		getTreasureBonuses ().Add (TreasureType.HISTORICAL);

	}
}

public class Room_DungeonOfMockery : TombRoom {
	public Room_DungeonOfMockery () {
		setName ("Dungeon of Mockery");
		setDescription ("This chamber is dedicated to mocking all the enemies you had in life. Get the last laugh!");
		setMinSize (2);
		getTreasureBonuses ().Add (TreasureType.HISTORICAL);
	}
}