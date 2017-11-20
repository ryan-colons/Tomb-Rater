using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvisorButton : MonoBehaviour {

	private GameController gameController;

	private void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}

	private void OnMouseDown () {
		if (!gameController.workTutorialNeeded ()) {
			gameController.loadScene ("work_map");
		} else {
			gameController.loadEvent (new Event_LabourAllocationTutorial ());
		}
	}

}
