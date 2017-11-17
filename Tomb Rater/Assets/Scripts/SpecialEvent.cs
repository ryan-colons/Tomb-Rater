﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialEvent {

	public static SpecialEventUI eventUI;
	public static GameController gameController;

	private string message;
	private string[] extraMessages = new string[0];
	private int messageIndex = 0;
	private string[] buttonTexts = new string[0];
	private string sprite;

	private bool reuse;

	public void setMessage(string str) {
		message = str;
	}
	public string getMessage() {
		return message;
	}
	public void initialiseExtraMessageArray (int size) {
		this.extraMessages = new string[size];
	}
	public void setExtraMessage(string str, int index) {
		this.extraMessages [index] = str;
	}
	public string[] getButtonTexts () {
		return buttonTexts;
	}
	public void initialiseButtonTexts (int size) {
		this.buttonTexts = new string[size];
	}
	public void setButtonText (string str, int index) {
		this.buttonTexts [index] = str;
	}

	public void go() {
		//do everything
		GameObject panel = eventUI.setEventPanel(this.message);
		addButtons (panel);
	}

	public void next() {
		//progress to next message
		//old panel needs to be removed, so it can't be clicked
		//add a next button if it's not the last one
		GameObject panel = eventUI.setEventPanel(this.extraMessages[messageIndex]);
		messageIndex += 1;
		addButtons (panel);
	}

	public void addButtons(GameObject panel) {
		Button button = panel.GetComponent<Button> ();
		if (messageIndex < extraMessages.Length - 1) {
			button.onClick.AddListener (next);
			EventPanel panelScript = panel.GetComponent<EventPanel> ();
			panelScript.prepareToDestroyThis ();
		} else {
			//provide buttons for the 'options'
			eventUI.setOptionsPanel(this);
		}

	}

	public virtual void option1 () {}
	public virtual void option2 () {}
	public virtual void option3 () {}
	public virtual void option4 () {}
	public virtual void option5 () {}
	public virtual void option6 () {}
	public virtual void option7 () {}
}


public class Event_Introduction : SpecialEvent {
	
	public Event_Introduction () {
		// need to set sprite
		initialiseExtraMessageArray(3);
		this.setMessage("Waking on your birthday, you think for the first time about " +
		"your own mortality. How will your people remember you when you are gone?\n\n...");
		this.setExtraMessage ("You decide immediately that you will build a magnficient tomb, " +
		" to carry your legacy through the ages.\n\n...", 0);
		this.setExtraMessage ("And then a third, final panel with a question...", 1);

		initialiseButtonTexts (5);
		this.setButtonText ("Option one", 0);
		this.setButtonText ("Two", 1);
		this.setButtonText ("A third option with lots of words", 2);
		this.setButtonText ("A fourth option with even more text on it, to test the limits", 3);
		this.setButtonText ("Webster's dictionary defines fifth options as the longest of the long. Check out all this text. Does it even fit??", 4);
	}
}