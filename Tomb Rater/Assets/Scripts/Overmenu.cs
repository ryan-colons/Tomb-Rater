using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overmenu : MonoBehaviour {

	private GameController gameController;
	public GameObject tutPanel;
	public GameObject helpPanel;

	private void Start () {
		this.gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();

		if (gameController.overmenuTutorialNeeded ()) {
			tutPanel.SetActive(true);
		}
	}

	public void removeTutPanel () {
		tutPanel.SetActive (false);
		gameController.setOvermenuTutorialNeeded (false);
	}

	public void showHelpPanel () {
		helpPanel.SetActive (true);
	}

	public void removeHelpPanel () {
		helpPanel.SetActive (false);
	}

}
