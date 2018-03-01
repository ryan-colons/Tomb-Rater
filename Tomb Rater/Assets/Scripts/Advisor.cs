using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Advisor {

	private string name;
	private int payment;
	private AdvisorMilestone milestone;
	private SpecialEvent tutorialEvent = null;

	public string getName () {
		if (name == null || name.Equals ("")) {
			name = generateName ();
		}
		return name;
	}
	public static string generateName () {
		string[] name1 = new string[]{"B", "D", "F", "J", "L", "M", "V"};
		string[] name2 = new string[]{"ance", "ohnson", "arryl", "eorge", "amlyn", "argaret", "osiah", "enjamin", "aniel",
		"olomon", "euben", "eborah", "arbara", "illiam", "atheryn", "amuel", "ampbell"};
		return name1 [Random.Range (0, name1.Length)] + name2 [Random.Range (0, name2.Length)];
	}
	public void setName (string str) {
		name = str;
	}

	public string getSpeech () {
		return milestone.getDescription ();
	}

	public int getPayment () {
		return this.payment;
	}
	public void setPayment (int n) {
		this.payment = n;
	}

	public SpecialEvent getTutorial () {
		return tutorialEvent;
	}
	public void setTutorial (SpecialEvent newTutorial) {
		tutorialEvent = newTutorial;
	}

	public AdvisorMilestone getMilestone () {
		return milestone;
	}
	public void proceedToNextMilestone () {
		milestone = milestone.getNextMilestone ();
	}
	public void setMilestone (AdvisorMilestone newMilestone) {
		milestone = newMilestone;
	}
}
