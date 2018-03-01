using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
		payment += newPayment;
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
[System.Serializable]
public class GM_UnlockMarble : AdvisorMilestone {
	public GM_UnlockMarble () {
		setThreshold (50);
		setDescription ("Happy Birthday, Your Grace! I have been in talks with the Guilds of the Kingdom, to investigate " +
		"the ways in which they can contribute to the construction of your Tomb. The Masonry Guild will construct and work " +
		"a new quarry to provide access to marble brick. They just need some funding.");
		setNextMilestone (new GM_GuildTreasure ());
	}
	public override string reward () {
		ManageBuilding buildingManagement = gameController.getBuildingManagement ();
		buildingManagement.addAvailableMaterial (new Mat_Marble ());
		return "The Masonry Guild has opened a new marble quarry. We can now use marble brick in the construction of the Tomb.";
	}
}

[System.Serializable]
public class GM_GuildTreasure : AdvisorMilestone {
	public GM_GuildTreasure () {
		CharacterData info = gameController.getCharData ();
		setThreshold (50);
		setDescription ("Your Grace, the Guilds of " + info.getKingdomName () + " have much to offer. For a small price, we " +
		"can commission a work of art for your Tomb.");
		setNextMilestone (new GM_AnotherGuildTreasure ());
	}
	public override string reward () {
		ManageYears yearManagement = gameController.getYearManagement ();
		yearManagement.addSpecialEventInXYears (new Event_GuildTreasure (), 0);
		return "";
	}
}

[System.Serializable]
public class GM_AnotherGuildTreasure : AdvisorMilestone {
	public GM_AnotherGuildTreasure () {
		CharacterData info = gameController.getCharData ();
		setThreshold (50);
		setDescription ("Happy Birthday, " + info.getPlayerName () + "! There are a lot of excited artisans waiting to contribute to " +
		"your Tomb! Let's commission another fine piece of art, shall we?");
		setNextMilestone (new GM_AddMurals ());
	}
	public override string reward () {
		ManageYears yearManagement = gameController.getYearManagement ();
		yearManagement.addSpecialEventInXYears (new Event_GuildTreasure (), 0);
		return "";
	}
}

[System.Serializable]
public class GM_AddMurals : AdvisorMilestone {
	public GM_AddMurals () {
		setThreshold (150);
		setDescription ("I have been in contact with the Masonry Guild. In exchange for some funding, " +
			"they will carve murals into all of the Hallways in your tomb. That includes Hallways built " +
			"between now and your death! I suggest we take them up on their offer.");
		setNextMilestone (new GM_ForeignGuildTreasure ());
	}
	public override string reward () {
		ManageBuilding buildingManagement = gameController.getBuildingManagement ();
		for (int x = 0; x < buildingManagement.getSizes () [0]; x++) {
			for (int y = 0; y < buildingManagement.getSizes () [1]; y++) {
				BuildTile buildTile = buildingManagement.getTileAtCoord (x, y);
				TombRoom existingRoom = buildTile.getRoom ();
				if (existingRoom != null && existingRoom.getName ().Equals ("Hallway")) {
					if (!existingRoom.containsTreasure (new Tre_Mural ())) {
						existingRoom.getTreasureList ().Add (new Tre_Mural ());
					}
				}
			}
		}
		buildingManagement.makeRoomUnavailableToBuild (new Room_Hallway ());
		buildingManagement.makeRoomAvailableToBuild (new Room_MuralHallway ());
		ManageYears yearManagement = gameController.getYearManagement ();
		yearManagement.addSpecialEventInXYears (new Event_ImmigratingArtists (), 0);
		return "All the hallways of your Tomb will now be covered in carved murals.";
	}
}

[System.Serializable]
public class GM_ForeignGuildTreasure : AdvisorMilestone {
	public GM_ForeignGuildTreasure () {
		CharacterData info = gameController.getCharData ();
		setThreshold (50);
		setDescription ("Your Grace, " + info.getKingdomName() + " is crawling with creative artisans from across the world! " +
			"Now would be an excellent time to commission a work of art for your tomb. Lots of exciting new art forms have made " +
			"their way here...");
		setNextMilestone (new GM_FundNewGuild ());
	}
	public override string reward () {
		ManageYears yearManagement = gameController.getYearManagement ();
		yearManagement.addSpecialEventInXYears (new Event_ForeignGuildTreasure (), 0);
		return "";
	}
}

[System.Serializable]
public class GM_FundNewGuild : AdvisorMilestone {
	public GM_FundNewGuild () {
		CharacterData info = gameController.getCharData ();
		setThreshold (150);
		setDescription ("Your Grace, we must make the most of the new talents that have arrived in " + info.getKingdomName () + ". " +
		"I think it is time for a new guild to be founded. With enough funding, we can ensure that these exciting new crafts " +
		"thrive here. That could only be a good thing.");
		setNextMilestone (new GM_FundAnotherNewGuild ());
	}
	public override string reward () {
		ManageYears yearManagement = gameController.getYearManagement ();
		yearManagement.addSpecialEventInXYears (new Event_FoundNewGuild (), 0);
		return "";
	}
}

