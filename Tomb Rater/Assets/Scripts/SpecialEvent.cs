using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEvent {

	public static SpecialEventUI eventUI;
	public static GameController gameController;

	private string message;
	private string sprite;

	private bool reuse;

	public void setMessage(string str) {
		message = str;
	}
	public string getMessage() {
		return message;
	}

	public void go() {
		//do everything
		eventUI.setEventPanel(this.message);
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
		this.setMessage("Waking on your birthday, you think for the first time about " +
		"your own mortality. How will your people remember you when you are gone?\n");
	}
}