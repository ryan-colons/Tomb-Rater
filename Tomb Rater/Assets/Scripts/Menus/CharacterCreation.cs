using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreation : MonoBehaviour {

	private GameController gameController;

	private string name;
	private string title;
	private string kingdomName;

	public InputField nameField;
	public Dropdown titleDropdown;
	public InputField kingdomField;
	public Text characterText;

	private void Start () {
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
		updateCharacterText();
	}

	public void updateCharacterText () {
		string str = "I am ";
		name = nameField.text;
		kingdomName = kingdomField.text;
		title = titleDropdown.options [titleDropdown.value].text;
		if (!name.Equals ("")) {
			str += name + ", ";
		}
		str += "the " + title + " ";
		if (!kingdomName.Equals ("")) {
			str += "of " + kingdomName;
		}
		characterText.text = str;
	}

	public void validate () {
		if (name == null || name.Equals ("")) {
			reportError ("You must enter a name.");
			return;
		}
		if (kingdomName == null || name.Equals ("")) {
			reportError ("Your Kingdom needs a name.");
			return;
		}
		if (title == null) {
			reportError ("I literally don't know how, but you didn't specify your title.");
			return;
		}
		CharacterData info = gameController.getCharData ();
		info.setPlayerName (name);
		info.setPlayerTitle (title);
		info.setKingdomName (kingdomName);
		info.setPlayerAge (52);

		gameController.loadEvent (new Event_Introduction ());
	}

	public void reportError (string errorMessage) {

	}

	public void returnToTitle () {
		gameController.loadScene ("title");
	}


}