[System.Serializable]
public class GM_FundAnotherNewGuild : AdvisorMilestone {
	public GM_FundAnotherNewGuild () {
		CharacterData info = gameController.getCharData ();
		setThreshold (500);
		setDescription ("Happy Birthday, " + info.getPlayerTitle () + " " + info.getPlayerName () + "! " + info.getKingdomName () + " " +
			"is fast becoming the world capital for culture, art, and innovation! It may be expensive, but I think we should have another " +
			"new guild founded.");
		setNextMilestone (new GM_GeneratedMilestone ());
	}
	public override string reward () {
		ManageYears yearManagement = gameController.getYearManagement ();
		yearManagement.addSpecialEventInXYears (new Event_FoundAnotherNewGuild (), 0);
		return "";
	}
}

[System.Serializable]
public class GM_GeneratedMilestone : AdvisorMilestone {
	public GM_GeneratedMilestone () {
		CharacterData info = gameController.getCharData ();
		setThreshold (50);
		setDescription ("Your Grace, the Guilds of " + info.getKingdomName () + " have much to offer. For a small price, we " +
			"can commission a work of art for your Tomb.");
		
	}
	public override string reward () {
		this.setNextMilestone (new GM_GeneratedMilestone ());
		ManageYears yearManagement = gameController.getYearManagement ();
		yearManagement.addSpecialEventInXYears (new Event_GuildTreasure (), 0);
		return "";
	}
}

[System.Serializable]
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

[System.Serializable]
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

