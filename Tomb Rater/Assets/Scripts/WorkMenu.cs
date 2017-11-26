using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkMenu : MonoBehaviour {

	private GameController gameController;
	private ManageResources resourceManagement;
	public GameObject tutPanel;
	public GameObject helpPanel;
	public GameObject advicePanel;

	private void Start () {
		this.gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		this.resourceManagement = gameController.getResourceManagement ();
		if (gameController.workTutorialNeeded ()) {
			tutPanel.SetActive(true);
			gameController.setWorkTutorialNeeded (false);
		}
	}

	public void removeTutPanel () {
		tutPanel.SetActive (false);
	}

	public void toggleHelpPanel () {
		if (helpPanel.activeSelf == true) {
			helpPanel.SetActive (false);
		} else {
			helpPanel.SetActive (true);
		}
	}

	public void toggleAdvicePanel () {
		if (advicePanel.activeSelf == true) {
			advicePanel.SetActive (false);
		} else {
			//get advice text from somewhere
			//can probably cycle through 2-3 pieces of advice when the button is clicked
			advicePanel.SetActive (true);
		}
	}

	public void returnToOverMenu () {
		gameController.loadScene ("menu");
	}

}