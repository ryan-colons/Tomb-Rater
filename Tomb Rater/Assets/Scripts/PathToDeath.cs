using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathToDeath {

	private int counter;
	private int lossPerYear;
	private SpecialEvent[] eventPathToDeath;
	private int pathToDeathIndex = 0;
	private int nextThreshold;

	public PathToDeath (SpecialEvent[] events) {
		counter = 100;
		lossPerYear = 2;
		nextThreshold = 75;
		eventPathToDeath = events;
	}

	public void decrement () {
		increment (-1 * lossPerYear);
	}
	public void increment (int n) {
		counter += n;
		checkThreshold ();
	}
	public void setLossPerYear (int n) {
		lossPerYear = n;
	}

	public bool checkThreshold () {
		return counter <= nextThreshold;
	}

	public SpecialEvent triggerNextSpecialEvent () {
		if (pathToDeathIndex < eventPathToDeath.Length) {
			nextThreshold -= 25;
			return eventPathToDeath [pathToDeathIndex++];
		} else {
			return null;
		}
	}

}
