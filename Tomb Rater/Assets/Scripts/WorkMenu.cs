using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkMenu : MonoBehaviour {

	private GameController gameController;
	//private ManageResources resourceManagement;
	private ManageLabour labourManagement;
	public GameObject tutPanel;
	public GameObject helpPanel;
	public GameObject advicePanel;

	private void Start () {
		this.gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		//this.resourceManagement = gameController.getResourceManagement ();
		this.labourManagement = gameController.getLabourManagement ();
		if (gameController.workTutorialNeeded ()) {
			tutPanel.SetActive(true);
			gameController.setWorkTutorialNeeded (false);
		}
		setScene ();
	}
		
	public void setScene () {
		ResourceSite[] sites = labourManagement.getSites ();
		foreach (ResourceSite site in sites) {
			float[] coords = ResourceSite.accessCoordLookUp (site);
			instantiateSite (site, coords[0], coords[1]);
			//put the right number of occupants at each site
		}
		//use labourManagement.getNumAvailable citizens to set out worker tokens

	}

	public void instantiateSite (ResourceSite resourceSite, float x, float y) {
		//instantiate Site object (use x and y to set position)
		//then use Site.setResourceSite()
		//always create Sites with this method
		GameObject siteParent = GameObject.Find ("Big Map/Sites");
		if (siteParent == null) {
			Debug.Log ("Couldn't find Big Map/Sites! Something probably got renamed, easy to fix!");
			Application.Quit ();
		}
		GameObject siteObj = (GameObject)Instantiate (Resources.Load ("Site"), siteParent.transform);
		Site site = siteObj.GetComponent<Site> ();
		site.moveTo (x, y);
		site.setResourceSite (resourceSite);
		site.handleAvailability ();
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