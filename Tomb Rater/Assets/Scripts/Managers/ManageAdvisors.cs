using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageAdvisors {
	public const int ECONOMY = 0;
	public const int MILITARY = 1;
	public const int GUILDS = 2;
	public const int NECRO = 3;
	public const int PARTY = 4;
	public const int TEMPLE = 5;

	private int goldPerTurn = 0;
	//percent chance of succeeding in a (normal) military event
	private int offensiveMight = 40;
	//percent chance of defending against invasion etc
	private int defensiveMight = 10;
	//guild advisor should give a +chance to get treasure special events

	private Advisor[] advisors;

	public ManageAdvisors () {
		advisors = new Advisor[6];
		advisors [ECONOMY] = new Advisor ();
		advisors [MILITARY] = new Advisor ();
		advisors [GUILDS] = new Advisor ();

		advisors [ECONOMY].setMilestone (new EM_BuildBoats ());
		advisors [MILITARY].setMilestone (new MM_FortifyWalls ());
		advisors [GUILDS].setMilestone (new GM_UnlockMarble ());

		advisors [ECONOMY].setTutorial (new Event_EconomicAdvisorTutorial (advisors[ECONOMY]));
		advisors [MILITARY].setTutorial (new Event_MilitaryAdvisorTutorial (advisors[MILITARY]));
		advisors [GUILDS].setTutorial (new Event_GuildAdvisorTutorial (advisors[GUILDS]));
		goldPerTurn = 20;
	}

	public Advisor[] getAdvisors () {
		return advisors;
	}
	public void setAdvisors (Advisor[] array) {
		advisors = array;
	}

	public int getGPT () {
		return goldPerTurn;
	}
	public void setGPT (int gpt) {
		this.goldPerTurn = gpt;
	}
		
	public void setOffensiveMight (int n) {
		offensiveMight = n;
	}
	public int getOffensiveMight () {
		return offensiveMight;
	}
	public void setDefensiveMight (int n) {
		defensiveMight = n;
	}
	public int getDefensiveMight () {
		return defensiveMight;
	}

	public int getMight () {
		return offensiveMight + defensiveMight;
	}	
}
