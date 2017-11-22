using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkMenu : MonoBehaviour {

	private GameController gameController;
	public GameObject tutPanel;
	public GameObject helpPanel;
	public GameObject advicePanel;

	private void Start () {
		this.gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();

		if (gameController.workTutorialNeeded ()) {
			tutPanel.SetActive(true);
			gameController.setWorkTutorialNeeded (false);
		}
	}

	public void removeTutPanel () {
		tutPanel.SetActive (false);
	}

	public void showHelpPanel () {
		helpPanel.SetActive (true);
	}

	public void removeHelpPanel () {
		helpPanel.SetActive (false);
	}

	public void showAdvicePanel () {
		//get advice text from somewhere
		//can probably cycle through 2-3 pieces of advice when the button is clicked
		advicePanel.SetActive (true);
	}

	public void removeAdvicePanel () {
		advicePanel.SetActive (false);
	}

}