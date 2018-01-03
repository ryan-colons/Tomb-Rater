using System.Collections;
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

public class EM_BuildBoats : AdvisorMilestone {
	public EM_BuildBoats () {
		setThreshold (50);
		setDescription ("Your Highness, the nearby lakes hold untapped economic potential! We should invest in a fleet " +
		"of fishing boats. This would bring in a little extra money every year. It's well worth it in the long run!");
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
	}
	public override string reward () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		advisorManagement.setGPT (advisorManagement.getGPT () + 10);
		return "Significant improvements have been made to our mining operations. This will bring in an extra 10g per year.";
	}
}
// NOT FINISHED, OBVIOUSLY
public class EM_OpenTradeRoute : AdvisorMilestone {
	public EM_OpenTradeRoute () {
		setThreshold (50);
		setDescription ("Happy Birthday, Your Highness! ");
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
		setDescription ("Happy Birthday, Your Majesty. Our rivals have an encampment not far from here - " +
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