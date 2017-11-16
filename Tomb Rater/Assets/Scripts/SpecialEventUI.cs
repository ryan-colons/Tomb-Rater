using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialEventUI : MonoBehaviour {

	public GameObject eventPanelPrefab;
	public static GameController gameController;

	private void Awake () {
		SpecialEvent.eventUI = this;
	}

	private void Start () {
		gameController.getHolsteredEvent ().go ();
	}

	public GameObject setEventPanel (string message) {
		GameObject eventPanel = (GameObject) Instantiate (Resources.Load ("EventPanel"), this.transform);
		eventPanel.transform.SetPositionAndRotation (new Vector3 (10, Screen.height - 10, 0), Quaternion.identity);

		Text eventText = eventPanel.transform.Find ("Text").GetComponent<Text> ();
		eventText.text = message;

		return eventPanel;
	}

}
