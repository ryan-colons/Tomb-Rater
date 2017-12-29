﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* System for choosing special events each year
 * - events can be guaranteed to happen on a certain year, by adding the event via ManageYears
 * - ManageSpecialEvents deals with events that are not guaranteed / tied to a specific year
 * - these events have probabilities; i.e. some events are more likely than others
 * - they also may have prereqs; i.e. they can only happen under certain conditions
 * - some events should only happen once, others are repeatable
 * Events will be stored in a list, and have 'probabilities'
 * NB. the probabilities here are strictly relative
 */
public class ManageSpecialEvents {

	private List<SpecialEvent> possibleEvents;

	public ManageSpecialEvents () {
		possibleEvents = new List<SpecialEvent> ();
		addPossibleEvent (new Event_IncurableTerminalIllness ());
	}

	public SpecialEvent chooseSpecialEventRandomly () {
		int sum = 0;
		foreach (SpecialEvent specialEvent in possibleEvents) {
			sum += specialEvent.getProbability ();
		}
		int randNum = Random.Range (0, sum);
		sum = 0;
		foreach (SpecialEvent specialEvent in possibleEvents) {
			sum += specialEvent.getProbability ();
			if (randNum < sum && specialEvent.prereq()) {
				if (specialEvent.getReuse ()) {
					removePossibleEvent (specialEvent);
				}
				return specialEvent;
			}
		}
		//somehow, no special event was chosen??
		//this could happen if there are no possible events
		return null;
	}

	public void addPossibleEvent (SpecialEvent specialEvent) {
		//should we allow multiple instances of the same event?
		//for now, let's say yes
		possibleEvents.Add(specialEvent);
	}
	public void removePossibleEvent (SpecialEvent specialEvent) {
		//remove all instances of the event
		foreach (SpecialEvent listEvent in possibleEvents) {
			if (specialEvent.isSameAs (listEvent)) {
				possibleEvents.Remove (listEvent);
			}
		}

	}
	public bool listContainsEvent (SpecialEvent specialEvent) {
		foreach (SpecialEvent listEvent in possibleEvents) {
			if (specialEvent.isSameAs (listEvent)) {
				return true;
			}
		}
		return false;
	}

}