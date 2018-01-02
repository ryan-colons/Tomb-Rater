using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvisorMilestone {

	//how much money BEYOND the previous node is required to unlock this
	private int threshold;
	//how much money has been sunk into this milestone already
	private int payment;
	//this string is how the advisor explains/sells the milestone
	private string description;
	private AdvisorMilestone nextMilestone;

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

	public virtual void reward () {
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
