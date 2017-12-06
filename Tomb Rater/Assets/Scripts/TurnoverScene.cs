using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnoverScene : MonoBehaviour {

	public static SpecialEvent[] specialEvents;
	public static int eventIndex;
	private GameController gameController;

	private void Start () {
		//play animation for a bit, then move to the next thing
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
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
			ManageYears yearManagement = gameController.getYearManagement();
			yearManagement.progressToNextYear (gameController);
		}
	}


}
