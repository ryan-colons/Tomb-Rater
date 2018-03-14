using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* A class for running tests! 
 */
public class TestRunner : MonoBehaviour {

	public void Start () {
		Debug.Log ("Testing comparison of events... passed: " + testSpecialEventComparison());
		Debug.Log ("Testing special event list... passed: " + testSpecialEventList ());
		Debug.Log ("Testing special event selection... passed: " + testSpecialEventPicking ());
		Debug.Log ("Testing calendar... passed: " + testCalendar ());
	}

	//*******************************************************************************
	public bool testCalendar () {
		ManageYears yearManagement = new ManageYears (0);
		for (int i = 0; i < 300; i++) {
			yearManagement.progressToNextYear ();
			if (yearManagement.getCurrentYear ().getYearName () != (i+1)) {
				Debug.Log ("Failed! " + yearManagement.getCurrentYear ().getYearName () + " vs. " + i + 1);
				return false;
			}
		}
		return true;
	}

	//*******************************************************************************
	private class TEST_Event : SpecialEvent {
		public TEST_Event (bool b, int p) {
			this.setReuse(b);
			this.setProbability(p);
		}
	}
	public bool testSpecialEventList () {
		ManageSpecialEvents specialEventManagement = new ManageSpecialEvents ();

		specialEventManagement.clearPossibleEvents ();
		bool chooseRandFromEmpty = specialEventManagement.chooseSpecialEventRandomly () == null;

		specialEventManagement.addPossibleEvent (new Event_SpecialEventTest ());
		specialEventManagement.addPossibleEvent (new TEST_Event (false, 5));
		specialEventManagement.addPossibleEvent (new TEST_Event (false, 5));
		bool findEventInList = specialEventManagement.listContainsEvent (new Event_SpecialEventTest ());
		bool notFindMissingEvent = !specialEventManagement.listContainsEvent (new Event_IncurableTerminalIllness());

		specialEventManagement.removePossibleEvent (new Event_SpecialEventTest ());
		bool removeEvent = !specialEventManagement.listContainsEvent (new Event_SpecialEventTest ());

		specialEventManagement.clearPossibleEvents ();
		specialEventManagement.addPossibleEvent (new TEST_Event (false, 5));
		specialEventManagement.chooseSpecialEventRandomly ();
		bool nonReusableEvents = !specialEventManagement.listContainsEvent (new TEST_Event (false, 5));

		specialEventManagement.clearPossibleEvents ();
		specialEventManagement.addPossibleEvent (new TEST_Event (true, 5));
		specialEventManagement.chooseSpecialEventRandomly ();
		bool reusableEvents = specialEventManagement.listContainsEvent (new TEST_Event (true, 5));
		/*
		Debug.Log ("Choose rand from empty: " + chooseRandFromEmpty);
		Debug.Log ("Find event in list: " + findEventInList);
		Debug.Log ("Don't find an absent event: " + notFindMissingEvent);
		Debug.Log ("Successful removal: " + removeEvent);
		Debug.Log ("Nonreusables: " + nonReusableEvents);
		Debug.Log ("Reuseables: " + reusableEvents);
		*/
		return chooseRandFromEmpty && findEventInList && notFindMissingEvent && removeEvent && nonReusableEvents && reusableEvents;
	}

	//*******************************************************************************
	public bool testSpecialEventComparison () {
		TEST_Event event1 = new TEST_Event (true, 1);
		TEST_Event event2 = new TEST_Event (false, 2);
		Event_SpecialEventTest event3 = new Event_SpecialEventTest ();
		bool checkSame1 = event1.isSameAs (event2);
		bool checkSame2 = event2.isSameAs (event1);
		bool checkIdentity = event1.isSameAs (event1);
		bool checkDifferent1 = !event1.isSameAs (event3);
		bool checkDifferent2 = !event3.isSameAs (event1);

		return checkSame1 && checkSame2 && checkIdentity && checkDifferent1 && checkDifferent2;
	}
	//*******************************************************************************
	public bool testSpecialEventPicking () {
		ManageSpecialEvents specialEventManagement = new ManageSpecialEvents ();

		TEST_Event leastLikely = new TEST_Event (true, 1);
		TEST_Event likely = new TEST_Event (true, 2);
		TEST_Event mostLikely = new TEST_Event (true, 4);

		specialEventManagement.addPossibleEvent (leastLikely);
		specialEventManagement.addPossibleEvent (likely);
		specialEventManagement.addPossibleEvent (mostLikely);

		int least = 0, middle = 0, most = 0;
		for (int i = 0; i < 10000; i++) {
			SpecialEvent chosenEvent = specialEventManagement.chooseSpecialEventRandomly ();
			if (chosenEvent == leastLikely)
				least++;
			else if (chosenEvent == likely)
				middle++;
			else if (chosenEvent == mostLikely)
				most++;
		}
		Debug.Log ("Least: " + least);
		Debug.Log ("Middle: " + middle);
		Debug.Log ("Most: " + most);
		return least <= middle && middle <= most;
	}

}
