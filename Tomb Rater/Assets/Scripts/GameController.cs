using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	private SpecialEvent holsteredEvent;

	private bool overmenuTutorial, workTutorial,  buildTutorial;

	private ManageLabour labourManagement;
	private ManageResources resourceManagement;

	private void Start () {
		SpecialEvent.gameController = this;
		SpecialEventUI.gameController = this;
		ResourceSite.gameController = this;

		overmenuTutorial = true;
		workTutorial = true;
		buildTutorial = true;

		//sites are currently added in the ManageLabour constructor
		labourManagement = new ManageLabour ();
		resourceManagement = new ManageResources ();
	}

	private void Awake () {
		DontDestroyOnLoad (this.gameObject);
	}

	public void loadScene (string sceneName) {
		SceneManager.LoadScene (sceneName);
	}

	public void loadEvent (SpecialEvent specialEvent) {
		this.holsteredEvent = specialEvent;
		loadScene ("event");
	}

	public SpecialEvent getHolsteredEvent () {
		return holsteredEvent;
	}

	public bool overmenuTutorialNeeded () {
		return overmenuTutorial;
	}
	public void setOvermenuTutorialNeeded (bool b) {
		overmenuTutorial = b;
	}
	public bool workTutorialNeeded () {
		return workTutorial;
	}
	public void setWorkTutorialNeeded (bool b) {
		workTutorial = b;
	}
	public bool buildTutorialNeeded () {
		return buildTutorial;
	}
	public void setBuildTutorialNeeded (bool b) {
		buildTutorial = b;
	}

	public ManageResources getResourceManagement() {
		return resourceManagement;
	}
	public ManageLabour getLabourManagement () {
		return labourManagement;
	}
}
