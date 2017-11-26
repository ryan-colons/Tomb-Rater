using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMenu : MonoBehaviour {

	private GameController gameController;
	//public GameObject tutPanel;
	public GameObject helpPanel;
	//public GameObject advicePanel;

	private void Start () {
		this.gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		/*
		if (gameController.buildTutorialNeeded ()) {
			tutPanel.SetActive(true);
			gameController.setBuildTutorialNeeded (false);
		}
		*/
	}

	public void toggleHelpPanel () {
		if (helpPanel.activeSelf == true) {
			helpPanel.SetActive (false);
		} else {
			helpPanel.SetActive (true);
		}
	}

	public void returnToOverMenu () {
		gameController.loadScene ("menu");
	}
}
