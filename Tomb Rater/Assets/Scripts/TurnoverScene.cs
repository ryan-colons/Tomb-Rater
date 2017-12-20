using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnoverScene : MonoBehaviour {

	public static SpecialEvent[] specialEvents;
	public static int eventIndex;
	public static string yearlyReport;
	private GameController gameController;

	public GameObject reportPanel;
	public Text reportText;

	private void Start () {
		//play animation for a bit, then move to the next thing
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		reportPanel.SetActive (false);
		StartCoroutine("turnover");
	}

	public IEnumerator turnover () {
		yield return new WaitForSeconds (2.5f);
		if (eventIndex < specialEvents.Length) {
			//do next special event
			SpecialEvent nextEvent = specialEvents[eventIndex];
			eventIndex += 1;
			gameController.loadEvent (nextEvent);
		} else {
			//move to end of year
			endYear ();
		}
	}

	public void endYear () {
		ManageYears yearManagement = gameController.getYearManagement();
		Year currentYear = yearManagement.getCurrentYear ();
		reportPanel.SetActive (true);

		string report = "Happy Birthday, Your Majesty!\nHere is the yearly report:\n\nYear " + currentYear.getYearName () + ":\n";
		if (yearlyReport.Equals ("")) {
			report += "Nothing to report this year. No news is good news??";
		}
		report += yearlyReport;
		reportText.text = report;



		yearManagement.progressToNextYear (gameController);
	}

	public void goToOvermenu () {
		gameController.loadScene ("menu");
	}
}
