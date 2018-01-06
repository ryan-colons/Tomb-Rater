﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvisorMilestone {

	public static GameController gameController;

	//how much money BEYOND the previous node is required to unlock this
	private int threshold;
	//how much money has been sunk into this milestone already
	private int payment;
	//this string is how the advisor explains/sells the milestone
	private string description;
	private AdvisorMilestone nextMilestone;

	public string getDescription () {
		return this.description;
	}
	public void setDescription (string str) {
		this.description = str;
	}

	public int getThreshold () {
		return this.threshold;
	}
	public void setThreshold (int n) {
		this.threshold = n;
	}

	public AdvisorMilestone getNextMilestone () {
		return this.nextMilestone;
	}
	public void setNextMilestone (AdvisorMilestone next) {
		this.nextMilestone = next;
	}

	//return string for yearly report
	public virtual string reward () {
		return "";
	}

	//returns the overflow (money spent beyond the threshold
	public int pay (int newPayment) {
		int overflow = (payment + newPayment) - threshold;
		payment = newPayment;
		return overflow;
	}
	public int getPayment () {
		return payment;
	}
}

/*
 * MILITARY MILESTONES
 *  50g (50)  - fortify walls, +defence against invasion
 * 100g (50)  - mount a raid against a nearby rival (% chance of gaining a lump sum, special event)
 * 200g (100) - capture a nearby mine currently used by a rival (% chance of gaining GPT, special event)
 * 400g (200) - pillage a nearby heretical/barbarous temple (% chance of gaining gold + treasures)
 * 700g (300) - 
 *1000g (300) - 
 *
 * ECONOMIC MILESTONES
 *  50g - build some fishing boats (GPT increase)
 * 100g - improve road/tools at mining camp (GPT increase)
 * 150g - open trade route (trade special event every 3 years for 12 years)
 * 300g - special event: buy slaves (+GPT, -rep) or free slaves (%chance (+mil) to gain gold, +rep)
 * 
 * GUILD REPRESENTATIVE
 *  50g - unlock Marble brick as a building material
 * 100g - gain treasure from a guild of your choice (special event)
 * 150g - gain treasure from a guild of your choice (special event)
 * 300g - add murals to all your hallways (existing and future)
 */



public class GM_UnlockMarble : AdvisorMilestone {
	public GM_UnlockMarble () {
		setThreshold (50);
		setDescription ("Happy Birthday, Your Grace! I have been in talks with the Guilds of the Kingdom, to investigate " +
		"the ways in which they can contribute to the construction of your Tomb. The Masonry Guild will construct and work " +
		"a new quarry to provide access to marble brick. They just need some funding.");
	}
	public override string reward () {
		ManageBuilding buildingManagement = gameController.getBuildingManagement ();
		buildingManagement.addAvailableMaterial (new Mat_Marble ());
		return "The Masonry Guild has opened a new marble quarry. We can now use marble brick in the construction of the Tomb.";
	}
}

public class GM_GuildTreasure : AdvisorMilestone {
	public GM_GuildTreasure () {

	}
	public override string reward () {
		return "";
	}
}

public class EM_BuildBoats : AdvisorMilestone {
	public EM_BuildBoats () {
		setThreshold (50);
		setDescription ("Your Highness, the nearby lakes hold untapped economic potential! We should invest in a fleet " +
		"of fishing boats. This would bring in a little extra money every year. It's well worth it in the long run!");
		setNextMilestone (new EM_ImproveMines ());
	}
	public override string reward () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		advisorManagement.setGPT (advisorManagement.getGPT () + 10);
		return "We now have a fleet of fishing boats, extracting valuables from nearby lakes. This will bring in " +
		"an extra 10g per year.";
	}
}

public class EM_ImproveMines : AdvisorMilestone {
	public EM_ImproveMines () {
		setThreshold (50);
		setDescription ("Happy Birthday, Your Highness. The workers from our silver mines have been complaining in recent years. " +
			"The roads around the mines and poorly maintained, and their tools are broken and outdated. If you wish to invest some " +
			"money, we can get the mines running properly again, bringing in more money for the Kingdom each year.");
		setNextMilestone (new EM_OpenTradeRoute ());
	}
	public override string reward () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		advisorManagement.setGPT (advisorManagement.getGPT () + 10);
		return "Significant improvements have been made to our mining operations. This will bring in an extra 10g per year.";
	}
}

public class EM_OpenTradeRoute : AdvisorMilestone {
	public EM_OpenTradeRoute () {
		setThreshold (50);
		setDescription ("Happy Birthday, Your Highness! I have been in contact with a group of travelling traders, from " +
		gameController.getTradeCivName () + ". For a small fee, they will include our Kingdom in their trading route, which " +
		"makes its rounds every three years. " + gameController.getTradeCivName () + " is known for its many fine treasures!");
	}
	public override string reward () {
		ManageYears yearManagement = gameController.getYearManagement ();
		yearManagement.addSpecialEventInXYears (new Event_TradeOpportunity (), 3);
		yearManagement.addSpecialEventInXYears (new Event_TradeOpportunity (), 6);
		yearManagement.addSpecialEventInXYears (new Event_TradeOpportunity (), 9);
		yearManagement.addSpecialEventInXYears (new Event_TradeOpportunity (), 12);
		return "Traders from " + gameController.getTradeCivName () + " will now stop by every 3 years, for the next 12 years.";
	}
}

