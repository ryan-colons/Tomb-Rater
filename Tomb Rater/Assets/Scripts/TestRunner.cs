using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* A class for running tests! 
 */
public class TestRunner : MonoBehaviour {

	public void Start () {
		Debug.Log ("Testing comparison of events... passed: " + testSpecialEventComparison());
		Debug.Log ("Testing special event list... passed: " + testSpecialEventList ());
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
		public TEST_Event (bool b) {
			this.setReuse(b);
		}
	}
	public bool testSpecialEventList () {
		ManageSpecialEvents specialEventManagement = new ManageSpecialEvents ();

		specialEventManagement.clearPossibleEvents ();
		bool chooseRandFromEmpty = specialEventManagement.chooseSpecialEventRandomly () == null;

		specialEventManagement.addPossibleEvent (new Event_SpecialEventTest ());
		specialEventManagement.addPossibleEvent (new Event_Introduction ());
		specialEventManagement.addPossibleEvent (new Event_Introduction ());
		bool findEventInList = specialEventManagement.listContainsEvent (new Event_SpecialEventTest ());
		bool notFindMissingEvent = !specialEventManagement.listContainsEvent (new Event_IncurableTerminalIllness());

		specialEventManagement.removePossibleEvent (new Event_SpecialEventTest ());
		bool removeEvent = !specialEventManagement.listContainsEvent (new Event_SpecialEventTest ());

		specialEventManagement.clearPossibleEvents ();
		specialEventManagement.addPossibleEvent (new TEST_Event (false));
		specialEventManagement.chooseSpecialEventRandomly ();
		bool nonReusableEvents = !specialEventManagement.listContainsEvent (new TEST_Event (false));

		specialEventManagement.clearPossibleEvents ();
		specialEventManagement.addPossibleEvent (new TEST_Event (true));
		specialEventManagement.chooseSpecialEventRandomly ();
		bool reusableEvents = specialEventManagement.listContainsEvent (new TEST_Event (true));

		return chooseRandFromEmpty && findEventInList && notFindMissingEvent && removeEvent && nonReusableEvents && reusableEvents;
	}

	//*******************************************************************************
	public bool testSpecialEventComparison () {
		TEST_Event event1 = new TEST_Event (true);
		TEST_Event event2 = new TEST_Event (false);
		Event_SpecialEventTest event3 = new Event_SpecialEventTest ();
		bool checkSame1 = event1.isSameAs (event2);
		bool checkSame2 = event2.isSameAs (event1);
		bool checkIdentity = event1.isSameAs (event1);
		bool checkDifferent1 = !event1.isSameAs (event3);
		bool checkDifferent2 = !event3.isSameAs (event1);

		return checkSame1 && checkSame2 && checkIdentity && checkDifferent1 && checkDifferent2;
	}

}
