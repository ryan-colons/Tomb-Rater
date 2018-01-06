using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Advisor {

	private int payment;
	private AdvisorMilestone milestone;
	private SpecialEvent tutorialEvent = null;

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
