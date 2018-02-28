using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvisorMenu : MonoBehaviour {

	private GameController gameController;
	public GameObject tutPanel;
	public GameObject helpPanel;
	public AdvisorButton[] advisorObjs;

	private void Start () {
		this.gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		placeAdvisors ();
	}

	public bool validateMoneyAllocation () {
		int sum = 0;
		foreach (AdvisorButton button in advisorObjs) {
			sum += button.getPayment ();
		}
		if (sum <= gameController.getMoney ()) {
			return true;
		} else {
			return false;
		}
	}

	public void closeAllAdvisorCanvases () {
		foreach (AdvisorButton button in advisorObjs) {
			button.closeCanvas ();
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

	public void placeAdvisors () {
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement ();
		Advisor[] advisors = advisorManagement.getAdvisors ();
		if (advisors.Length != advisorObjs.Length) {
			Debug.Log ("Error! Need a 1:1 ratio of gameobjects : advisor scripts!");
			Application.Quit ();
		}
		for (int i = 0; i < advisors.Length; i++) {
			if (advisors [i] != null) {
				advisorObjs [i].gameObject.SetActive (true);
				advisorObjs [i].setAdvisor (advisors [i]);
				advisorObjs [i].nameTag.text = advisors [i].getName ();
			} else {
				advisorObjs [i].gameObject.SetActive (false);
			}
		}
	}

	public void saveAndQuit () {
		//gameController.save ();
		Application.Quit ();
	}

}
