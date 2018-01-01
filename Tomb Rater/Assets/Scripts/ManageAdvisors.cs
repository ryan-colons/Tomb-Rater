using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageAdvisors {
	public const int ECONOMY = 0;
	public const int MILITARY = 1;
	public const int GUILDS = 2;

	private Advisor[] advisors;

	public ManageAdvisors () {
		advisors = new Advisor[3];
		advisors [ECONOMY] = new Advisor ();
		advisors [MILITARY] = new Advisor ();
		advisors [GUILDS] = new Advisor ();
	}

	public Advisor[] getAdvisors () {
		return advisors;
	}

}
