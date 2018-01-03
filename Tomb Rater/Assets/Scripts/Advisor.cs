using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Advisor {

	private int payment;
	private AdvisorMilestone milestone;

	public string getSpeech () {
		return milestone.getDescription ();
	}

	public int getPayment () {
		return this.payment;
	}
	public void setPayment (int n) {
		this.payment = n;
	}

	public AdvisorMilestone getMilestone () {
		return milestone;
	}
	public void proceedToNextMilestone () {
		milestone = milestone.getNextMilestone ();
	}
}
