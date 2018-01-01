using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Advisor {

	private int payment;

	public string getSpeech () {
		return "Hi, please buy stuff!";
	}

	public int getPayment () {
		return this.payment;
	}
	public void setPayment (int n) {
		this.payment = n;
	}

}