[System.Serializable]
public class EM_OpenTradeRoute : AdvisorMilestone {
	public EM_OpenTradeRoute () {
		setThreshold (50);
		setDescription ("Happy Birthday, Your Highness! I have been in contact with a group of travelling traders, from " +
		gameController.getTradeCivName () + ". For a small fee, they will include our Kingdom in their trading route, which " +
		"makes its rounds every three years. " + gameController.getTradeCivName () + " is known for its many fine treasures!");
		setNextMilestone (new EM_GeneratedMilestone ());
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

[System.Serializable]
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

[System.Serializable]
public class MM_FortifyWalls : AdvisorMilestone {
	public MM_FortifyWalls () {
		setThreshold (50);
		setDescription ("Your Majesty, our enemies are always conspiring against us. They want to steal your glory! " +
			"Your soldiers stand ready to fight them, but the city walls are falling into disrepair... for a small price," +
			"we can fortify the walls and station ballistae on our fearsome guard towers!");
		setNextMilestone (new MM_RaidRival ());
		
	}
	public override string reward () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		advisorManagement.setDefensiveMight (advisorManagement.getDefensiveMight () + 30);
		return "The walls around your Kingdom were fortified and stationed with guards.";
	}
}

[System.Serializable]
public class MM_RaidRival : AdvisorMilestone {
	public MM_RaidRival () {
		setThreshold (50);
		setDescription ("Happy Birthday, Your Majesty. Our rivals, from " + gameController.getRivalCivName() + " have an encampment not far from here - " +
			"it is full of resources, but poorly defended. We should mount up an expedition to take what we can! " +
			"It would be nice of us to teach them the importance of a strong defence.");
		setNextMilestone (new MM_CaptureMine ());
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
	
[System.Serializable]
public class MM_CaptureMine : AdvisorMilestone {
	public MM_CaptureMine () {
		setThreshold (100);
		setDescription ("Our enemies have set up a mining camp, far beyond the walls of their city. It would be much " +
			"easier for us to maintain and defend it. We should send a squadron of soldiers to capture it. The mine " +
			"contains many valuable minerals, and would bring in extra money for the Kingdom each year.");
		setNextMilestone (new MM_GeneratedMilestone ());
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

[System.Serializable]
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
	
[System.Serializable]
public class NM_DabbleNecromancy : AdvisorMilestone {
	public NM_DabbleNecromancy () {
		setThreshold (75);
		setDescription ("Good day, Your Excellence. My colleagues wish to perform some... research. We require " +
			"certain tools that are expensive to acquire. If you were to provide us with some additional funding, " +
			"we could happily return a favour. We can acquire things for your Tomb in which you will be quite interested.");
		setNextMilestone (new NM_ZombieWorkers ());
	}
	public override string reward () {
		ManageSpecialEvents specialEventManagement = gameController.getSpecialEventManagement ();
		specialEventManagement.addPossibleEvent (new Event_NecromancerRitualsAreWeird ());
		ManageYears yearManagement = gameController.getYearManagement ();
		yearManagement.addSpecialEventInXYears (new Event_NecromancerDabblingGifts (), 0);
		return "";
	}
}

[System.Serializable]
public class NM_ZombieWorkers : AdvisorMilestone {
	public NM_ZombieWorkers () {
		setThreshold (75);
		setDescription ("Happy Birthday, Your Excellence...\nMy colleagues and I wish to extend an offer. If you provide us " +
			"funding for our studies... we can help you. Your perished citizens are just laying in graveyards - they should be " +
			"working for you! We will remind them of their responsibilities.");
		setNextMilestone (new NM_BecomeLich ());
	}
	public override string reward () {
		// add possible event about zombies running around
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		advisorManagement.setGPT (advisorManagement.getGPT () + 20);
		string econAdvisor = "Your economic advisors ";
		if (advisorManagement.getAdvisors () [ManageAdvisors.ECONOMY] != null) {
			econAdvisor = advisorManagement.getAdvisors () [ManageAdvisors.ECONOMY].getName ();
		}
		ManageOpinion opinionManagement = gameController.getOpinionManagement ();
		opinionManagement.incrementFavour (-1);
		return "Recently dead citizens have risen from the grave to till your fields. " + econAdvisor +
		" predicts an increase of 20 gold per year, from the extra productivity. The other farmers " +
		"seemed to disapprove a little.";
	}
}

[System.Serializable]
public class NM_BecomeLich : AdvisorMilestone {
	public NM_BecomeLich () {
		setThreshold (100);
		setDescription ("You have been good to us, Your Excellence. You wish to thank you... " +
		"We have plans for a ritual that can hide a person from death... If you can simply provide us " +
		"with some materials, we will try - I mean, perform - the ritual on you.");
	}
	public override string reward () {
		ManageYears yearManagement = gameController.getYearManagement ();
		yearManagement.addSpecialEventInXYears (new Event_BecomeLich (), 0);
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		string necroName = advisorManagement.getAdvisors () [ManageAdvisors.NECRO].getName ();
		advisorManagement.getAdvisors () [ManageAdvisors.NECRO] = null;
		return necroName + " has left, presumably in pursuit of otherwordly knowledge.";
	}
}
	
[System.Serializable]
public class PM_ThrowParty : AdvisorMilestone {
	public PM_ThrowParty () {
		setThreshold (50);
		setDescription ("Oh hey, Your... uh, Graciousness. Listen, me and the others were thinking " +
			"about throwing a big party in the town hall. Invite the whole city, y'know? We kinda need " +
			"some cash though, for like drinks and stuff... What do you say?");
		setNextMilestone (new PM_TombTavern ());
	}
	public override string reward () {
		ManageOpinion opinionManagement = gameController.getOpinionManagement ();
		opinionManagement.incrementFavour (1);
		return "Citizens are raving about an amazing party that was thrown in your name this year.";
	}
}

[System.Serializable]
public class PM_TombTavern : AdvisorMilestone {
	public PM_TombTavern () {
		CharacterData info = gameController.getCharData ();
		setThreshold (50);
		setDescription ("Hey, it's the " + info.getPlayerTitle() + "! Happy Birthday! Hey, I was thinking " +
			"the other day about your whole tomb thing, and I was like, man it'd be sick to hang out there. " +
			"Pretty sweet venue for gigs and stuff, y'know? Hit me up if you're interested, we could hook you " +
			"up with some designs and stuff. Something to think about!");
		setNextMilestone (new PM_BecomeALegend ());
	}
	public override string reward () {
		//add new tomb rooms (and treasures maybe?)
		ManageBuilding buildingManagement = gameController.getBuildingManagement();
		buildingManagement.makeRoomAvailableToBuild (new Room_Tavern ());
		buildingManagement.makeRoomAvailableToBuild (new Room_SpeakeasyEntrance ());
		ManageTreasure treasureManagement = gameController.getTreasureManagement ();
		treasureManagement.getTreasureList ().Add (new Tre_BlackLightCandle ());
		treasureManagement.getTreasureList ().Add (new Tre_BlackLightCandle ());
		treasureManagement.getTreasureList ().Add (new Tre_ClockworkDrums ());
		treasureManagement.getTreasureList ().Add (new Tre_ClockworkDrums ());
		return "Your building team has been in talks with the Rave Society.";
	}
}

[System.Serializable]
public class PM_BecomeALegend : AdvisorMilestone {
	public PM_BecomeALegend () {
		setThreshold (50);
		setDescription ("Hey, what's up Your Honour? So, me and the squad were talking about " +
			"how chill you've been, and we know you really wanna leave a big legacy behind and " +
			"all that good stuff, and word of mouth is real helpful, y'know? Basically, we want to do " +
			"a big world tour, throwing parties and telling everyone how tight your reign is. Sounds good right??");	
	}
	public override string reward () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		string partyName = advisorManagement.getAdvisors () [ManageAdvisors.PARTY].getName ();
		advisorManagement.getAdvisors () [ManageAdvisors.PARTY] = null;
		ManageOpinion opinion = gameController.getOpinionManagement ();
		opinion.incrementFavour (3);
		return partyName + " is now on a world tour, spreading favourable stories about you.";
	}
}

