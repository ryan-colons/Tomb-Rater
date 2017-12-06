using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Year {

	private int yearName;
	private List<SpecialEvent> specialEvents;

	public Year (int n) {
		yearName = n;
		specialEvents = new List<SpecialEvent> ();
	}

	public int getYearName () {
		return yearName;
	}

	public void addSpecialEvent (SpecialEvent newEvent) {
		specialEvents.Add (newEvent);
	}

	public List<SpecialEvent> getEvents () {
		return specialEvents;
	}

}
