using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageAdvisors {
	public const int ECONOMY = 0;
	public const int MILITARY = 1;
	public const int GUILDS = 2;

	private int goldPerTurn = 0;
	//percent chance of succeeding in a (normal) military event
	private int offensiveMight = 40;
	//percent chance of defending against invasion etc
	private int defensiveMight = 10;

	private Advisor[] advisors;

	public ManageAdvisors () {
		advisors = new Advisor[3];
		advisors [ECONOMY] = new Advisor ();
		advisors [MILITARY] = new Advisor ();
		advisors [GUILDS] = new Advisor ();

		goldPerTurn = 20;
	}

	public Advisor[] getAdvisors () {
		return advisors;
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
}