public class EM_GeneratedMilestone : AdvisorMilestone {
	private string[] investments = new string[] {
		"eagle feather pillows",
		"double-ended pickaxes",
		"cloud juicing",
		"video games (whatever that is)", 
		"all-you-can-eat restaurants",
		"*actual* pyramid schemes",
		"funny t-shirts",
		"ostrich farming",
		"carp racing",
		"puppet shows"
	};
	private string scheme;
	public EM_GeneratedMilestone () {
		CharacterData info = gameController.getCharData ();
		scheme = investments [Random.Range (0, investments.Length)];
		setThreshold (75);
		setDescription ("Happy Birthday, " + info.getPlayerTitle () + " " + info.getPlayerName () + ". We have an excellent opportunity " +
		"to invest in " + this.scheme + ". If you put aside a little money now, we can bring in " +
		"extra funds for tomb building every year going forward.");
	}
	public override string reward () {
		this.setNextMilestone (new EM_GeneratedMilestone ());
		int boon = Random.Range (10, 15);
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		advisorManagement.setGPT (advisorManagement.getGPT () + boon);
		return "We made some shrewd investments in " + this.scheme + ". This will bring in an extra " + boon + " gold per year.";
	}
}

public class MM_FortifyWalls : AdvisorMilestone {
	public MM_FortifyWalls () {
		setThreshold (50);
		setDescription ("Your Majesty, our enemies are always conspiring against us. They want to steal your glory! " +
			"Your soldiers stand ready to fight them, but the city walls are falling into disrepair... for a small price," +
			"we can fortify the walls and station ballistae on our fearsome guard towers!");
		
	}
	public override string reward () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		advisorManagement.setDefensiveMight (advisorManagement.getDefensiveMight () + 30);
		return "The walls around your Kingdom were fortified and stationed with guards.";
	}
}

public class MM_RaidRival : AdvisorMilestone {
	public MM_RaidRival () {
		setThreshold (50);
		setDescription ("Happy Birthday, Your Majesty. Our rivals, from " + gameController.getRivalCivName() + " have an encampment not far from here - " +
			"it is full of resources, but poorly defended. We should mount up an expedition to take what we can! " +
			"It would be nice of us to teach them the importance of a strong defence.");
	}
	public override string reward () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		if (Random.Range (0, 100) <= advisorManagement.getOffensiveMight ()) {
			//success!
			gameController.setMoney(gameController.getMoney() + 125);
			return "Our raid against the rival encampment was a great success. We plundered 125g!";
		} else {
			//failure
			return "We sent a raid against the rival encampment. Unfortunately, they saw us coming and managed to retreat.";
		}
	}
}
	
public class MM_CaptureMine : AdvisorMilestone {
	public MM_CaptureMine () {
		setThreshold (100);
		setDescription ("Our enemies have set up a mining camp, far beyond the walls of their city. It would be much " +
			"easier for us to maintain and defend it. We should send a squadron of soldiers to capture it. The mine " +
			"contains many valuable minerals, and would bring in extra money for the Kingdom each year.");
	}
	public override string reward () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		if (Random.Range (0, 100) <= advisorManagement.getOffensiveMight ()) {
			//success!
			advisorManagement.setGPT (advisorManagement.getGPT() + 30);
			return "We successfully captured the enemy mine! This will bring in an extra 30g every year.";
		} else {
			//failure
			return "Our mission to capture the enemy mine failed.";
		}
	}
}

public class MM_GeneratedMilestone : AdvisorMilestone {
	private string[] raidLocations = new string[]{
		"a rival encampment"
	};
	private string[] raidGoods = new string[]{
		"fine cheeses", "valuable silks", "gems", "gold", "silver", "baboon-fur coats"
	};
	private string mission;
	public MM_GeneratedMilestone () {
		setThreshold (75);
		mission = raidLocations[Random.Range(0, raidLocations.Length)];
		CharacterData info = gameController.getCharData ();
		setDescription (info.getPlayerTitle () + " " + info.getPlayerName () + ", we have an opportunity at hand. " +
		"There is a stockpile of " + raidGoods [Random.Range (0, raidGoods.Length)] + " at " + mission + ". " +
		"The mission is somewhat risky, but if we can put together a well-equipped raid team, we can take it.");
	}
	public override string reward () {
		setNextMilestone (new MM_GeneratedMilestone ());
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		if (Random.Range (0, 100) <= advisorManagement.getOffensiveMight ()) {
			//success!
			gameController.setMoney(gameController.getMoney() + 200);
			return "We raided " + mission + " and successfully plundered 200g!";
		} else {
			//failure
			return "Our raid at " + mission + " was a failure.";
		}
	}
}