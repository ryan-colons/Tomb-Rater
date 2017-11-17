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

	public GameObject[] setOptionsPanel (SpecialEvent specialEvent) {
		GameObject optionsPanel = (GameObject)Instantiate (Resources.Load ("OptionsPanel"), this.transform);
		optionsPanel.transform.SetPositionAndRotation (new Vector3 (Screen.width - 200, Screen.height / 2, 0), Quaternion.identity);

		string[] buttonTexts = specialEvent.getButtonTexts ();
		GameObject[] buttonObjects = new GameObject[buttonTexts.Length];
		for (int i = 0; i < buttonTexts.Length; i++) {
			GameObject button = (GameObject)Instantiate (Resources.Load ("OptionButton"), optionsPanel.transform);
			Text buttonLabel = button.transform.Find ("Text").GetComponent<Text> ();
			buttonLabel.text = buttonTexts [i];
			buttonObjects [i] = button;
		}

		return buttonObjects;
	}

}
