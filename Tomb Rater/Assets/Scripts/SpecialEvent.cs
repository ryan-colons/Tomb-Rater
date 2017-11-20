using System.Collections;
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
		GameObject panel = eventUI.setEventPanel(this.extraMessages[messageIndex]);
		messageIndex += 1;
		addButtons (panel);
	}

	public void addButtons(GameObject panel) {
		Button button = panel.GetComponent<Button> ();
		if (messageIndex < extraMessages.Length) {
			//if there are extra messages to see, prepare appropriately
			button.onClick.AddListener (next);
			EventPanel panelScript = panel.GetComponent<EventPanel> ();
			panelScript.prepareToDestroyThis ();
		} else {
			//provide buttons for the final decision
			GameObject[] buttonObjects = eventUI.setOptionsPanel(this);
			//add listeners to buttons
			for (int i = 0; i < buttonObjects.Length; i++) {
				switch (i) {
				case 0:
					buttonObjects[i].GetComponent<Button>().onClick.AddListener (option1);
					break;
				case 1:
					buttonObjects[i].GetComponent<Button>().onClick.AddListener (option2);
					break;
				case 2:
					buttonObjects[i].GetComponent<Button>().onClick.AddListener (option3);
					break;
				case 3:
					buttonObjects[i].GetComponent<Button>().onClick.AddListener (option4);
					break;
				case 4:
					buttonObjects[i].GetComponent<Button>().onClick.AddListener (option5);
					break;
				default:
					Debug.Log ("There are too many buttons, and not enough methods! " + buttonObjects.Length);
					break;
				}
			}
		}

	}

	public virtual void option1 () {}
	public virtual void option2 () {}
	public virtual void option3 () {}
	public virtual void option4 () {}
	public virtual void option5 () {}
}


public class Event_Introduction : SpecialEvent {
	
	public Event_Introduction () {
		this.setMessage ("Waking on your birthday, you think for the first time about " +
		"your own mortality. How will your people remember you when you are gone?\n\n...");
		
		initialiseExtraMessageArray(2);
		this.setExtraMessage ("You decide immediately that you will build a magnficient tomb, " +
		"to carry your legacy through the ages.\n\n...", 0);
		this.setExtraMessage ("And then a third, final panel with a question...", 1);

		/* The extra messages here should explain the next menu, ideally.
		 * i.e. make it clear that your talking to advisors about plans.
		 * but also the next screen should have some tutorial panels
		 */

		initialiseButtonTexts (5);
		this.setButtonText ("Go to main scene", 0);
		this.setButtonText ("Two", 1);
		this.setButtonText ("A third option with lots of words", 2);
		this.setButtonText ("A fourth option with even more text on it, to test the limits", 3);
		this.setButtonText ("Webster's dictionary defines fifth options as the longest of the long. Check out all this text. Does it even fit??", 4);
	}

	public override void option1 () {
		gameController.loadScene ("menu");
	}
	public override void option2 () {
		Debug.Log ("2");
	}
	public override void option3 () {
		Debug.Log ("3");
	}
	public override void option4 () {
		Debug.Log ("4");
	}
	public override void option5 () {
		Debug.Log ("5");
	}
}

public class Event_LabourAllocationTutorial : SpecialEvent {

	public Event_LabourAllocationTutorial () {
		this.setMessage ("Well met, Your Majesty! I am here to discuss your plans for " +
		"managing our resources this year.");

		initialiseExtraMessageArray (1);
		this.setExtraMessage ("Okay, well hopefully that explains everything.", 0);

		initialiseButtonTexts (1);
		this.setButtonText ("Let's begin.", 0);
	}

	public override void option1 () {
		gameController.loadScene ("work_map");
	}
}

/* Checklist for writing a SpecialEvent
 * Constructor:
 * 		-initialise extra message array, and set extra messages
 * 		-initialise button texts array, and set button texts
 * 		-set the sprite (??)
 * Implement an option for each button (option1... optionN)
 */